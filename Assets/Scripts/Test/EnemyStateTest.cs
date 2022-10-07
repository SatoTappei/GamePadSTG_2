
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State�p�^�[����p���������̃e�X�g
/// �G�̈ړ��𐧌䂷��
/// </summary>
public class EnemyStateTest
{
    /// <summary>�G�L��������肤����</summary>
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK
    };
    /// <summary>STATE���̃C�x���g</summary>
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    /// <summary>���݂̏��</summary>
    public STATE _name;
    /// <summary>���݂̃C�x���g</summary>
    protected EVENT _stage;
    /// <summary>�G�L�����̃Q�[���I�u�W�F�N�g</summary>
    protected GameObject _enemy;
    /// <summary>�v���C���[��Transform</summary>
    protected Transform _player;
    /// <summary>���̏��</summary>
    protected EnemyStateTest _nextState;
    /// <summary>�G�L������NavMeshAgent�R���|�[�l���g</summary>
    protected NavMeshAgent _agent;

    /// <summary>"Vis"ible"Dist"ance�v���C���[��F�����鋗��</summary>
    readonly float _visDist = 10.0f;
    /// <summary>"Vis"ible"Angle"�v���C���[��F������p�x</summary>
    readonly float _visAngle = 30.0f;
    /// <summary>�v���C���[���U�����鋗��</summary>
    readonly float _shootDist = 7.0f;

    /// <summary>
    /// ���̃X�N���v�g�̃N���X�̃R���X�g���N�^
    /// �����ɑS�Ă̏�ԂŎg�p�������ݒ肷��
    /// </summary>
    public EnemyStateTest(GameObject enemy, NavMeshAgent agent, Transform player)
    {
        _enemy = enemy;
        _agent = agent;
        _stage = EVENT.ENTER;
        _player = player;
    }

    /// <summary>�����ԂɂȂ�ƍŏ��Ɏ��s�����</summary>
    public virtual void Enter() => _stage = EVENT.UPDATE;
    /// <summary>�����Ԓ��A�X�e�[�g���ς��܂Ŏ��s�����</summary>
    public virtual void Update() => _stage = EVENT.UPDATE;
    /// <summary>�����Ԃ���X�e�[�g���ω�����Ƃ��Ɏ��s�����</summary>
    public virtual void Exit() => _stage = EVENT.EXIT;

    /// <summary>�O������Ăяo���āA�e�X�e�[�W�ŏ�Ԃ�i�s������</summary>
    public EnemyStateTest Process()
    {
        if (_stage == EVENT.ENTER) Enter();
        if (_stage == EVENT.UPDATE) Update();
        if (_stage == EVENT.EXIT)
        {
            // ���݂̏�Ԃ��甲����ۂɎ��̃X�e�[�g��Ԃ�
            Exit();
            return _nextState;
        }

        // ���݂̃X�e�[�g��Ԃ�
        return this;
    }

    /// <summary>�G�L�����̑O���Ƀv���C���[�����邩</summary>
    public bool CanSeePlayer()
    {
        // �G�L��������v���C���[�ւ̃x�N�g�����v�Z
        Vector3 dir = _player.position - _enemy.transform.position;
        // ���o���v�Z
        float angle = Vector3.Angle(dir, _enemy.transform.forward);
        // �v���C���[���߂��ɂ��āA��������͈͂ɂ���ꍇ
        if (dir.magnitude < _visDist && angle < _visAngle)
        {
            return true;
        }
        return false;
    }
}

/// <summary>
/// �A�C�h�����:���̏�ŗ����~�܂�
/// </summary>
public class Idle : EnemyStateTest
{
    public Idle(GameObject enemy, NavMeshAgent agent, Transform player) : base(enemy, agent, player)
    {
        _name = STATE.IDLE;
    }

    public override void Enter()
    {
        // �X�e�[�W��"UPDATE"�ɂ���
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            // �{�X�e�[�^�X�̃X�e�[�W��EXIT�ɂȂ�AnextStage�Ŏw�肵���X�e�[�^�X�ɂȂ�
            _nextState = new Pursue(_enemy, _agent, _player);
            _stage = EVENT.EXIT;
        }
        // 10%�̊m����IDLE��Ԃ���Patrol��Ԃɐ���
        else if (Random.Range(0, 100) < 10)
        {
            _nextState = new Patrol(_enemy, _agent, _player);
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

/// <summary>
/// �p�g���[�����:�e�E�F�C�|�C���g�����񂷂�
/// </summary>
public class Patrol : EnemyStateTest
{
    int currentIndex = -1;
    public Patrol(GameObject _enemy, NavMeshAgent _agent, Transform _player) : base(_enemy, _agent, _player)
    {
        _name = STATE.PATROL;
        _agent.speed = 2;         // ���񎞂̑��x
        _agent.isStopped = false; // �p�X�T���̊J�n/��~����(false = �J�n)
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity; // �G�L�����Ƃ̋���

        // �e�E�F�C�|�C���g�����[�v���āA�G�L�����Ɗe�E�F�C�|�C���g�Ԃ̋������v�Z���A
        // �ł��߂��E�F�C�|�C���g���Z�o����
        for (int i = 0; i < WayPointManagerTest.Singleton.Waypoints.Count; i++)
        {
            var thisWP = WayPointManagerTest.Singleton.Waypoints[i];
            var distance = Vector3.Distance(_enemy.transform.position, thisWP.transform.position);
            if (distance < lastDist)
            {
                // Update�ł�i��1�������Ă��玟�̖ړI�n��ݒ肷�邽�߂�1������
                currentIndex = i - 1;
                lastDist = distance;
            }
        }

        // �ړI�n�̐ݒ�
        var newWayPoint = WayPointManagerTest.Singleton.Waypoints[currentIndex];
        _agent.SetDestination(newWayPoint.transform.position);

        base.Enter();
    }

    public override void Update()
    {
        // �E�F�C�|�C���g�ɓ��B���Ă��邩�m�F
        if (_agent.remainingDistance < 1)
        {
            // ���̃E�F�C�|�C���g�Ɉړ�(���̂���]���g��Ȃ�)
            if (currentIndex >= WayPointManagerTest.Singleton.Waypoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;

            // �ړI�n�̐ݒ�
            var newWayPoint = WayPointManagerTest.Singleton.Waypoints[currentIndex];
            _agent.SetDestination(newWayPoint.transform.position);
        }

        // �v���C���[����������Pursue(�ǐ�)��Ԃɐ���
        if (CanSeePlayer())
        {
            _nextState = new Pursue(_enemy, _agent, _player);
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

/// <summary>
/// �ǐՏ��:�v���C���[�����F���A�ǐՂ���
/// </summary>
public class Pursue : EnemyStateTest
{
    public Pursue(GameObject enemy, NavMeshAgent agent, Transform player) : base(enemy, agent, player)
    {
        _name = STATE.PURSUE; // ���݂̏�Ԃ�ݒ�
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();
    }
}

/// <summary>
/// �U�����:???
/// </summary>
public class Attack : EnemyStateTest
{
    // �G�L�������v���C���[�Ɍ������ĉ�]���鑬�x��ݒ�
    float rotationSpeed = 2.0f;
    public Attack(GameObject enemy, NavMeshAgent agent, Transform player) : base(enemy, agent, player)
    {
        _name = STATE.ATTACK; // ���݂̏�Ԃ�ݒ�
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();
    }
}