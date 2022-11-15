using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 敵の行動をStateパターンで実装する
/// プレイヤーに向かって真っ直ぐ近づいて近接攻撃してくる敵
/// の各ステートの基底クラス
/// </summary>
public class ChaseStraightEnemyBase : StateMachineBase
{
    /// <summary>キャラクターの状態</summary>
    public  enum State
    {
        Idle, Wander, Chase, Attack, Completed
    };

    public State CurrentState;

    // 各アニメーション名
    protected readonly string WalkAnim = "Run";
    protected readonly string AttackAnim = "Slash";
    protected readonly string IdleAnim = "Idle";

    protected override float SightRange => 10.0f;
    protected override float SightAngle => 100.0f;
    protected override float AttackRange => 1.0f;

    public ChaseStraightEnemyBase(GameObject character, Transform target, Animator anim, GameObject findIcon)
    {
        _character = character;
        _target = target;
        _anim = anim;
        _findIcon = findIcon;
    }

    protected override bool FindTarget()
    {
        // キャラクターの前向きを基準に角度内にターゲットがいるか調べる
        Vector3 diff = _target.position - _character.transform.position;
        float angle = Vector3.Angle(diff, _character.transform.forward);
        bool inSight = diff.magnitude <= SightRange && angle <= SightAngle;
        return inSight;
    }

    public override void ToCompleted()
    {
        _nextState = new ChaseStraightEnemyCompleted(_character, _target, _anim, _findIcon);
        _event = Event.Exit;
    }

    /// <summary>対象との距離が攻撃可能か調べる</summary>
    protected bool CheckCanAttack()
    {
        Vector3 diff = _target.position - _character.transform.position;
        return diff.magnitude <= AttackRange;
    }
}

/// <summary>
/// アイドル:その場で立ち止まっている状態のクラス
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

        // ターゲットが視界に入っていれば追跡を始める
        if (FindTarget())
        {
            ChangeState(new ChaseStraightEnemyChase(_character, _target, _anim, _findIcon));
        }
        // ターゲットが視界に入っていない場合
        // 3％の確率でうろうろし始める
        else if (UnityEngine.Random.Range(0, 100) == 3)
        {
            ChangeState(new ChaseStraightEnemyWander(_character, _target, _anim, _findIcon));
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
    readonly float RayDist = 1.5f;
    Vector3 _dir;
    // レイの発射間隔、壁に埋まる場合はここを見直す
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

        // 1％の確率で停止し、アイドル状態に戻す
        if (UnityEngine.Random.Range(0, 100) <= 1)
        {
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim, _findIcon));
            return;
        }
        // 一定間隔で前方向にRayを飛ばして壁にめり込まないようにする
        // ノックバックでは壁にめり込まなくなったので歩きで壁にめり込まなければ壁にめり込むことはない
        else if (_rayCount > _raydist && 
                 Physics.Raycast(_character.transform.position, _character.transform.forward, RayDist, Mask))
        {
            _rayCount = 0;
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim, _findIcon));
            return;
        }
        Debug.DrawRay(_character.transform.position, _character.transform.forward * RayDist, Color.red, 0);

        // 座標をセット
        _character.transform.position += _dir * Time.deltaTime * Speed;
        _character.transform.rotation = Quaternion.LookRotation(_dir);
        SetCharacterPosY();

        // ターゲットを見つけた場合は追跡状態にする
        if (FindTarget())
            ChangeState(new ChaseStraightEnemyChase(_character, _target, _anim, _findIcon));
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// 追跡:ターゲットに向かって真っ直ぐ向かっていく状態のクラス
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
        // ターゲットとの差から進行方向を求め、回転させる
        Vector3 diff = _target.position - _character.transform.position;
        Vector3 dir = new Vector3(diff.x, 0, diff.z);
        _character.transform.rotation = Quaternion.LookRotation(dir);
        SetCharacterPosY();

        // 前方向にRayを飛ばして壁にめり込まないようにする
        // ノックバックでは壁にめり込まなくなったので歩きで壁にめり込まなければ壁にめり込むことはない
        if (!Physics.Raycast(_character.transform.position, _character.transform.forward, RayDist, Mask))
        {
            _character.transform.position += dir.normalized * Time.deltaTime * Speed;
        }
        Debug.DrawRay(_character.transform.position, _character.transform.forward * RayDist, Color.red, 0);

        // ターゲットとの距離が攻撃可能な距離なら攻撃状態にする
        if (CheckCanAttack())
            ChangeState(new ChaseStraightEnemyAttack(_character, _target, _anim, _findIcon));
        // ターゲットを見失ったらアイドル状態に戻す
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
/// 攻撃:その場に立ち止まってターゲットに攻撃をする状態のクラス
/// </summary>
public class ChaseStraightEnemyAttack : ChaseStraightEnemyBase, IDisposable
{
    IDisposable _disposable;

    public ChaseStraightEnemyAttack(GameObject character, Transform target, Animator anim, GameObject findIcon)
        : base(character, target, anim, findIcon)
    {
        CurrentState = State.Attack;
    }

    public override void Enter()
    {
        // 攻撃状態になった時に一度攻撃して以後2秒に1回攻撃する
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
        // 攻撃範囲外に出たらアイドル状態に戻す
        if (!CheckCanAttack() && !_anim.GetCurrentAnimatorStateInfo(0).IsName(AttackAnim))
            ChangeState(new ChaseStraightEnemyIdle(_character, _target, _anim, _findIcon));

        SetCharacterPosY();
    }

    public override void Exit()
    {
        ActiveFindIcon(false);
        if (_disposable != null)
            _disposable.Dispose();
        _event = Event.Exit;
    }

    public void Dispose()
    {
        // 外部から破棄される事があるのでこっちでもDispose()を呼ぶ
        _disposable.Dispose();
    }
}

/// <summary>
/// 完全に停止:これ以上動かさない状態のクラス
/// </summary>
public class ChaseStraightEnemyCompleted : ChaseStraightEnemyBase
{
    public ChaseStraightEnemyCompleted(GameObject character, Transform target, Animator anim, GameObject findIcon)
        : base(character, target, anim, findIcon)
    {
        CurrentState = State.Completed;
    }

    /// <summary>Stateに推移した際、1度だけ呼ばれる</summary>
    public override void Enter() => base.Enter();
    /// <summary>Enterが呼ばれた後、Exitになるまで毎フレーム呼ばれる</summary>
    public override void Update() => base.Update();
    /// <summary>次のStateに推移する際、1度だけ呼ばれる</summary>
    public override void Exit() => base.Exit();
}