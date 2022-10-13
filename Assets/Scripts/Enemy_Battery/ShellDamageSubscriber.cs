using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DamageSender�ɖC�e���_���[�W��^�����Ƃ��̏�����o�^����
/// </summary>
public class ShellDamageSubscriber : MonoBehaviour
{
    [SerializeField] DamageSender _damageSender;

    void OnEnable()
    {
        _damageSender.OnDamageSended += OnDamageSended;
    }

    void OnDisable()
    {
        _damageSender.OnDamageSended -= OnDamageSended;
    }

    void OnDamageSended()
    {
        // �I�u�W�F�N�g�v�[���ɖ߂�
        gameObject.SetActive(false);
    }
}
