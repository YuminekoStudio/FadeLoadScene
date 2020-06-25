using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneComponent : MonoBehaviour {
    public float FadeOutTime { get; set; }
    public float FadeInTime { get; set; }
    public void FadeLoad (string sceneName) {
        if (Math.Abs(FadeOutTime) <= 0) FadeOutTime = 1.0f;
        if (Math.Abs(FadeInTime) <= 0) FadeInTime = 1.0f;
        SceneLoadManager.Instance.FadeLoad (sceneName, FadeOutTime, FadeInTime);
    }

    public void Load (string sceneName) {
        SceneLoadManager.Instance.Load (sceneName);
    }
}