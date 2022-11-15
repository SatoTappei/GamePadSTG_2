using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̃X�N���v�g��"Enemy_�G��"�̃I�u�W�F�N�g�ɃA�^�b�`����
// BatteryEnemyBase���̊e�A�j���[�V�������萔������������Γ���
/// <summary>
/// �G�̍s����State�p�^�[���Ŏ������Ă���
/// ���̏ꂩ�瓮�����������挂���Ă���G
/// ���݂̏�Ԃɂ�����s�������s����
/// </summary>
public class BatteryEnemyAI : EnemyAIBase
{
    // ��]�C���A�q�I�u�W�F�N�g��Muzzle�Ƃ������O�̃I�u�W�F�N�g��������
    // BatteryEnemyIdle�̃R���X�g���N�^���Ă񂾎��_�ŃG���[�ɂȂ�̂Œ���
    [SerializeField] Transform _turret;

    /// <summary>���݂̃X�e�[�g�ɑΉ������N���X</summary>
    BatteryEnemyBase _currentStateClass;

    public override void Init()
    {
        // �^�[�Q�b�g�͌���Player�̂݁A�^�O��ς��邱�ƂŃ^�[�Q�b�g��ς��邱�Ƃ��o����
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Animator anim = GetComponentInChildren<Animator>();
        _currentStateClass = new BatteryEnemyIdle(gameObject, target, anim, _turret);
    }

    public override void Stay()
    {
        _currentStateClass = _currentStateClass.Process();
    }

    public override void Exit()
    {
        if (_currentStateClass != null)
        {
            _currentStateClass.ChangeCompleted();
            _currentStateClass.Process();
        }
    }

    // TODO:��Ԃ̎��S���̃v���t�@�u����ɐ��ʂ������Ă��܂��̂Ŏ��S���Ɍ����Ă��������ɐ��������悤����
    // TODO:�G�̃��O�h�[�����v���C���[�Ƃ̓����蔻��������Ȃ��ɒ����B���C���[�Őݒ肷��B
}
