using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �eUnit�N���X(EnemyUnit,PlayerUnit)�̊��N���X
/// </summary>
public abstract class ActorUnit : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReciever;
    [SerializeField] DamageSender _damageSender;

    void OnEnable()
    {
        if(_damageReciever != null)
        {
            _damageReciever.OnDamageReceived += OnDamageReceived;
        }
        if(_damageSender != null)
        {
            _damageSender.OnDamageSended += OnDamageSended;
        }
    }

    void OnDisable()
    {
        if (_damageReciever != null)
        {
            _damageReciever.OnDamageReceived -= OnDamageReceived;
        }
        if (_damageSender != null)
        {
            _damageSender.OnDamageSended -= OnDamageSended;
        }
    }

    protected abstract void OnDamageReceived();
    protected abstract void OnDamageSended();
}
