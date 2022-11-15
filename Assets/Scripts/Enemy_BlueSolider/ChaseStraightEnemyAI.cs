using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̃X�N���v�g��"Enemy_�G��"�̃I�u�W�F�N�g�ɃA�^�b�`����
// ChaseStraightEnemyBase���̊e�A�j���[�V�������萔������������Γ���
/// <summary>
/// �G�̍s����State�p�^�[���Ŏ������Ă���
/// �^�[�Q�b�g�Ɍ������Đ^�������߂Â��ċߐڍU�����Ă���G
/// ���݂̏�Ԃɂ�����s�������s����
/// </summary>
public class ChaseStraightEnemyAI : EnemyAIBase
{
    public override void Init()
    {
        // �^�[�Q�b�g�͌���Player�̂݁A�^�O��ς��邱�ƂŃ^�[�Q�b�g��ς��邱�Ƃ��o����
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Animator anim = GetComponentInChildren<Animator>();
        GameObject findIcon = transform.Find("FindIcon").gameObject;
        _currentStateClass = new ChaseStraightEnemyIdle(gameObject, target, anim, findIcon);
    }
}
