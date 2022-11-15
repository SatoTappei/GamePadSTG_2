using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵の行動をStateパターンで実装する
/// その場から動かず発見次第撃ってくる敵
/// の各ステートの基底クラス
/// </summary>
public class BatteryEnemyBase :StateMachineBase
{
    /// <summary>キャラクターの状態</summary>
    public enum State
    {
        Idle, Search, Capture, Completed
    };

    public State CurrentState;

    protected Transform _turret;
    protected Transform _muzzle;

    // 各アニメーション名
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
        // タレットの子オブジェクトにMuzzleという名前のオブジェクトが必要
        if(_turret != null)
            _muzzle = _turret.Find("Muzzle");
    }

    protected override bool FindTarget()
    {
        // 砲塔の前向きを基準に角度内にターゲットがいるか調べる
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
/// アイドル:砲塔を動かさず、弾も撃たない状態のクラス
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
/// 探索:視線を動かして敵を探索する状態のクラス
/// </summary>
public class BatteryEnemySearch : BatteryEnemyBase
{
    /// <summary>砲塔が正面を向いたか</summary>
    bool _lookedFront;
    /// <summary>砲塔を回転させるために使う三角関数用のカウント</summary>
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
            ChangeState(new BatteryEnemyCapture(_character, _target, _anim, _turret, _findIcon));
        }
    }

    public override void Exit() => base.Exit();
}

/// <summary>
/// 捕捉:敵を狙って攻撃する状態のクラス
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
        // TODO:攻撃する状態になったら毎回GetComponentしてくるので
        //      他に良い方法が見つかったら変える
        _pool = _character.GetComponent<ObjectPool>();
        _disposable = Observable.Interval(System.TimeSpan.FromSeconds(2.0f)).Subscribe(_ =>
        {
            if(_pool != null)
            {
                // 攻撃処理
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

        // ターゲットの方向を計算して、Y軸方向の回転を防ぐためにYを0にする
        Vector3 dir = _target.position - _character.transform.position;
        dir.y = 0;
        Quaternion look = Quaternion.LookRotation(dir);
        // 向きをターゲットの向きに近づけていく
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
        // 外部から破棄されることがあるのでここでも呼ぶ
        _disposable.Dispose();
    }
}

/// <summary>
/// 完全に停止:これ以上動かさない状態のクラス
/// </summary>
public class BatteryEnemyCompleted : BatteryEnemyBase
{
    public BatteryEnemyCompleted(GameObject character, Transform target, Animator anim, Transform _, GameObject findIcon)
        : base(character, target, anim, null, findIcon)
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