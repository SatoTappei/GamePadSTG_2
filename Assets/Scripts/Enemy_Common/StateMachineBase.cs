using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各ステートマシンの基底クラスの基底クラス
/// </summary>
public abstract class StateMachineBase
{
    /// <summary>ステート内のイベント</summary>
    protected enum Event
    {
        Enter, Stay, Exit
    };

    protected Event _event;
    protected StateMachineBase _nextState;
    protected GameObject _character;
    protected Transform _target;
    protected Animator _anim;
    protected GameObject _findIcon;

    // キャラクターの座標を更新するのに使うレイ周りの定数
    protected readonly LayerMask Mask = 1 << 6;
    readonly float RayRadius = 0.01f;
    readonly float RayDistance = 10.0f;
    readonly Vector3 RayOffset = new Vector3(0.0f, 2.5f, 0.0f);

    // 攻撃に使う定数は抽象クラスの仕様上、プロパティでしか実装出来ない
    protected abstract float SightRange { get; }
    protected abstract float SightAngle { get; }
    protected abstract float AttackRange { get; }

    /// <summary>Stateに推移した際、1度だけ呼ばれる</summary>
    public virtual void Enter() => _event = Event.Stay;

    /// <summary>Enterが呼ばれた後、Exitになるまで毎フレーム呼ばれる</summary>
    public virtual void Update() => _event = Event.Stay;

    /// <summary>次のStateに推移する際、1度だけ呼ばれる</summary>
    public virtual void Exit() => _event = Event.Exit;

    /// <summary>ターゲット発見アイコンの表示を切り替える</summary>
    protected void ActiveFindIcon(bool value)
    {
        if (_findIcon != null)
            _findIcon.SetActive(value);
        // TODO: 音を鳴らす
    }

    /// <summary>
    /// 現在のイベントに対応したメソッドを呼び出して
    /// 次フレームでの状態クラスを返す
    /// </summary>
    public StateMachineBase Process()
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

    /// <summary>ターゲットが視界に入っているか判定する</summary>
    protected abstract bool FindTarget();

    /// <summary>外部から動きを完全に止める</summary>
    public abstract void ToCompleted();

    /// <summary>状態を推移させる</summary>
    protected void ChangeState<T>(T next) where T : StateMachineBase
    {
        _nextState = next;
        _event = Event.Exit;
    }
}
