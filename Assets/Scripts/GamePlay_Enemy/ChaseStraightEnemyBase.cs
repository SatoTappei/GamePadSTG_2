using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>視界の距離</summary>
    [SerializeField] float _sightRange = 10.0f;
    /// <summary>視界の角度</summary>
    [SerializeField] float _sightAngle = 30.0f;
    /// <summary>攻撃してくる距離</summary>
    [SerializeField] float _attackRange = 7.0f;

    public ChaseStraightEnemyBase(GameObject character, Transform target)
    {
        _character = character;
        _target = target;
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

    /// <summary>ターゲットが視界に入っているか</summary>
    protected bool FindTarget()
    {
        // ターゲットと自身の距離ベクトルを求める
        Vector3 diff = _target.position - _character.transform.position;
        // ターゲットとの角度を計算
        float angle = Vector3.Angle(diff, _character.transform.forward);
        // ターゲットが視界内にいるかを返す
        bool inSight = diff.magnitude < _sightRange && angle < _sightAngle;
        return inSight ? true : false;
    }
}

/// <summary>
/// アイドル:その場で立ち止まっている状態のクラス
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
        // ターゲットが視界に入っていれば追跡を始める
        if (FindTarget())
        {
            _nextState = new ChaseStraightEnemyChase(_character, _target);
            _event = Event.Exit;
        }
        // ターゲットが視界に入っていない場合
        // 確率でうろうろし始める
        else if (Random.Range(0, 10) == 0)
        {
            _nextState = new ChaseStraightEnemyWander(_character, _target);
            _event = Event.Exit;
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// うろうろ:ある一点を基準にその周辺を行ったり来たりする状態のクラス
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
/// 追跡:ターゲットに向かって真っ直ぐ向かっていく状態のクラス
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
/// 攻撃:その場に立ち止まってターゲットに攻撃をする状態のクラス
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