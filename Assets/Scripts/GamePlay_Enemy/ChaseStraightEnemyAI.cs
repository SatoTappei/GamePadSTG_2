using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̃X�N���v�g��"Enemy_�G��"�̃I�u�W�F�N�g�ɃA�^�b�`���邾���œ����悤�ɂ���
/// <summary>
/// �G�̍s����State�p�^�[���Ŏ������Ă���
/// �^�[�Q�b�g�Ɍ������Đ^�������߂Â��ċߐڍU�����Ă���G
/// ���݂̏�Ԃɂ�����s�������s����
/// </summary>
public class ChaseStraightEnemyAI : MonoBehaviour
{
    /// <summary>���݂̃X�e�[�g�ɑΉ������N���X</summary>
    ChaseStraightEnemyBase _currentStateClass;

    void Start()
    {
        // �^�[�Q�b�g�͌���Player�̂݁A�^�O��ς��邱�ƂŃ^�[�Q�b�g��ς��邱�Ƃ��o����
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        _currentStateClass = new ChaseStraightEnemyIdle(gameObject, target);
    }

    void Update()
    {
        _currentStateClass = _currentStateClass.Process();
    }
}
