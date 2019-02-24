using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : SingletonMonoBehaviour<Fader> {
    private Canvas faderCanvas = null;
    private Image blackImage = null;
    private IEnumerator fade = null;

    void Reset () {
        Debug.Log ("Reset");
        gameObject.GetOrAddComponent<CanvasScaler> ();
        faderCanvas = gameObject.GetOrAddComponent<Canvas> ();
        faderCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        faderCanvas.sortingOrder = 9999;
        gameObject.GetOrAddComponent<GraphicRaycaster> ();

        blackImage = gameObject.GetOrAddComponent<Image> ();
        blackImage.color = Color.black;
        blackImage.SetAlpha (0.0f);
        blackImage.enabled = false;
    }

    void Start () {
        Reset ();
    }

    public void FadeOut (float time) {
        if (fade == null) {
            fade = Fade (true, time);
            StartCoroutine (fade);
        } else {
            Debug.LogWarning ("既に別のフェードを実行中です！");
        }
    }

    public void FadeIn (float time) {
        if (fade == null) {
            fade = Fade (false, time);
            StartCoroutine (fade);
        } else {
            Debug.LogWarning ("既に別のフェードを実行中です！");
        }
    }

    public IEnumerator Fade (bool fadeout, float time) {
        float alpha = fadeout ? 0.0f : 1.0f;
        float countTime = 0.0f;
        blackImage.enabled = true;
        blackImage.SetAlpha (alpha);
        yield return new WaitForEndOfFrame ();

        while (countTime < time) {
            countTime += Time.deltaTime;
            alpha = fadeout ? countTime / time : 1.0f - countTime / time;
            blackImage.SetAlpha (alpha);
            yield return new WaitForEndOfFrame ();
        }
        alpha = fadeout ? 1.0f : 0.0f;
        blackImage.SetAlpha (alpha);
        fade = null;
        blackImage.enabled = fadeout;
        yield return null;
    }
}