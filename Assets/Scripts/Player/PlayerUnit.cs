using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using DG.Tweening;

/// <summary>
/// プレイヤーの制御を行う
/// </summary>
public class PlayerUnit : ActorUnit
{
    [Header("各種ダメージ演出に必要")]
    [SerializeField] Animator _anim;

    void Update()
    {
        
    }

    /// <summary>ダメージを受けた際の演出</summary>
    protected override void OnDamageReceived()
    {
        // 未実装
    }

    protected override void OnDeath()
    {
        gameObject.SetActive(false);
    }

    /// <summary>ダメージを与えた際の演出</summary>
    protected override void OnDamageSended()
    {
        // アニメーションを一時停止する
        _anim.speed = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(InGameUtility.HitStopTime);
        sequence.AppendCallback(() => _anim.speed = 1.0f);

        // カメラを揺らす
        float duration = InGameUtility.HitStopTime;
        Vector3 strength = new Vector3(1f, 1f, 0);
        int vibratio = 20;
        CameraController.Shake(duration, strength, vibratio);
    }
}
