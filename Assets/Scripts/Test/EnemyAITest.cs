using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State�p�^�[����p���������̃e�X�g
/// �󋵂𔻒f���A���̏�ɉ������X�e�[�g�����s����
/// </summary>
public class EnemyAITest : MonoBehaviour
{
    // EnemyStateTest���K�v�Ƃ��Ă���ϐ�
    NavMeshAgent _agent;        // �G�L������NavMeshAgent�R���|�[�l���g
    public Transform _player;   // �v���C���[��Transform�R���|�[�l���g

    EnemyStateTest _currentState;      // ���݂̏��

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        // �ŏ��̏��
        _currentState = new Idle(gameObject, _agent, _player);
    }

    void Update()
    {
        // ���݂̏�Ԃ����s����A�߂�l�͎��̏��
        _currentState = _currentState.Process();
    }
}
