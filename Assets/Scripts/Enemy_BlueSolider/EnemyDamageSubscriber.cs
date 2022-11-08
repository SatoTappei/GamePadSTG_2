using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// DamageReceiverとDamageSenderに
/// 敵がダメージを受けた与えたときの処理を登録する
/// 現在未使用、消さないこと
/// </summary>
public class EnemyDamageSubscriber : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReceiver;
    [SerializeField] DamageSender _damageSender;

    void OnEnable()
    {
        _damageReceiver.OnDamageReceived += OnDamageReceived;
        _damageSender.OnDamageSended += OnDamageSended;
    }

    void OnDisable()
    {
        _damageSender.OnDamageSended -= OnDamageSended;
        _damageReceiver.OnDamageReceived -= OnDamageReceived;
    }

    /// <summary>ダメージを受けた際の演出</summary>
    void OnDamageReceived()
    {
        transform.DOShakePosition(ConstValue.HitStopTime, 0.15f, 25, fadeOut: false);
        // ダメージを受けたときに死んだかどうか判定したい
        // HPを減らして0以下だったらと否かで分岐する
    }

    /// <summary>ダメージを与えた際の演出</summary>
    void OnDamageSended()
    {
        // 未実装
    }
}
