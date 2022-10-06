using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 敵がダメージを受けた与えたときの処理を登録する
/// </summary>
public class EnemyDamageHitter : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReceiver;
    [SerializeField] DamageSender _damageSender;
    [SerializeField] Renderer _enemyBodyRenderer;
    [SerializeField] Color _damageColor = Color.red;
    Color _originalColor;

    void Start()
    {
        if (_enemyBodyRenderer != null)
        {
            _originalColor = _enemyBodyRenderer.material.color;
        }
    }

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

    void OnDamageReceived()
    {
        Sequence seqE = DOTween.Sequence();
        // ディレイをかけて遅らせる
        //seqE.SetDelay(EffectConst.HitStopTime);
        // 一時停止の代わりに振動処理を追加
        seqE.Append(transform.DOShakePosition(EffectConst.HitStopTime, 0.15f, 25, fadeOut: false));

        if (_enemyBodyRenderer != null)
        {
            seqE.OnStart(() => _enemyBodyRenderer.material.color = _damageColor)
                .Insert(0, _enemyBodyRenderer.material.DOColor(_originalColor, 0.3f));
        }
    }

    void OnDamageSended()
    {
        // 未実装
    }
}
