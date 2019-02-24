using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Extention {
    /// <summary>
    /// コンポーネントを取得。
    /// 存在しなければ追加してから取得
    /// </summary>
    /// <typeparam name="T">取得するコンポーネントの型</typeparam>
    public static T GetOrAddComponent<T> (this GameObject self) where T : Component {
        return self.GetComponent<T> () ?? self.AddComponent<T> ();
    }

    /// <summary>
    /// RGBはそのままに、透明度を設定
    /// </summary>
    /// <param name="alpha">透明度（1が不透明、0が透明）</param>
    public static void SetAlpha (this Graphic self, float alpha) {
        var color = self.color;
        color.a = alpha;
        self.color = color;
    }
}