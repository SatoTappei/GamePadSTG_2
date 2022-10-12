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
public class BatteryEnemyAI : MonoBehaviour
{
    [SerializeField] Transform _muzzle;
    [SerializeField] Transform _turret;

    /// <summary>���݂̃X�e�[�g�ɑΉ������N���X</summary>
    BatteryEnemyBase _currentStateClass;

    void Start()
    {
        // �^�[�Q�b�g�͌���Player�̂݁A�^�O��ς��邱�ƂŃ^�[�Q�b�g��ς��邱�Ƃ��o����
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Animator anim = GetComponentInChildren<Animator>();
        _currentStateClass = new BatteryEnemyIdle(gameObject, target, anim);
    }

    void Update()
    {
        _currentStateClass = _currentStateClass.Process();
    }
}
