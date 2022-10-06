using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// ���̃X�N���v�g���t�����I�u�W�F�N�g�̃R���C�_�[���ΏۂƂԂ������ꍇ
/// �Ώۂɂ��Ă���DamageReceiver�Ƀ_���[�W��^�������Ƃ�ʒm����
/// </summary>
public class DamageSender : MonoBehaviour
{
    /// <summary>�_���[�W�̒l</summary>
    [SerializeField] int _damage;
    /// <summary>�_���[�W��^����G�Ƃ��ĔF������^�O</summary>
    [SerializeField] string _effectiveTag;

    public event Action OnDamageSended;

    void OnTriggerEnter(Collider other)
    {
        // �G�Ƀq�b�g�������ǂ���
        if (other.gameObject.CompareTag(_effectiveTag))
        {
            // �R���C�_�[���Ԃ������ʒu���擾
            Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
            // �_���[�W�������Ăяo��
            ExecuteEvents.Execute<IDamageable>(other.gameObject, null, (reciever, eventData) => reciever.OnDamage(_damage, hitPos));
            // �R�[���o�b�N����
            OnDamageSended?.Invoke();
        }
    }
}
