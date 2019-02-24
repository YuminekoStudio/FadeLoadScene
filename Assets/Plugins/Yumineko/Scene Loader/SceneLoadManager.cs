using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager> {
    IEnumerator load = null;
    void Reset () {
        if (GameObject.FindObjectOfType<Fader> () == null) {
            gameObject.AddComponent<Fader> ();
        }
        gameObject.name = "SceneLoader";
    }

    public void Load (string sceneName) {
        if (load == null) {
            load = LoadAsync (sceneName);
            StartCoroutine (load);
        } else {
            Debug.LogWarning ("既にシーンをロード中です！");
        }
    }
    IEnumerator LoadAsync (string sceneName) {
        string unloadSceneName = SceneManager.GetActiveScene ().name;
        AsyncOperation async;
        yield return null;
        async = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
        while (async.progress < 1.0f) {
            yield return new WaitForEndOfFrame ();
        }
        async = SceneManager.UnloadSceneAsync (unloadSceneName);
        while (async.progress < 1.0f) {
            yield return new WaitForEndOfFrame ();
        }
        load = null;
        yield return null;
    }

    public void FadeLoad (string sceneName, float outTime = 1.0f, float inTime = 1.0f) {
        if (load == null) {
            load = FadeLoadAsync (sceneName, outTime, inTime);
            StartCoroutine (load);
        } else {
            Debug.LogWarning ("既にシーンをロード中です！");
        }
    }

    IEnumerator FadeLoadAsync (string sceneName, float outTime = 1.0f, float inTime = 1.0f) {
        string unloadSceneName = SceneManager.GetActiveScene ().name;
        AsyncOperation async = default;
        IEnumerator fade = Fader.Instance.Fade (fadeout: true, time: outTime);
        yield return null;

        //  シーンを非同期で読み込み（切り替えない）
        async = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
        async.allowSceneActivation = false;

        //  フェードアウト開始
        StartCoroutine (fade);

        //  フェードアウトが完了し、シーンの読み込みが終わるまで待機
        while (fade.Current != null || async.progress < 0.9f) {
            yield return new WaitForEndOfFrame ();
        }

        //  シーンの切り替えを許可
        async.allowSceneActivation = true;
        yield return new WaitForEndOfFrame ();

        //  シーンの切り替えが終わるまで待機
        while (async.progress < 1.0f) {
            yield return new WaitForEndOfFrame ();
        }

        async = SceneManager.UnloadSceneAsync (unloadSceneName);
        while (async.progress < 1.0f) {
            yield return new WaitForEndOfFrame ();
        }

        //  フェードイン開始
        fade = Fader.Instance.Fade (fadeout: false, time: inTime);
        StartCoroutine (fade);
        while (fade.Current != null) {
            yield return new WaitForEndOfFrame ();
        }

        load = null;
        yield return null;
    }
}