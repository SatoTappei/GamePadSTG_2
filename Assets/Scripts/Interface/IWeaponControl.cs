using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ����̃R���C�_�[�̃I���I�t��؂�ւ���C���^�[�t�F�[�X
/// EventSystem�ɂ�郁�b�Z�[�W���M���g��
/// </summary>
public interface IWeaponControl : IEventSystemHandler
{
    /// <summary>�U���̃A�j���[�V�������ɃR���C�_�[���I���ɂ���</summary>
    void EnableCollider();
    /// <summary>�U���̃A�j���[�V�����I�����ɃR���C�_�[���I�t�ɂ���</summary>
    void DisableCollider();
}
