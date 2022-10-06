using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �v���C���[�̕���̃R���C�_�[�̐�������邽�߂̃C���^�[�t�F�[�X
/// EventSystem�ɂ�郁�b�Z�[�W���M���g��
/// </summary>
public interface IWeaponControl : IEventSystemHandler
{
    void EnableWeaponCollider();

    void DisableWeaponCollider();
}
