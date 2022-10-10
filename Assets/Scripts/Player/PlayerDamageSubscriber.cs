using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// DamageReceiverとDamageSenderに
/// プレイヤーがダメージを受けた与えたときの処理を登録する
/// </summary>
public class PlayerDamageSubscriber : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReciever;
    [SerializeField] DamageSender _damageSender;

    [Header("各種ダメージ演出に必要")]
    [SerializeField] Animator _anim;
    /// <summary>
    /// ヒットストップ実装のテスト用
    /// これを消してもヒットストップが無くなるだけで他に影響はない
    /// </summary>
    //[SerializeField] Animator _anim;

    void OnEnable()
    {
        _damageReciever.OnDamageReceived += OnDamageReceived;
        _damageSender.OnDamageSended += OnDamageSended;
    }

    void OnDisable()
    {
        _damageReciever.OnDamageReceived -= OnDamageReceived;
        _damageSender.OnDamageSended -= OnDamageSended;
    }

    /// <summary>ダメージを受けた際の演出</summary>
    void OnDamageReceived()
    {
        // 未実装
    }

    /// <summary>ダメージを与えた際の演出</summary>
    void OnDamageSended()
    {
        // アニメーションを一時停止する
        _anim.speed = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(ConstValue.HitStopTime);
        sequence.AppendCallback(() => _anim.speed = 1.0f);

        // カメラを揺らす
        float duration = ConstValue.HitStopTime;
        Vector3 strength = new Vector3(1f, 1f, 0);
        int vibratio = 20;
        CameraController.Shake(duration, strength, vibratio);
    }
}
