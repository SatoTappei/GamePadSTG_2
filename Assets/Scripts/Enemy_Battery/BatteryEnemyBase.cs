using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵の行動をStateパターンで実装する
/// その場から動かず発見次第撃ってくる敵
/// の各ステートの基底クラス
/// </summary>
public class BatteryEnemyBase
{
    /// <summary>キャラクターの状態</summary>
    public enum State
    {
        Idle, Search, Capture
    };

    /// <summary>ステート内のイベント</summary>
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

    // 各アニメーション名
    protected readonly string IdleAnim = "Idle";

    // CharacterのY座標を決めるためのRay
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
        // タレットの子オブジェクトにMuzzleという名前のオブジェクトが必要
        _muzzle = _turret.Find("Muzzle");
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

    /// <summary>ステートを変える</summary>
    protected void ChangeState(BatteryEnemyBase next)
    {
        _nextState = next;
        _event = Event.Exit;
    }

    /// <summary>ターゲットが視界に入っているか</summary>
    //protected bool FindTarget()
    //{
    //    // ターゲットと自身の距離ベクトルを求める
    //    Vector3 diff = _target.position - _character.transform.position;
    //    // ターゲットとの角度を計算
    //    float angle = Vector3.Angle(diff, _character.transform.forward);
    //    // ターゲットが視界内にいるかを返す
    //    bool inSight = diff.magnitude <= SightRange && angle <= SightAngle;
    //    return inSight;
    //}
    protected bool FindTarget()
    {
        Vector3 diff = _target.position - _character.transform.position;
        float angle = Vector3.Angle(diff, _turret.transform.forward);
        bool isSight = diff.magnitude <= SightRange && angle <= SightAngle;
        return isSight;
    }

    /// <summary>対象との距離が攻撃可能か調べる</summary>
    //protected bool CheckCanAttack()
    //{
    //    Vector3 diff = _target.position - _character.transform.position;
    //    return diff.magnitude <= AttackRange;
    //}

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
/// アイドル:視線を動かさず、弾も撃たない状態のクラス
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
        Debug.Log("アイドル状態です");

        CheckFloor(out float y);
        SetCharacterPosY(y);
        ChangeState(new BatteryEnemySearch(_character, _target, _anim, _turret));
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// 探索:視線を動かして敵を探索する状態のクラス
/// </summary>
public class BatteryEnemySearch : BatteryEnemyBase
{
    /// <summary>砲塔が正面を向いたか</summary>
    bool _lookedFront;
    /// <summary>砲塔を回転させるために使う三角関数用のカウント</summary>
    float _count;

    public BatteryEnemySearch(GameObject character, Transform target, Animator anim, Transform turret)
        : base(character, target, anim, turret)
    {
        CurrentState = State.Search;
    }

    public override void Enter() => base.Enter();

    public override void Update()
    {
        CheckFloor(out float y);
        SetCharacterPosY(y);

        // 探索状態になったらまず砲塔を正面を向かせる
        if (!_lookedFront)
        {
            Quaternion look = Quaternion.LookRotation(_character.transform.forward);
            _turret.rotation = Quaternion.Lerp(_turret.rotation, look, 0.05f);
            // 砲塔の正面と戦車の正面の角度がほぼ同じになったら正面を向いたとみなす
            float angle = Vector3.Angle(_turret.forward, _character.transform.forward);
            if (angle < 0.01f)
                _lookedFront = true;
        }
        // 正面を向いた後はくるくる回転させる
        else
        {
            // 正面に向かせる状態のときはカウントを足さないようにする
            _count += Time.deltaTime;
            float sin = Mathf.Sin(_count);
            float cos = Mathf.Cos(_count);

            Vector3 dir = new Vector3(sin, 0, -1 * cos);
            Quaternion look = Quaternion.LookRotation(dir);
            _turret.rotation = Quaternion.Lerp(_turret.rotation, look, 0.05f);
        }

        if (FindTarget())
        {
            ChangeState(new BatteryEnemyCapture(_character, _target, _anim, _turret));
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// 捕捉:敵を狙って攻撃する状態のクラス
/// </summary>
public class BatteryEnemyCapture : BatteryEnemyBase
{
    ObjectPool _pool;
    System.IDisposable _disposable;

    public BatteryEnemyCapture(GameObject character, Transform target, Animator anim, Transform turret)
        : base(character, target, anim, turret)
    {
        CurrentState = State.Capture;
    }

    public override void Enter()
    {
        // TODO:攻撃する状態になったら毎回GetComponentしてくるので
        //      他に良い方法が見つかったら変える
        _pool = _character.GetComponent<ObjectPool>();
        _disposable = Observable.Interval(System.TimeSpan.FromSeconds(2.0f)).Subscribe(_ =>
        {
            // 攻撃処理
            GameObject go = _pool.GetPooledObject();
            go.SetActive(true);
            go.transform.position = _muzzle.position;
            go.transform.forward = _muzzle.forward;
        });

        _event = Event.Stay;
    }

    public override void Update()
    {
        CheckFloor(out float y);
        SetCharacterPosY(y);

        // ターゲットの方向を計算して、Y軸方向の回転を防ぐためにYを0にする
        Vector3 dir = _target.position - _character.transform.position;
        dir.y = 0;
        Quaternion look = Quaternion.LookRotation(dir);
        // 向きをターゲットの向きに近づけていく
        _turret.rotation = Quaternion.Lerp(_turret.rotation, look, 0.05f);

        if (!FindTarget())
        {
            ChangeState(new BatteryEnemySearch(_character, _target, _anim, _turret));
        }
    }

    public override void Exit()
    {
        _disposable.Dispose();
    }
}