using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// DamageReceiver��DamageSender��
/// �G���_���[�W���󂯂��^�����Ƃ��̏�����o�^����
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

    /// <summary>�_���[�W���󂯂��ۂ̉��o</summary>
    void OnDamageReceived()
    {
        transform.DOShakePosition(ConstValue.HitStopTime, 0.15f, 25, fadeOut: false);
    }

    /// <summary>�_���[�W��^�����ۂ̉��o</summary>
    void OnDamageSended()
    {
        // ������
    }
}