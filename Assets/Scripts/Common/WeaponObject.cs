using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �L�����N�^�[���U������ۂ̕���ƂȂ�I�u�W�F�N�g�ɃA�^�b�`����
/// <summary>
/// ����̃R���C�_�[�̃I���I�t�̃��\�b�h���錾���ꂽ
/// �C���^�[�t�F�[�X�����������N���X
/// </summary>
public class WeaponObject : MonoBehaviour, IAttackAnimControl
{
    Collider _weaponCollider;

    void Awake()
    {
        _weaponCollider = GetComponent<Collider>();
        _weaponCollider.enabled = false;
    }

    void Update()
    {
        
    }

    /// <summary>�R���C�_�[��L���ɂ��ē����蔻����o��</summary>
    public void OnAnimEnter()
    {
        if (_weaponCollider != null)
            _weaponCollider.enabled = true;
    }

    /// <summary>�R���C�_�[�𖳌��ɂ��ē����蔻�������</summary>
    public void OnAnimExit()
    {
        if (_weaponCollider != null)
            _weaponCollider.enabled = false;
    }
}
