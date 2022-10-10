using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
    protected Animator _anim;

    // �e�A�j���[�V������
    protected readonly string WalkAnim = "Run";
    protected readonly string AttackAnim = "Slash";
    protected readonly string IdleAnim = "Idle";

    // Character��Y���W�����߂邽�߂�Ray
    readonly float RayRadius = 0.01f;
    readonly float RayDistance = 10.0f;
    readonly Vector3 RayOffset = new Vector3(0.0f, 2.5f, 0.0f);
    readonly LayerMask RayMask = 1 << 6;

    /// <summary>���E�̋���</summary>
    readonly float SightRange = 10.0f;
    /// <summary>���E�̊p�x</summary>
    readonly float SightAngle = 100.0f;
    /// <summary>�U�����Ă��鋗��</summary>
    readonly float AttackRange = 1.2f;

    public ChaseStraightEnemyBase(GameObject character, Transform target, Animator anim)
    {
        _character = character;
        _target = target;
        _anim = anim;
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

    /// <summary>�X�e�[�g��ς���</summary>
    protected void ChangeState(ChaseStraightEnemyBase next)
    {
        _nextState = next;
        _event = Event.Exit;
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

    /// <summary>�ΏۂƂ̋������U���\�����ׂ�</summary>
    protected bool CheckCanAttack()
    {
        Vector3 diff = _target.position - _character.transform.position;
        return diff.magnitude <= AttackRange;
    }

    /// <summary>���C��^���ɔ�΂��ĉ��ɒn�ʂ����邩���ׂ�</summary>
    protected bool CheckFloor(out float y)
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

    /// <summary>Y���W���Z�b�g����</summary>
    protected void SetCharacterPosY(float y)
    {
        Vector3 pos = _character.transform.position;
        pos.y = y;
        _character.transform.position = pos;
    }
}

/// <summary>
/// �A�C�h��:���̏�ŗ����~�܂��Ă����Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyIdle : ChaseStraightEnemyBase
{
    public ChaseStraightEnemyIdle(GameObject character, Transform target, Animator anim)
        : base(character, target, anim)
    {
        CurrentState = State.Idle;
    }

    public override void Enter()
    {
        _anim.Play(IdleAnim);

        _event = Event.Stay;
    }
    
    public override void Update()
    {
        CheckFloor(out float y);
        SetCharacterPosY(y);

        // �^�[�Q�b�g�����E�ɓ����Ă���ΒǐՂ��n�߂�
        if (FindTarget())
        {
            ChangeState(new ChaseStraightEnemyChase(_character, _target, _anim));
        }
        // �^�[�Q�b�g�����E�ɓ����Ă��Ȃ��ꍇ
        // 3���̊m���ł��낤�낵�n�߂�
        else if (Random.Range(0, 100) == 3)
        {
            ChangeState(new ChaseStraightEnemyWander(_character, _target, _anim));
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// ���낤��:�����_����ɂ��̎��ӂɈړ������Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyWander : ChaseStraightEnemyBase
{

    readonly float Speed = 1.5f;
    Vector3 _dir;
    Vector3 _prevPos;

    public ChaseStraightEnemyWander(GameObject character, Transform target, Animator anim)
        : base(character, target, anim)
    {
        CurrentState = State.Wander;
    }

    public override void Enter()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);
        _dir = new Vector3(x, 0, z).normalized;
        _anim.Play(WalkAnim);
        _prevPos = _character.transform.position;

        _event = Event.Stay;
    }

    public override void Update()
    {
        // 1���̊m���Œ�~���A�A�C�h����Ԃɖ߂�
        if (Random.Range(0, 100) <= 1)
        {
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim));
            return;
        }

        _character.transform.position += _dir * Time.deltaTime * Speed;
        _character.transform.rotation = Quaternion.LookRotation(_dir);

        // �^���ɏ������邩���ׂ�
        if (CheckFloor(out float y))
        {
            _prevPos = _character.transform.position;
        }
        else
        {
            // �^�������ł͂Ȃ��ꍇ�͑O�t���[���̈ʒu�ɖ߂�
            _character.transform.position = _prevPos;
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim));
            return;
        }

        // �ړ����Y�����ς���
        SetCharacterPosY(y);

        // �^�[�Q�b�g���������ꍇ�͒ǐՏ�Ԃɂ���
        if (FindTarget())
        {
            ChangeState(new ChaseStraightEnemyChase(_character, _target, _anim));
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// �ǐ�:�^�[�Q�b�g�Ɍ������Đ^�������������Ă�����Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyChase : ChaseStraightEnemyBase
{
    readonly float Speed = 3.0f;
    Vector3 _prevPos;

    public ChaseStraightEnemyChase(GameObject character, Transform target, Animator anim)
        : base(character, target, anim)
    {
        CurrentState = State.Chase;
    }

    public override void Enter()
    {
        _anim.Play(WalkAnim);
        _prevPos = _character.transform.position;

        _event = Event.Stay;
    }

    public override void Update()
    {
        Vector3 diff = _target.position - _character.transform.position;
        Vector3 dir = new Vector3(diff.x, 0, diff.z);

        _character.transform.position += dir.normalized * Time.deltaTime * Speed;
        _character.transform.rotation = Quaternion.LookRotation(dir);

        if (CheckFloor(out float y))
        {
            _prevPos = _character.transform.position;
        }
        else
        {
            _character.transform.position = _prevPos;
        }

        SetCharacterPosY(y);

        // �^�[�Q�b�g�Ƃ̋������U���\�ȋ����Ȃ�U����Ԃɂ���
        if (CheckCanAttack())
        {
            ChangeState(new ChaseStraightEnemyAttack(_character, _target, _anim));
        }
        // �^�[�Q�b�g������������A�C�h����Ԃɖ߂�
        else if (!FindTarget())
        {
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim));
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// �U��:���̏�ɗ����~�܂��ă^�[�Q�b�g�ɍU���������Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyAttack : ChaseStraightEnemyBase
{
    System.IDisposable _disposable;

    public ChaseStraightEnemyAttack(GameObject character, Transform target, Animator anim)
        : base(character, target, anim)
    {
        CurrentState = State.Attack;
    }

    public override void Enter()
    {
        // �U����ԂɂȂ������Ɉ�x�U�����ĈȌ�2�b��1��U������
        _anim.Play(AttackAnim);
        _disposable = Observable.Interval(System.TimeSpan.FromSeconds(2.0f)).Subscribe(_ =>
        {
            _anim.Play(AttackAnim);
        });

        _event = Event.Stay;
    }

    public override void Update()
    {
        // �U���͈͊O�ɏo����A�C�h����Ԃɖ߂�
        if (!CheckCanAttack() && !_anim.GetCurrentAnimatorStateInfo(0).IsName(AttackAnim))
        {
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim));
        }

        CheckFloor(out float y);
        SetCharacterPosY(y);
    }

    public override void Exit()
    {
        _disposable.Dispose();

        _event = Event.Exit;
    }
}