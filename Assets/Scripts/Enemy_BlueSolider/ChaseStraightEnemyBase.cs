using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

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
        Idle, Wander, Chase, Attack, Completed
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
    protected GameObject _findIcon;

    // �e�A�j���[�V������
    protected readonly string WalkAnim = "Run";
    protected readonly string AttackAnim = "Slash";
    protected readonly string IdleAnim = "Idle";

    // Character��Y���W�����߂邽�߂�Ray
    protected readonly LayerMask Mask = 1 << 6;
    readonly float RayRadius = 0.01f;
    readonly float RayDistance = 10.0f;
    readonly Vector3 RayOffset = new Vector3(0.0f, 2.5f, 0.0f);

    /// <summary>���E�̋���</summary>
    readonly float SightRange = 10.0f;
    /// <summary>���E�̊p�x</summary>
    readonly float SightAngle = 100.0f;
    /// <summary>�U�����Ă��鋗��</summary>
    readonly float AttackRange = 1.0f;

    public ChaseStraightEnemyBase(GameObject character, Transform target, Animator anim, GameObject findIcon)
    {
        _character = character;
        _target = target;
        _anim = anim;
        _findIcon = findIcon;
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

    /// <summary>��������S�Ɏ~�߂�</summary>
    public void ChangeCompleted()
    {
        _nextState = new ChaseStraightEnemyCompleted(_character, _target, _anim, _findIcon);
        _event = Event.Exit;
    }

    /// <summary>�^�[�Q�b�g�����A�C�R���̕\����؂�ւ���</summary>
    protected void ActiveFindIcon(bool value)
    {
        if (_findIcon != null)
            _findIcon.SetActive(value);
        // TODO: ����炷
    }

    /// <summary>�^�[�Q�b�g�����E�ɓ����Ă��邩</summary>
    protected bool FindTarget()
    {
        // �^�[�Q�b�g�Ǝ��g�̋����x�N�g�������߂�
        Vector3 diff = _target.position - _character.transform.position;
        // �^�[�Q�b�g�Ƃ̊p�x���v�Z
        float angle = Vector3.Angle(diff, _character.transform.forward);
        // �^�[�Q�b�g�����E���ɂ��邩��Ԃ�
        bool inSight = diff.magnitude <= SightRange && angle <= SightAngle;
        return inSight;
    }

    /// <summary>�ΏۂƂ̋������U���\�����ׂ�</summary>
    protected bool CheckCanAttack()
    {
        Vector3 diff = _target.position - _character.transform.position;
        return diff.magnitude <= AttackRange;
    }

    /// <summary>Y���W���Z�b�g����</summary>
    protected void SetCharacterPosY()
    {
        Vector3 rayPos = _character.transform.position + RayOffset;
        Ray ray = new Ray(rayPos, Vector3.down);
        if (Physics.SphereCast(ray, RayRadius, out RaycastHit hit, RayDistance, Mask))
        {
            Vector3 pos = _character.transform.position;
            pos.y = hit.point.y;
            _character.transform.position = pos;
        }
        else
        {
            // �������Ȃ�
        }
    }
}

/// <summary>
/// �A�C�h��:���̏�ŗ����~�܂��Ă����Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyIdle : ChaseStraightEnemyBase
{
    public ChaseStraightEnemyIdle(GameObject character, Transform target, Animator anim, GameObject findIcon)
        : base(character, target, anim, findIcon)
    {
        CurrentState = State.Idle;
    }

    public override void Enter()
    {
        _anim.Play(IdleAnim);
        ActiveFindIcon(false);
        _event = Event.Stay;
    }
    
    public override void Update()
    {
        SetCharacterPosY();

        // �^�[�Q�b�g�����E�ɓ����Ă���ΒǐՂ��n�߂�
        if (FindTarget())
        {
            ChangeState(new ChaseStraightEnemyChase(_character, _target, _anim, _findIcon));
        }
        // �^�[�Q�b�g�����E�ɓ����Ă��Ȃ��ꍇ
        // 3���̊m���ł��낤�낵�n�߂�
        else if (UnityEngine.Random.Range(0, 100) == 3)
        {
            ChangeState(new ChaseStraightEnemyWander(_character, _target, _anim, _findIcon));
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
    readonly float RayDist = 1.5f;
    Vector3 _dir;
    // ���C�̔��ˊԊu�A�ǂɖ��܂�ꍇ�͂�����������
    float _raydist = 0.2f;
    float _rayCount = 0;

    public ChaseStraightEnemyWander(GameObject character, Transform target, Animator anim, GameObject findIcon)
        : base(character, target, anim, findIcon)
    {
        CurrentState = State.Wander;
    }

    public override void Enter()
    {
        float x = UnityEngine.Random.Range(-1.0f, 1.0f);
        float z = UnityEngine.Random.Range(-1.0f, 1.0f);
        _dir = new Vector3(x, 0, z).normalized;
        _anim.Play(WalkAnim);
        _event = Event.Stay;
    }

    public override void Update()
    {
        _rayCount += Time.deltaTime;

        // 1���̊m���Œ�~���A�A�C�h����Ԃɖ߂�
        if (UnityEngine.Random.Range(0, 100) <= 1)
        {
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim, _findIcon));
            return;
        }
        // ���Ԋu�őO������Ray���΂��ĕǂɂ߂荞�܂Ȃ��悤�ɂ���
        // �m�b�N�o�b�N�ł͕ǂɂ߂荞�܂Ȃ��Ȃ����̂ŕ����ŕǂɂ߂荞�܂Ȃ���Εǂɂ߂荞�ނ��Ƃ͂Ȃ�
        else if (_rayCount > _raydist && 
                 Physics.Raycast(_character.transform.position, _character.transform.forward, RayDist, Mask))
        {
            _rayCount = 0;
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim, _findIcon));
            return;
        }
        Debug.DrawRay(_character.transform.position, _character.transform.forward * RayDist, Color.red, 0);

        // ���W���Z�b�g
        _character.transform.position += _dir * Time.deltaTime * Speed;
        _character.transform.rotation = Quaternion.LookRotation(_dir);
        SetCharacterPosY();

        // �^�[�Q�b�g���������ꍇ�͒ǐՏ�Ԃɂ���
        if (FindTarget())
            ChangeState(new ChaseStraightEnemyChase(_character, _target, _anim, _findIcon));
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// �ǐ�:�^�[�Q�b�g�Ɍ������Đ^�������������Ă�����Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyChase : ChaseStraightEnemyBase
{
    readonly float Speed = 3.0f;
    readonly float RayDist = 1.5f;

    public ChaseStraightEnemyChase(GameObject character, Transform target, Animator anim, GameObject findIcon)
        : base(character, target, anim, findIcon)
    {
        CurrentState = State.Chase;
    }

    public override void Enter()
    {
        _anim.Play(WalkAnim);
        ActiveFindIcon(true);
        _event = Event.Stay;
    }

    public override void Update()
    {
        // �^�[�Q�b�g�Ƃ̍�����i�s���������߁A��]������
        Vector3 diff = _target.position - _character.transform.position;
        Vector3 dir = new Vector3(diff.x, 0, diff.z);
        _character.transform.rotation = Quaternion.LookRotation(dir);
        SetCharacterPosY();

        // �O������Ray���΂��ĕǂɂ߂荞�܂Ȃ��悤�ɂ���
        // �m�b�N�o�b�N�ł͕ǂɂ߂荞�܂Ȃ��Ȃ����̂ŕ����ŕǂɂ߂荞�܂Ȃ���Εǂɂ߂荞�ނ��Ƃ͂Ȃ�
        if (!Physics.Raycast(_character.transform.position, _character.transform.forward, RayDist, Mask))
        {
            _character.transform.position += dir.normalized * Time.deltaTime * Speed;
        }
        Debug.DrawRay(_character.transform.position, _character.transform.forward * RayDist, Color.red, 0);

        // �^�[�Q�b�g�Ƃ̋������U���\�ȋ����Ȃ�U����Ԃɂ���
        if (CheckCanAttack())
            ChangeState(new ChaseStraightEnemyAttack(_character, _target, _anim, _findIcon));
        // �^�[�Q�b�g������������A�C�h����Ԃɖ߂�
        else if (!FindTarget())
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim, _findIcon));
    }

    public override void Exit()
    {
        ActiveFindIcon(false);
        _event = Event.Exit;
    }
}

/// <summary>
/// �U��:���̏�ɗ����~�܂��ă^�[�Q�b�g�ɍU���������Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyAttack : ChaseStraightEnemyBase, IDisposable
{
    IDisposable _disposable;

    public ChaseStraightEnemyAttack(GameObject character, Transform target, Animator anim, GameObject findIcon = null)
        : base(character, target, anim, findIcon)
    {
        CurrentState = State.Attack;
    }

    public override void Enter()
    {
        // �U����ԂɂȂ������Ɉ�x�U�����ĈȌ�2�b��1��U������
        _anim.Play(AttackAnim);
        _disposable = Observable.Interval(TimeSpan.FromSeconds(2.0f)).Subscribe(_ =>
        {
            if(_anim != null)
                _anim.Play(AttackAnim);
        });
        ActiveFindIcon(true);
        _event = Event.Stay;
    }

    public override void Update()
    {
        // �U���͈͊O�ɏo����A�C�h����Ԃɖ߂�
        if (!CheckCanAttack() && !_anim.GetCurrentAnimatorStateInfo(0).IsName(AttackAnim))
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim, _findIcon));

        SetCharacterPosY();
    }

    public override void Exit()
    {
        ActiveFindIcon(false);
        _disposable.Dispose();
        _event = Event.Exit;
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}

/// <summary>
/// ���S�ɒ�~:����ȏ㓮�����Ȃ���Ԃ̃N���X
/// </summary>
public class ChaseStraightEnemyCompleted : ChaseStraightEnemyBase
{
    public ChaseStraightEnemyCompleted(GameObject character, Transform target, Animator anim, GameObject findIcon)
        : base(character, target, anim, findIcon)
    {
        CurrentState = State.Completed;
    }

    /// <summary>State�ɐ��ڂ����ہA1�x�����Ă΂��</summary>
    public override void Enter() => base.Enter();
    /// <summary>Enter���Ă΂ꂽ��AExit�ɂȂ�܂Ŗ��t���[���Ă΂��</summary>
    public override void Update() => base.Update();
    /// <summary>����State�ɐ��ڂ���ہA1�x�����Ă΂��</summary>
    public override void Exit() => base.Exit();
}