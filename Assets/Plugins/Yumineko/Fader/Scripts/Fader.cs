using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;

public class Fader : SingletonMonoBehaviour<Fader>
{
    private Animator _anim;
    private Animator Anim => this._anim ? this._anim : this._anim = GetComponent<Animator>();
    private Image _image;
    private Image Image => this._image ? this._image : this._image = GetComponent<Image>();

    /// <summary>
    /// フェード可能か？（他のフェードが実行中でないか）
    /// </summary>
    public bool Available => Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle");

    private static readonly int FadeInHash = Animator.StringToHash("Fade In");
    private static readonly int FadeOutHash = Animator.StringToHash("Fade Out");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");


    public async UniTask FadeIn(float time = 0.5f)
    {
        if (Available)
        {
            Anim.SetFloat(SpeedHash, GetAnimatorSpeed(time));
            Anim.SetTrigger(FadeInHash);
            await UniTask.Delay(TimeSpan.FromSeconds(time));
        }
        else
        {
            Debug.Log("すでにフェードが実行中です");
        }
    }

    public async UniTask FadeOut(float time = 0.5f)
    {
        if (Available)
        {
            Anim.SetFloat(SpeedHash, GetAnimatorSpeed(time));
            Anim.SetTrigger(FadeOutHash);
            await UniTask.Delay(TimeSpan.FromSeconds(time));
        }
        else
        {
            Debug.Log("すでにフェードが実行中です");
        }
    }

    /// <summary>
    /// UnityEvent用（Scriptから使ってもいい）
    /// </summary>
    public async void FadeInForInspector(float time = 0.5f)
    {
        await FadeIn(time);
    }

    /// <summary>
    /// UnityEvent用（Scriptから使ってもいい）
    /// </summary>
    public async void FadeOutForInspector(float time = 0.5f)
    {
        await FadeOut(time);
    }

    private static float GetAnimatorSpeed(float time)
    {
        return (time <= 0) ? 1f : (1f / time);
    }
}