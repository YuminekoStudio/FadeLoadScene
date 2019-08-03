using System.Collections;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class Fader : SingletonMonoBehaviour<Fader> {
    private Canvas faderCanvas = null;
    private Image blackImage = null;
    private UniTask fade = default;

    void Reset () {
        gameObject.name = "Fader";
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
        Debug.Log("");
        if (fade.IsCompleted) {
            fade = Fade (true, time);
        } else {
            Debug.LogWarning ("既に別のフェードを実行中です！");
        }
    }

    public void FadeIn (float time) {
        if (fade.IsCompleted) {
            fade = Fade (false, time);
        } else {
            Debug.LogWarning ("既に別のフェードを実行中です！");
        }
    }

    public async UniTask Fade(bool fadeout, float time){
        float alpha = fadeout ? 0.0f : 1.0f;
        float countTime = 0.0f;
        blackImage.enabled = true;
        blackImage.SetAlpha (alpha);
        await UniTask.DelayFrame(1);

        while (countTime < time) {
            countTime += Time.deltaTime;
            alpha = fadeout ? countTime / time : 1.0f - countTime / time;
            blackImage.SetAlpha (alpha);
            await UniTask.DelayFrame(1);
        }
        alpha = fadeout ? 1.0f : 0.0f;
        blackImage.SetAlpha (alpha);
        blackImage.enabled = fadeout;
    }
}