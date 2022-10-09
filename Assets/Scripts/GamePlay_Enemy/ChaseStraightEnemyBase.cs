using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

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

    // Character��Y���W�����߂邽�߂�Ray
    readonly float RayRadius = 0.01f;
    readonly float RayDistance = 10.0f;
    readonly Vector3 RayOffset = new Vector3(0.0f, 2.5f, 0.0f);
    readonly LayerMask RayMask = 1 << 6;

    /// <summary>���E�̋���</summary>
    readonly float SightRange = 10.0f;
    /// <summary>���E�̊p�x</summary>
    readonly float SightAngle = 30.0f;
    /// <summary>�U�����Ă��鋗��</summary>
    readonly float AttackRange = 7.0f;

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
        bool inSight = diff.magnitude < SightRange && angle < SightAngle;
        return inSight ? true : false;
    }

    /// <summary>���C��^���ɔ�΂��ĉ��ɒn�ʂ����邩���ׂ�</summary>
    protected bool GetCharacterPosY(out float y)
    {
        Vector3 rayPos = _character.transform.position + RayOffset;
        Ray ray = new Ray(rayPos, Vector3.down);
        if (Physics.SphereCast(ray, RayRadius, out RaycastHit hit, RayDistance, RayMask))
        {
            y = hit.point.y;
            return true;
        }
        else
        {
            y = _character.transform.position.y;
            return false;
        }
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
        Vector3 pos = _character.transform.position;
        GetCharacterPosY(out float y);
        pos.y = y;
        _character.transform.position = pos;

        // �^�[�Q�b�g�����E�ɓ����Ă���ΒǐՂ��n�߂�
        if (FindTarget())
        {
            _nextState = new ChaseStraightEnemyChase(_character, _target);
            _event = Event.Exit;
        }
        // �^�[�Q�b�g�����E�ɓ����Ă��Ȃ��ꍇ
        // 3���̊m���ł��낤�낵�n�߂�
        else if (Random.Range(0, 100) == 3)
        {
            _nextState = new ChaseStraightEnemyWander(_character, _target);
            _event = Event.Exit;
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// ���낤��:�����_����ɂ��̎��ӂɈړ������Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyWander : ChaseStraightEnemyBase
{
    /// <summary>XZ���ʂł̈ړ���������AY�̓��C�Ŕ��肷��</summary>
    Vector3 _dir;

    readonly int Speed = 3;

    public ChaseStraightEnemyWander(GameObject character, Transform target)
        : base(character, target)
    {
        CurrentState = State.Wander;
    }

    public override void Enter()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);
        _dir = new Vector3(x, 0, z).normalized;

        _event = Event.Stay;
    }

    public override void Update()
    {
        // 1���̊m���Œ�~���A�A�C�h����Ԃɖ߂�
        if (Random.Range(0, 100) <= 1)
        {
            _nextState = new ChaseStraightEnemyIdle(_character, _target);
            _event = Event.Exit;
            return;
        }

        // �^���ɏ������邩���ׂ�
        bool checkFloor = GetCharacterPosY(out float y);
        if (checkFloor)
        {
            _character.transform.position += _dir * Time.deltaTime * Speed;
        }
        else
        {
            // �^�������ł͂Ȃ��ꍇ�͋t������1�t���[���������i�܂��A�A�C�h����Ԃɖ߂�
            _character.transform.position += -1 * _dir * Time.deltaTime * Speed;
            _nextState = new ChaseStraightEnemyIdle(_character, _target);
            _event = Event.Exit;
            return;
        }

        // �ړ����Y�����ς���
        Vector3 pos = _character.transform.position;
        pos.y = y;
        _character.transform.position = pos;

        // �^�[�Q�b�g���������ꍇ�͒ǐՏ�Ԃɂ���
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

    public override void Update()
    {
        // �y�����邽�߂�Ray�ƍ��W���������œ�����
        Vector3 diff = _target.position - _character.transform.position;
        Vector3 moveVec = new Vector3(diff.x, 0, diff.z);
        _character.transform.position += moveVec.normalized * Time.deltaTime * 3;

        Vector3 pos = _character.transform.position;
        GetCharacterPosY(out float y);
        pos.y = y;
        _character.transform.position = pos;
    }

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