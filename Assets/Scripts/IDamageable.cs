using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �_���[�W���󂯂鏈�����錾���ꂽ�C���^�[�t�F�[�X
/// EventSystem�ɂ�郁�b�Z�[�W���M���g��
/// </summary>
public interface IDamageable : IEventSystemHandler
{
    void OnDamage(int damageValue, Vector3 hitPos);
}
