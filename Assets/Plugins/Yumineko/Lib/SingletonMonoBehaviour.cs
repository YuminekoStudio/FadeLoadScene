﻿using System;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    public bool dontDestroyOnLoad = true;
    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                Type t = typeof (T);

                instance = (T) FindObjectOfType (t);
                if (instance == null) {
                    GameObject obj = new GameObject ();
                    obj.AddComponent<T> ();
                    obj.name = typeof (T).ToString ();
                    Debug.LogError (t + " をアタッチしているGameObjectはありませんでした。生成します。");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake () {
        // 他のGameObjectにアタッチされているか調べる.
        // アタッチされている場合は破棄する.
        if (this != Instance) {
            Destroy (this);
            //Destroy(this.gameObject);
            Debug.LogError (
                typeof (T) +
                " は既に他のGameObjectにアタッチされているため、コンポーネントを破棄しました." +
                " アタッチされているGameObjectは " + Instance.gameObject.name + " です.");
            return;
        }

        if (dontDestroyOnLoad)
            DontDestroyOnLoad (this.gameObject);
    }

}