using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Yumineko.SceneManagement
{
    /// <summary>
    /// シーン切替用パラメータ。Inspectorから切り替えたい時に使う
    /// </summary>
    [CreateAssetMenu]
    public class LoadParameter : ScriptableObject
    {
        public string SceneName;
        public float FadeInTime = 0.5f;
        public float FadeOutTime = 0.5f;
        public LoadSceneMode Mode = LoadSceneMode.Single;
    }
}
