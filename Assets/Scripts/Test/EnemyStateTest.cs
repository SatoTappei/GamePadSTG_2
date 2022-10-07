
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Stateパターンを用いた実装のテスト
/// 敵の移動を制御する
/// </summary>
public class EnemyStateTest
{
    /// <summary>敵キャラが取りうる状態</summary>
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK
    };
    /// <summary>STATE内のイベント</summary>
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    /// <summary>現在の状態</summary>
    public STATE _name;
    /// <summary>現在のイベント</summary>
    protected EVENT _stage;
    /// <summary>敵キャラのゲームオブジェクト</summary>
    protected GameObject _enemy;
    /// <summary>プレイヤーのTransform</summary>
    protected Transform _player;
    /// <summary>次の状態</summary>
    protected EnemyStateTest _nextState;
    /// <summary>敵キャラのNavMeshAgentコンポーネント</summary>
    protected NavMeshAgent _agent;

    /// <summary>"Vis"ible"Dist"anceプレイヤーを認識する距離</summary>
    readonly float _visDist = 10.0f;
    /// <summary>"Vis"ible"Angle"プレイヤーを認識する角度</summary>
    readonly float _visAngle = 30.0f;
    /// <summary>プレイヤーを攻撃する距離</summary>
    readonly float _shootDist = 7.0f;

    /// <summary>
    /// このスクリプトのクラスのコンストラクタ
    /// ここに全ての状態で使用する情報を設定する
    /// </summary>
    public EnemyStateTest(GameObject enemy, NavMeshAgent agent, Transform player)
    {
        _enemy = enemy;
        _agent = agent;
        _stage = EVENT.ENTER;
        _player = player;
    }

    /// <summary>ある状態になると最初に実行される</summary>
    public virtual void Enter() => _stage = EVENT.UPDATE;
    /// <summary>ある状態中、ステートが変わるまで実行される</summary>
    public virtual void Update() => _stage = EVENT.UPDATE;
    /// <summary>ある状態からステートが変化するときに実行される</summary>
    public virtual void Exit() => _stage = EVENT.EXIT;

    /// <summary>外部から呼び出して、各ステージで状態を進行させる</summary>
    public EnemyStateTest Process()
    {
        if (_stage == EVENT.ENTER) Enter();
        if (_stage == EVENT.UPDATE) Update();
        if (_stage == EVENT.EXIT)
        {
            // 現在の状態から抜ける際に次のステートを返す
            Exit();
            return _nextState;
        }

        // 現在のステートを返す
        return this;
    }

    /// <summary>敵キャラの前方にプレイヤーがいるか</summary>
    public bool CanSeePlayer()
    {
        // 敵キャラからプレイヤーへのベクトルを計算
        Vector3 dir = _player.position - _enemy.transform.position;
        // 視覚を計算
        float angle = Vector3.Angle(dir, _enemy.transform.forward);
        // プレイヤーが近くにいて、かつ見える範囲にいる場合
        if (dir.magnitude < _visDist && angle < _visAngle)
        {
            return true;
        }
        return false;
    }
}

/// <summary>
/// アイドル状態:その場で立ち止まる
/// </summary>
public class Idle : EnemyStateTest
{
    public Idle(GameObject enemy, NavMeshAgent agent, Transform player) : base(enemy, agent, player)
    {
        _name = STATE.IDLE;
    }

    public override void Enter()
    {
        // ステージを"UPDATE"にする
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            // 本ステータスのステージがEXITになり、nextStageで指定したステータスになる
            _nextState = new Pursue(_enemy, _agent, _player);
            _stage = EVENT.EXIT;
        }
        // 10%の確率でIDLE状態からPatrol状態に推移
        else if (Random.Range(0, 100) < 10)
        {
            _nextState = new Patrol(_enemy, _agent, _player);
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

/// <summary>
/// パトロール状態:各ウェイポイントを巡回する
/// </summary>
public class Patrol : EnemyStateTest
{
    int currentIndex = -1;
    public Patrol(GameObject _enemy, NavMeshAgent _agent, Transform _player) : base(_enemy, _agent, _player)
    {
        _name = STATE.PATROL;
        _agent.speed = 2;         // 巡回時の速度
        _agent.isStopped = false; // パス探索の開始/停止制御(false = 開始)
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity; // 敵キャラとの距離

        // 各ウェイポイントをループして、敵キャラと各ウェイポイント間の距離を計算し、
        // 最も近いウェイポイントを算出する
        for (int i = 0; i < WayPointManagerTest.Singleton.Waypoints.Count; i++)
        {
            var thisWP = WayPointManagerTest.Singleton.Waypoints[i];
            var distance = Vector3.Distance(_enemy.transform.position, thisWP.transform.position);
            if (distance < lastDist)
            {
                // Updateではiに1を加えてから次の目的地を設定するために1を引く
                currentIndex = i - 1;
                lastDist = distance;
            }
        }

        // 目的地の設定
        var newWayPoint = WayPointManagerTest.Singleton.Waypoints[currentIndex];
        _agent.SetDestination(newWayPoint.transform.position);

        base.Enter();
    }

    public override void Update()
    {
        // ウェイポイントに到達しているか確認
        if (_agent.remainingDistance < 1)
        {
            // 次のウェイポイントに移動(何故か剰余を使わない)
            if (currentIndex >= WayPointManagerTest.Singleton.Waypoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;

            // 目的地の設定
            var newWayPoint = WayPointManagerTest.Singleton.Waypoints[currentIndex];
            _agent.SetDestination(newWayPoint.transform.position);
        }

        // プレイヤーを見つけたらPursue(追跡)状態に推移
        if (CanSeePlayer())
        {
            _nextState = new Pursue(_enemy, _agent, _player);
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

/// <summary>
/// 追跡状態:プレイヤーを視認し、追跡する
/// </summary>
public class Pursue : EnemyStateTest
{
    public Pursue(GameObject enemy, NavMeshAgent agent, Transform player) : base(enemy, agent, player)
    {
        _name = STATE.PURSUE; // 現在の状態を設定
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();
    }
}

/// <summary>
/// 攻撃状態:???
/// </summary>
public class Attack : EnemyStateTest
{
    // 敵キャラがプレイヤーに向かって回転する速度を設定
    float rotationSpeed = 2.0f;
    public Attack(GameObject enemy, NavMeshAgent agent, Transform player) : base(enemy, agent, player)
    {
        _name = STATE.ATTACK; // 現在の状態を設定
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();
    }
}