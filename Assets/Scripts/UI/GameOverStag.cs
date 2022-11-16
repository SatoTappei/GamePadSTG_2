using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ゲームオーバーの演出を行う
/// </summary>
public class GameOverStag : MonoBehaviour
{
    [SerializeField] Transform _text;

    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>演出を再生する</summary>
    public void Play()
    {
        transform.localScale = Vector3.one;
        Sequence seq = DOTween.Sequence();
        seq.Append(_text.DOMoveY(800, 0.5f).SetEase(Ease.InCubic))
           .Append(_text.DORotate(new Vector3(0,0,-12),0.1f).SetDelay(1.0f));
    }
}
