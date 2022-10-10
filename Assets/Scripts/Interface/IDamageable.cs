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
    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="value">�_���[�W��</param>
    /// <param name="hitPos">�_���[�W��^�����I�u�W�F�N�g�ƐڐG�������W</param>
    void OnDamage(int value, Vector3 hitPos);
}
