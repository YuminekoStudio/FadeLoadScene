using System.Collections;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager> {
    UniTask load = default;
    void Reset () {
        if (GameObject.FindObjectOfType<Fader> () == null) {
            gameObject.AddComponent<Fader> ();
        }
        gameObject.name = "Scene Loader";
    }

    public void Load (string sceneName) {
        if (load.IsCompleted) {
            load = LoadAsync (sceneName);
        } else {
            Debug.LogWarning ("既にシーンをロード中です！");
        }
    }
    async UniTask LoadAsync (string sceneName) {
        string unloadSceneName = SceneManager.GetActiveScene ().name;
        await SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
        await SceneManager.UnloadSceneAsync (unloadSceneName);
    }

    public void FadeLoad (string sceneName, float outTime = 1.0f, float inTime = 1.0f) {
        if (load.IsCompleted) {
            load = FadeLoadAsync (sceneName, outTime, inTime);
        } else {
            Debug.LogWarning ("既にシーンをロード中です！");
        }
    }

    async UniTask FadeLoadAsync (string sceneName, float outTime = 1.0f, float inTime = 1.0f) {
        string unloadSceneName = SceneManager.GetActiveScene ().name;
        var f = Fader.Instance;
        await UniTask.DelayFrame (1);

        var loadTask = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
        loadTask.allowSceneActivation = false;

        await Fader.Instance.Fade (fadeout: true, time: outTime);;

        loadTask.allowSceneActivation = true;
        await loadTask;

        await SceneManager.UnloadSceneAsync (unloadSceneName);
        await Fader.Instance.Fade (fadeout: false, time: inTime);
    }
}