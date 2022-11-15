using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �G�̍s����State�p�^�[���Ŏ�������
/// ���̏ꂩ�瓮�����������挂���Ă���G
/// �̊e�X�e�[�g�̊��N���X
/// </summary>
public class BatteryEnemyBase :StateMachineBase
{
    /// <summary>�L�����N�^�[�̏��</summary>
    public enum State
    {
        Idle, Search, Capture, Completed
    };

    public State CurrentState;

    protected Transform _turret;
    protected Transform _muzzle;

    // �e�A�j���[�V������
    protected readonly string IdleAnim = "Idle";

    protected override float SightRange => 10.0f;
    protected override float SightAngle => 60.0f;
    protected override float AttackRange => 1.2f;

    public BatteryEnemyBase(GameObject character, Transform target, Animator anim, Transform turret, GameObject findIcon)
    {
        _character = character;
        _target = target;
        _anim = anim;
        _turret = turret;
        _findIcon = findIcon;
        // �^���b�g�̎q�I�u�W�F�N�g��Muzzle�Ƃ������O�̃I�u�W�F�N�g���K�v
        if(_turret != null)
            _muzzle = _turret.Find("Muzzle");
    }

    protected override bool FindTarget()
    {
        // �C���̑O��������Ɋp�x���Ƀ^�[�Q�b�g�����邩���ׂ�
        Vector3 diff = _target.position - _character.transform.position;
        float angle = Vector3.Angle(diff, _turret.transform.forward);
        bool isSight = diff.magnitude <= SightRange && angle <= SightAngle;
        return isSight;
    }

    public override void ToCompleted()
    {
        _nextState = new BatteryEnemyCompleted(_character, _target, _anim, _turret, _findIcon);
        _event = Event.Exit;
    }
}

/// <summary>
/// �A�C�h��:�C���𓮂������A�e�������Ȃ���Ԃ̃N���X
/// </summary>
public class BatteryEnemyIdle : BatteryEnemyBase
{
    public BatteryEnemyIdle(GameObject character, Transform target, Animator anim, Transform turret, GameObject findIcon)
        : base(character, target, anim, turret, findIcon)
    {
        CurrentState = State.Idle;
    }

    public override void Enter()
    {
        ActiveFindIcon(false);
        _event = Event.Stay;
    }

    public override void Update()
    {
        SetCharacterPosY();
        ChangeState(new BatteryEnemySearch(_character, _target, _anim, _turret, _findIcon));
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// �T��:�����𓮂����ēG��T�������Ԃ̃N���X
/// </summary>
public class BatteryEnemySearch : BatteryEnemyBase
{
    /// <summary>�C�������ʂ���������</summary>
    bool _lookedFront;
    /// <summary>�C������]�����邽�߂Ɏg���O�p�֐��p�̃J�E���g</summary>
    float _count;

    public BatteryEnemySearch(GameObject character, Transform target, Animator anim, Transform turret, GameObject findIcon)
        : base(character, target, anim, turret, findIcon)
    {
        CurrentState = State.Search;
    }

    public override void Enter() => base.Enter();

    public override void Update()
    {
        SetCharacterPosY();

        // �T����ԂɂȂ�����܂��C���𐳖ʂ���������
        if (!_lookedFront)
        {
            Quaternion look = Quaternion.LookRotation(_character.transform.forward);
            _turret.rotation = Quaternion.Lerp(_turret.rotation, look, 0.05f);
            // �C���̐��ʂƐ�Ԃ̐��ʂ̊p�x���قړ����ɂȂ����琳�ʂ��������Ƃ݂Ȃ�
            float angle = Vector3.Angle(_turret.forward, _character.transform.forward);
            if (angle < 0.01f)
                _lookedFront = true;
        }
        // ���ʂ���������͂��邭���]������
        else
        {
            // ���ʂɌ��������Ԃ̂Ƃ��̓J�E���g�𑫂��Ȃ��悤�ɂ���
            _count += Time.deltaTime;
            float sin = Mathf.Sin(_count);
            float cos = Mathf.Cos(_count);

            Vector3 dir = new Vector3(sin, 0, -1 * cos);
            Quaternion look = Quaternion.LookRotation(dir);
            _turret.rotation = Quaternion.Lerp(_turret.rotation, look, 0.05f);
        }

        if (FindTarget())
        {
            ChangeState(new BatteryEnemyCapture(_character, _target, _anim, _turret, _findIcon));
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// �ߑ�:�G��_���čU�������Ԃ̃N���X
/// </summary>
public class BatteryEnemyCapture : BatteryEnemyBase, System.IDisposable
{
    ObjectPool _pool;
    System.IDisposable _disposable;

    public BatteryEnemyCapture(GameObject character, Transform target, Animator anim, Transform turret, GameObject findIcon)
        : base(character, target, anim, turret, findIcon)
    {
        CurrentState = State.Capture;
    }

    public override void Enter()
    {
        // TODO:�U�������ԂɂȂ����疈��GetComponent���Ă���̂�
        //      ���ɗǂ����@������������ς���
        _pool = _character.GetComponent<ObjectPool>();
        _disposable = Observable.Interval(System.TimeSpan.FromSeconds(2.0f)).Subscribe(_ =>
        {
            if(_pool != null)
            {
                // �U������
                GameObject go = _pool.GetPooledObject();
                go.SetActive(true);
                go.transform.position = _muzzle.position;
                go.transform.forward = _muzzle.forward;
            }
        });

        ActiveFindIcon(true);
        _event = Event.Stay;
    }

    public override void Update()
    {
        SetCharacterPosY();

        // �^�[�Q�b�g�̕������v�Z���āAY�������̉�]��h�����߂�Y��0�ɂ���
        Vector3 dir = _target.position - _character.transform.position;
        dir.y = 0;
        Quaternion look = Quaternion.LookRotation(dir);
        // �������^�[�Q�b�g�̌����ɋ߂Â��Ă���
        _turret.rotation = Quaternion.Lerp(_turret.rotation, look, 0.05f);

        if (!FindTarget())
        {
            ChangeState(new BatteryEnemySearch(_character, _target, _anim, _turret, _findIcon));
        }
    }

    public override void Exit()
    {
        if (_disposable != null)
            _disposable.Dispose();
        ActiveFindIcon(false);
        _event = Event.Exit;
    }

    public void Dispose()
    {
        // �O������j������邱�Ƃ�����̂ł����ł��Ă�
        _disposable.Dispose();
    }
}

/// <summary>
/// ���S�ɒ�~:����ȏ㓮�����Ȃ���Ԃ̃N���X
/// </summary>
public class BatteryEnemyCompleted : BatteryEnemyBase
{
    public BatteryEnemyCompleted(GameObject character, Transform target, Animator anim, Transform _, GameObject findIcon)
        : base(character, target, anim, null, findIcon)
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