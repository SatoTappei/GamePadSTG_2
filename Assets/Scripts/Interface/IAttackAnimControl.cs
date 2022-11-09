using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �U���̃A�j���[�V�����̍Đ����ɌĂ΂�鏈�����錾���ꂽ�C���^�[�t�F�[�X
/// EventSystem�ɂ�郁�b�Z�[�W���M���g��
/// </summary>
public interface IAttackAnimControl : IEventSystemHandler
{
    /// <summary>�U���̃A�j���[�V�����Đ�����1�񂾂��Ă΂��</summary>
    void OnAnimEnter();
    /// <summary>�U���̃A�j���[�V�����I������1�񂾂��Ă΂��</summary>
    void OnAnimExit();
}
