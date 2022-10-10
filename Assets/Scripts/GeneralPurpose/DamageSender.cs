using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// �R���C�_�[���t�����I�u�W�F�N�g�ɃA�^�b�`���邾��
/// <summary>
/// ���̃X�N���v�g���t�����I�u�W�F�N�g�̃R���C�_�[���ΏۂƂԂ������ꍇ
/// �Ώۂɂ��Ă���DamageReceiver�Ƀ_���[�W��^�������Ƃ�ʒm����
/// </summary>
public class DamageSender : MonoBehaviour
{
    /// <summary>�_���[�W�̒l</summary>
    [SerializeField] int _damage;
    /// <summary>���肷��^�O</summary>
    [SerializeField] string _hitTag;

    /// <summary>�_���[�W��^�������ɍs���ǉ��̏���</summary>
    public UnityAction OnDamageSended;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_hitTag))
        {
            // �R���C�_�[���Ԃ������ʒu���擾
            Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
            // �_���[�W��^�������̏������Ăяo��
            ExecuteEvents.Execute<IDamageable>(other.gameObject, null, (reciever, _) => reciever.OnDamage(_damage, hitPos));
            OnDamageSended?.Invoke();
        }
    }
}
