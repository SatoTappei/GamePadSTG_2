using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̍s����State�p�^�[���Ŏ�������
/// �v���C���[�Ɍ������Đ^�������߂Â��ċߐڍU�����Ă���G
/// �̊e�X�e�[�g�̊��N���X
/// </summary>
public class ChaseStraightEnemyBase
{
    /// <summary>�L�����N�^�[�̏��</summary>
    public enum State
    {
        Idle, Wander, Chase, Attack
    };

    /// <summary>�X�e�[�g���̃C�x���g</summary>
    public enum Event
    {
        Enter, Stay, Exit
    };

    public State CurrentState;
    protected Event _event;
    protected GameObject _character;
    protected Transform _target;
    protected ChaseStraightEnemyBase _nextState;

    /// <summary>���E�̋���</summary>
    [SerializeField] float _sightRange = 10.0f;
    /// <summary>���E�̊p�x</summary>
    [SerializeField] float _sightAngle = 30.0f;
    /// <summary>�U�����Ă��鋗��</summary>
    [SerializeField] float _attackRange = 7.0f;

    public ChaseStraightEnemyBase(GameObject character, Transform target)
    {
        _character = character;
        _target = target;
    }

    /// <summary>State�ɐ��ڂ����ہA1�x�����Ă΂��</summary>
    public virtual void Enter() => _event = Event.Stay;
    /// <summary>Enter���Ă΂ꂽ��AExit�ɂȂ�܂Ŗ��t���[���Ă΂��</summary>
    public virtual void Update() => _event = Event.Stay;
    /// <summary>����State�ɐ��ڂ���ہA1�x�����Ă΂��</summary>
    public virtual void Exit() => _event = Event.Exit;

    /// <summary>
    /// ���݂̃C�x���g�ɑΉ��������\�b�h���Ăяo����
    /// ���t���[���ł̏�ԃN���X��Ԃ�
    /// </summary>
    public ChaseStraightEnemyBase Process()
    {
        if (_event == Event.Enter) Enter();
        else if (_event == Event.Stay) Update();
        else if (_event == Event.Exit)
        {
            Exit();
            return _nextState;
        }
        return this;
    }

    /// <summary>�^�[�Q�b�g�����E�ɓ����Ă��邩</summary>
    protected bool FindTarget()
    {
        // �^�[�Q�b�g�Ǝ��g�̋����x�N�g�������߂�
        Vector3 diff = _target.position - _character.transform.position;
        // �^�[�Q�b�g�Ƃ̊p�x���v�Z
        float angle = Vector3.Angle(diff, _character.transform.forward);
        // �^�[�Q�b�g�����E���ɂ��邩��Ԃ�
        bool inSight = diff.magnitude < _sightRange && angle < _sightAngle;
        return inSight ? true : false;
    }
}

/// <summary>
/// �A�C�h��:���̏�ŗ����~�܂��Ă����Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyIdle : ChaseStraightEnemyBase
{
    public ChaseStraightEnemyIdle(GameObject character, Transform target)
        : base(character, target)
    {
        CurrentState = State.Idle;
    }

    public override void Enter() => base.Enter();
    
    public override void Update()
    {
        // �^�[�Q�b�g�����E�ɓ����Ă���ΒǐՂ��n�߂�
        if (FindTarget())
        {
            _nextState = new ChaseStraightEnemyChase(_character, _target);
            _event = Event.Exit;
        }
        // �^�[�Q�b�g�����E�ɓ����Ă��Ȃ��ꍇ
        // �m���ł��낤�낵�n�߂�
        else if (Random.Range(0, 10) == 0)
        {
            _nextState = new ChaseStraightEnemyWander(_character, _target);
            _event = Event.Exit;
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// ���낤��:�����_����ɂ��̎��ӂ��s�����藈���肷���Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyWander : ChaseStraightEnemyBase
{
    public ChaseStraightEnemyWander(GameObject character, Transform target)
        : base(character, target)
    {
        CurrentState = State.Wander;
    }

    public override void Enter() => base.Enter();

    public override void Update()
    {
        if (FindTarget())
        {
            _nextState = new ChaseStraightEnemyChase(_character, _target);
            _event = Event.Exit;
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// �ǐ�:�^�[�Q�b�g�Ɍ������Đ^�������������Ă�����Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyChase : ChaseStraightEnemyBase
{
    public ChaseStraightEnemyChase(GameObject character, Transform target)
        : base(character, target)
    {
        CurrentState = State.Chase;
    }

    public override void Enter() => base.Enter();

    public override void Update() => base.Update();

    public override void Exit() => base.Exit();
}

/// <summary>
/// �U��:���̏�ɗ����~�܂��ă^�[�Q�b�g�ɍU���������Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyAttack : ChaseStraightEnemyBase
{
    public ChaseStraightEnemyAttack(GameObject character, Transform target)
        : base(character, target)
    {
        CurrentState = State.Attack;
    }

    public override void Enter() => base.Enter();

    public override void Update() => base.Update();

    public override void Exit() => base.Exit();
}