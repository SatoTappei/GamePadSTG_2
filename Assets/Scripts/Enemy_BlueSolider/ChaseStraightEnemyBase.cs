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
public class ChaseStraightEnemyBase
{
    /// <summary>キャラクターの状態</summary>
    public enum State
    {
        Idle, Wander, Chase, Attack, Completed
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
    protected GameObject _findIcon;

    // 各アニメーション名
    protected readonly string WalkAnim = "Run";
    protected readonly string AttackAnim = "Slash";
    protected readonly string IdleAnim = "Idle";

    // CharacterのY座標を決めるためのRay
    protected readonly LayerMask Mask = 1 << 6;
    readonly float RayRadius = 0.01f;
    readonly float RayDistance = 10.0f;
    readonly Vector3 RayOffset = new Vector3(0.0f, 2.5f, 0.0f);

    /// <summary>視界の距離</summary>
    readonly float SightRange = 10.0f;
    /// <summary>視界の角度</summary>
    readonly float SightAngle = 100.0f;
    /// <summary>攻撃してくる距離</summary>
    readonly float AttackRange = 1.0f;

    public ChaseStraightEnemyBase(GameObject character, Transform target, Animator anim, GameObject findIcon)
    {
        _character = character;
        _target = target;
        _anim = anim;
        _findIcon = findIcon;
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

    /// <summary>動作を完全に止める</summary>
    public void ChangeCompleted()
    {
        _nextState = new ChaseStraightEnemyCompleted(_character, _target, _anim, _findIcon);
        _event = Event.Exit;
    }

    /// <summary>ターゲット発見アイコンの表示を切り替える</summary>
    protected void ActiveFindIcon(bool value)
    {
        if (_findIcon != null)
            _findIcon.SetActive(value);
        // TODO: 音を鳴らす
    }

    /// <summary>ターゲットが視界に入っているか</summary>
    protected bool FindTarget()
    {
        // ターゲットと自身の距離ベクトルを求める
        Vector3 diff = _target.position - _character.transform.position;
        // ターゲットとの角度を計算
        float angle = Vector3.Angle(diff, _character.transform.forward);
        // ターゲットが視界内にいるかを返す
        bool inSight = diff.magnitude <= SightRange && angle <= SightAngle;
        return inSight;
    }

    /// <summary>対象との距離が攻撃可能か調べる</summary>
    protected bool CheckCanAttack()
    {
        Vector3 diff = _target.position - _character.transform.position;
        return diff.magnitude <= AttackRange;
    }

    /// <summary>Y座標をセットする</summary>
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
            // 何もしない
        }
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

    public ChaseStraightEnemyAttack(GameObject character, Transform target, Animator anim, GameObject findIcon = null)
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
        _disposable.Dispose();
        _event = Event.Exit;
    }

    public void Dispose()
    {
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