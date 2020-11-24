using System.Collections;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yumineko.Fade;
using Yumineko.Lib;

namespace Yumineko.SceneManagement
{
    public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
    {
        private UniTask _load = default;

        private void Reset()
        {
            gameObject.name = "Scene Loader";
        }

        /// <summary>
        /// シーンをロードして切り替える。
        /// </summary>
        /// <param name="sceneName"></param>
        public void Load(string sceneName)
        {
            if (_load.IsCompleted)
            {
                _load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).ToUniTask();
            }
            else
            {
                Debug.LogWarning("既にシーンをロード中です！");
            }
        }

        /// <summary>
        /// フェードしながらシーンを読み込み切り替える
        /// </summary>
        /// <param name="sceneName">読み込むシーン名</param>
        /// <param name="outTime">フェードアウト時間</param>
        /// <param name="inTime">フェードイン時間</param>
        /// <param name="mode">Additiveは追加、Singleは上書き切替</param>
        public void FadeLoad(string sceneName, float outTime = 1.0f, float inTime = 1.0f,
            LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (_load.IsCompleted)
            {
                _load = FadeLoadAsync(sceneName, outTime, inTime, mode);
            }
            else
            {
                Debug.LogWarning("既にシーンをロード中です！");
            }
        }

        /// <summary>
        /// Inspectorからシーンを切り替える。パラメータはScriptableObjectで指定する
        /// </summary>
        /// <param name="parameter"></param>
        public void FadeLoadForInspector(LoadParameter parameter)
        {
            if (_load.IsCompleted)
            {
                _load = FadeLoadAsync(parameter.SceneName, parameter.FadeOutTime, parameter.FadeInTime, parameter.Mode);
            }
            else
            {
                Debug.LogWarning("既にシーンをロード中です！");
            }
        }

        private async UniTask FadeLoadAsync(string sceneName, float outTime = 1.0f, float inTime = 1.0f,
            LoadSceneMode mode = LoadSceneMode.Single)
        {
            await Fader.Instance.FadeOut(outTime);
            await SceneManager.LoadSceneAsync(sceneName, mode);
            await Fader.Instance.FadeIn(inTime);
        }
    }
}