using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̍s����State�p�^�[���Ŏ�������
/// ���̏ꂩ�瓮�����������挂���Ă���G
/// �̊e�X�e�[�g�̊��N���X
/// </summary>
public class BatteryEnemyBase
{
    /// <summary>�L�����N�^�[�̏��</summary>
    public enum State
    {
        Idle, Search, Capture
    };

    /// <summary>�X�e�[�g���̃C�x���g</summary>
    public enum Event
    {
        Enter, Stay, Exit,
    };

    public State CurrentState;
    protected Event _event;
    protected GameObject _character;
    protected Transform _target;
    protected BatteryEnemyBase _nextState;
    protected Animator _anim;

    protected Transform _turret;
    protected Transform _muzzle;

    // �e�A�j���[�V������
    protected readonly string IdleAnim = "Idle";

    // Character��Y���W�����߂邽�߂�Ray
    readonly float RayRadius = 0.01f;
    readonly float RayDistance = 10.0f;
    readonly Vector3 RayOffset = new Vector3(0.0f, 2.5f, 0.0f);
    readonly LayerMask RayMask = 1 << 6;

    readonly float SightRange = 10.0f;
    readonly float SightAngle = 60.0f;
    readonly float AttackRange = 1.2f;

    public BatteryEnemyBase(GameObject character, Transform target, Animator anim, Transform turret)
    {
        _character = character;
        _target = target;
        _anim = anim;
        _turret = turret;
        // �^���b�g�̎q�I�u�W�F�N�g��Muzzle�Ƃ������O�̃I�u�W�F�N�g���K�v
        _muzzle = _turret.Find("Muzzle");
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
    public BatteryEnemyBase Process()
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
    protected void ChangeState(BatteryEnemyBase next)
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
        bool inSight = diff.magnitude <= SightRange && angle <= SightAngle;
        return inSight;
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
/// �A�C�h��:�����𓮂������A�e�������Ȃ���Ԃ̃N���X
/// </summary>
public class BatteryEnemyIdle : BatteryEnemyBase
{
    public BatteryEnemyIdle(GameObject character, Transform target, Animator anim, Transform turret)
        : base(character, target, anim, turret)
    {
        CurrentState = State.Idle;
    }

    public override void Enter() => base.Enter();

    public override void Update()
    {
        Debug.Log("�A�C�h����Ԃł�");

        CheckFloor(out float y);
        SetCharacterPosY(y);
        ChangeState(new BatteryEnemySearch(_character, _target, _anim, _turret));
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// �T��:�����𓮂����ēG��T�������Ԃ̃N���X
/// </summary>
public class BatteryEnemySearch : BatteryEnemyBase
{
    public BatteryEnemySearch(GameObject character, Transform target, Animator anim, Transform turret)
        : base(character, target, anim, turret)
    {
        CurrentState = State.Search;
    }

    public override void Enter() => base.Enter();

    public override void Update()
    {
        // -90~90���s�����藈���肷��
        float angle = Mathf.Sin(Time.time);
        Debug.Log(angle);

        _turret.transform.eulerAngles = new Vector3(0, angle * 90, 0);
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// �ߑ�:�G��_���čU�������Ԃ̃N���X
/// </summary>
public class BatteryEnemyCapture : BatteryEnemyBase
{
    public BatteryEnemyCapture(GameObject character, Transform target, Animator anim, Transform turret)
        : base(character, target, anim, turret)
    {
        CurrentState = State.Capture;
    }

    public override void Enter() => base.Enter();

    public override void Update() => base.Update();

    public override void Exit() => base.Exit();
}