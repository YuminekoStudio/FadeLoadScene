using System.Collections;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager> {
    private UniTask _load = default;

    private void Reset () {
        gameObject.name = "Scene Loader";
    }

    public void Load (string sceneName) {
        if (_load.IsCompleted) {
            _load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).ToUniTask();
        } else {
            Debug.LogWarning ("既にシーンをロード中です！");
        }
    }

    public void FadeLoad (string sceneName, float outTime = 1.0f, float inTime = 1.0f) {
        if (_load.IsCompleted) {
            _load = FadeLoadAsync (sceneName, outTime, inTime);
        } else {
            Debug.LogWarning ("既にシーンをロード中です！");
        }
    }

    private async UniTask FadeLoadAsync (string sceneName, float outTime = 1.0f, float inTime = 1.0f) {
        await Fader.Instance.FadeOut(outTime);
        await SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Single);
        await Fader.Instance.FadeIn (inTime);
    }
}