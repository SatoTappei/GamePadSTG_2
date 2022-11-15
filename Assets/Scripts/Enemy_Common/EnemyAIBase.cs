using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using System;

/// <summary>
/// 敵の行動をStateパターンで実装している
/// 敵のAIクラスはこのクラスを継承して作ること
/// </summary>
public abstract class EnemyAIBase : MonoBehaviour
{
    /// <summary>敵を機能させるかどうかを判断する</summary>
    protected bool _isWakeUp;
    /// <summary>現在のステートに対応したクラス</summary>
    protected StateMachineBase _currentStateClass;

    protected async void Start()
    {
        await UniTask.WaitUntil(() => _isWakeUp);
        Init();

        this.UpdateAsObservable()
            .Subscribe(_ => Stay())
            .AddTo(this);
    }
    
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Exit();
        }
#endif
    }

    /// <summary>敵として機能させる</summary>
    public void WakeUp() => _isWakeUp = true;
    
    /// <summary>敵として機能させた時に最初の一回だけ呼ばれる</summary>
    public abstract void Init();

    /// <summary>敵として機能している間毎フレーム呼ばれる</summary>
    public void Stay()
    {
        _currentStateClass = _currentStateClass.Process();
    }

    /// <summary>これ以上動かないように敵を止める時に他のスクリプトから呼ぶ</summary>
    public void Exit()
    {
        if (_currentStateClass != null)
        {
            _currentStateClass.ToCompleted();
            _currentStateClass.Process();
        }
    }
}
