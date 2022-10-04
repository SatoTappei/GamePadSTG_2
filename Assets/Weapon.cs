using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����̃R���C�_�[�̃I���I�t�̃��\�b�h���錾���ꂽ�C���^�[�t�F�[�X��
/// ���������N���X
/// </summary>
public class Weapon : MonoBehaviour, IWeaponControl
{
    Collider _weaponCollider;

    void Start()
    {
        _weaponCollider = GetComponent<Collider>();
        _weaponCollider.enabled = false;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Animator�ɂ�����Attack�X�e�[�g�̊J�n���Ɉꎞ�I�ɕ���̃R���C�_�[��
    /// �L���ɂ��邽�߂̃R�[���o�b�N���\�b�h
    /// </summary>
    public void EnableWeaponCollider()
    {
        if (_weaponCollider != null)
            _weaponCollider.enabled = true;
    }

    /// <summary>
    /// Animator�ɂ�����Attack�X�e�[�g�̏I�����Ɉꎞ�I�ɕ���̃R���C�_�[��
    /// �����ɂ��邽�߂̃R�[���o�b�N���\�b�h
    /// </summary>
    public void DisableWeaponCollider()
    {
        if (_weaponCollider != null)
            _weaponCollider.enabled = false;
    }
}
