using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// プレイヤーがダメージを受けた与えたときの処理を登録する
/// </summary>
public class PlayerDamageHitter : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReciever;
    [SerializeField] DamageSender _damageSender;
    /// <summary>
    /// ヒットストップ実装のテスト用
    /// これを消してもヒットストップが無くなるだけで他に影響はない
    /// </summary>
    [SerializeField] Animator _anim;
 
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

    void OnDamageReceived()
    {
        // 未実装
    }

    void OnDamageSended()
    {
        // アニメーションを一時停止してヒットストップの実装
        _anim.speed = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(EffectConst.HitStopTime);
        sequence.AppendCallback(() => _anim.speed = 1.0f);
    }
}
