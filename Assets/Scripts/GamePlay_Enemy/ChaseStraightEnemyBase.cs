using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵の行動をStateパターンで実装する
/// プレイヤーに向かって真っ直ぐ近づいて近接攻撃してくる敵
/// の各ステートの基底クラス
/// </summary>
public class ChaseStraightEnemyBase
{
    /// <summary>キャラクターの状態</summary>
    public enum State
    {
        Idle, Wander, Chase, Attack
    };

    /// <summary>ステート内のイベント</summary>
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

    // 各アニメーション名
    protected readonly string WalkAnim = "Run";
    protected readonly string AttackAnim = "Slash";
    protected readonly string IdleAnim = "Idle";

    // CharacterのY座標を決めるためのRay
    readonly float RayRadius = 0.01f;
    readonly float RayDistance = 10.0f;
    readonly Vector3 RayOffset = new Vector3(0.0f, 2.5f, 0.0f);
    readonly LayerMask RayMask = 1 << 6;

    /// <summary>視界の距離</summary>
    readonly float SightRange = 10.0f;
    /// <summary>視界の角度</summary>
    readonly float SightAngle = 100.0f;
    /// <summary>攻撃してくる距離</summary>
    readonly float AttackRange = 1.2f;

    public ChaseStraightEnemyBase(GameObject character, Transform target, Animator anim)
    {
        _character = character;
        _target = target;
        _anim = anim;
    }

    /// <summary>Stateに推移した際、1度だけ呼ばれる</summary>
    public virtual void Enter() => _event = Event.Stay;
    /// <summary>Enterが呼ばれた後、Exitになるまで毎フレーム呼ばれる</summary>
    public virtual void Update() => _event = Event.Stay;
    /// <summary>次のStateに推移する際、1度だけ呼ばれる</summary>
    public virtual void Exit() => _event = Event.Exit;

    /// <summary>
    /// 現在のイベントに対応したメソッドを呼び出して
    /// 次フレームでの状態クラスを返す
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

    /// <summary>ステートを変える</summary>
    protected void ChangeState(ChaseStraightEnemyBase next)
    {
        _nextState = next;
        _event = Event.Exit;
    }

    /// <summary>ターゲットが視界に入っているか</summary>
    protected bool FindTarget()
    {
        // ターゲットと自身の距離ベクトルを求める
        Vector3 diff = _target.position - _character.transform.position;
        // ターゲットとの角度を計算
        float angle = Vector3.Angle(diff, _character.transform.forward);
        // ターゲットが視界内にいるかを返す
        bool inSight = diff.magnitude < SightRange && angle < SightAngle;
        return inSight ? true : false;
    }

    /// <summary>対象との距離が攻撃可能か調べる</summary>
    protected bool CheckCanAttack()
    {
        Vector3 diff = _target.position - _character.transform.position;
        return diff.magnitude <= AttackRange;
    }

    /// <summary>レイを真下に飛ばして下に地面があるか調べる</summary>
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

    /// <summary>Y座標をセットする</summary>
    protected void SetCharacterPosY(float y)
    {
        Vector3 pos = _character.transform.position;
        pos.y = y;
        _character.transform.position = pos;
    }
}

/// <summary>
/// アイドル:その場で立ち止まっている状態のクラス
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

        // ターゲットが視界に入っていれば追跡を始める
        if (FindTarget())
        {
            ChangeState(new ChaseStraightEnemyChase(_character, _target, _anim));
        }
        // ターゲットが視界に入っていない場合
        // 3％の確率でうろうろし始める
        else if (Random.Range(0, 100) == 3)
        {
            ChangeState(new ChaseStraightEnemyWander(_character, _target, _anim));
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// うろうろ:ある一点を基準にその周辺に移動する状態のクラス
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
        // 1％の確率で停止し、アイドル状態に戻す
        if (Random.Range(0, 100) <= 1)
        {
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim));
            return;
        }

        _character.transform.position += _dir * Time.deltaTime * Speed;
        _character.transform.rotation = Quaternion.LookRotation(_dir);

        // 真下に床があるか調べる
        if (CheckFloor(out float y))
        {
            _prevPos = _character.transform.position;
        }
        else
        {
            // 真下が床ではない場合は前フレームの位置に戻す
            _character.transform.position = _prevPos;
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim));
            return;
        }

        // 移動後にYだけ変える
        SetCharacterPosY(y);

        // ターゲットを見つけた場合は追跡状態にする
        if (FindTarget())
        {
            ChangeState(new ChaseStraightEnemyChase(_character, _target, _anim));
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// 追跡:ターゲットに向かって真っ直ぐ向かっていく状態のクラス
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

        // ターゲットとの距離が攻撃可能な距離なら攻撃状態にする
        if (CheckCanAttack())
        {
            ChangeState(new ChaseStraightEnemyAttack(_character, _target, _anim));
        }
        // ターゲットを見失ったらアイドル状態に戻す
        else if (!FindTarget())
        {
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim));
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// 攻撃:その場に立ち止まってターゲットに攻撃をする状態のクラス
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
        // 攻撃状態になった時に一度攻撃して以後2秒に1回攻撃する
        _anim.Play(AttackAnim);
        _disposable = Observable.Interval(System.TimeSpan.FromSeconds(2.0f)).Subscribe(_ =>
        {
            _anim.Play(AttackAnim);
        });

        _event = Event.Stay;
    }

    public override void Update()
    {
        // 攻撃範囲外に出たらアイドル状態に戻す
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