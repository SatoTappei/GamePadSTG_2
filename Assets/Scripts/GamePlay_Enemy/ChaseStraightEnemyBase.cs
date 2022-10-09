using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

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

    // CharacterのY座標を決めるためのRay
    readonly float RayRadius = 0.01f;
    readonly float RayDistance = 10.0f;
    readonly Vector3 RayOffset = new Vector3(0.0f, 2.5f, 0.0f);
    readonly LayerMask RayMask = 1 << 6;

    /// <summary>視界の距離</summary>
    readonly float SightRange = 10.0f;
    /// <summary>視界の角度</summary>
    readonly float SightAngle = 30.0f;
    /// <summary>攻撃してくる距離</summary>
    readonly float AttackRange = 7.0f;

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
        bool inSight = diff.magnitude < SightRange && angle < SightAngle;
        return inSight ? true : false;
    }

    /// <summary>レイを真下に飛ばして下に地面があるか調べる</summary>
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
        Vector3 pos = _character.transform.position;
        GetCharacterPosY(out float y);
        pos.y = y;
        _character.transform.position = pos;

        // ターゲットが視界に入っていれば追跡を始める
        if (FindTarget())
        {
            _nextState = new ChaseStraightEnemyChase(_character, _target);
            _event = Event.Exit;
        }
        // ターゲットが視界に入っていない場合
        // 3％の確率でうろうろし始める
        else if (Random.Range(0, 100) == 3)
        {
            _nextState = new ChaseStraightEnemyWander(_character, _target);
            _event = Event.Exit;
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// うろうろ:ある一点を基準にその周辺に移動する状態のクラス
/// </summary>
public class ChaseStraightEnemyWander : ChaseStraightEnemyBase
{
    /// <summary>XZ平面での移動する方向、Yはレイで判定する</summary>
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
        // 1％の確率で停止し、アイドル状態に戻す
        if (Random.Range(0, 100) <= 1)
        {
            _nextState = new ChaseStraightEnemyIdle(_character, _target);
            _event = Event.Exit;
            return;
        }

        // 真下に床があるか調べる
        bool checkFloor = GetCharacterPosY(out float y);
        if (checkFloor)
        {
            _character.transform.position += _dir * Time.deltaTime * Speed;
        }
        else
        {
            // 真下が床ではない場合は逆方向に1フレーム分だけ進ませ、アイドル状態に戻す
            _character.transform.position += -1 * _dir * Time.deltaTime * Speed;
            _nextState = new ChaseStraightEnemyIdle(_character, _target);
            _event = Event.Exit;
            return;
        }

        // 移動後にYだけ変える
        Vector3 pos = _character.transform.position;
        pos.y = y;
        _character.transform.position = pos;

        // ターゲットを見つけた場合は追跡状態にする
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

    public override void Update()
    {
        // 軽くするためにRayと座標書き換えで動かす
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