using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// プレイヤーのステータスを管理する
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReciever;
    [SerializeField] DamageSender _damageSender;

    [Header("各種ダメージ演出に必要")]
    [SerializeField] Animator _anim;
    /// <summary>プレイヤーの体力</summary>
    [SerializeField] int _maxLife;
    int _currentLife;

    void Start()
    {
        
    }

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

    void Update()
    {
        
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
