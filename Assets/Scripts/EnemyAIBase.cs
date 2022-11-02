using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 敵の行動をStateパターンで実装している
/// 敵のAIクラスはこのクラスを継承して作ること
/// </summary>
public abstract class EnemyAIBase : MonoBehaviour
{
    /// <summary>敵を機能させるかどうかを判断する</summary>
    protected bool _isWakeUp;

    protected async void Start()
    {
        // 機能させるかどうかを管理してもらうために自身を登録する
        // 他のところで使いまわす場合は下の行を消せば良い
        FindObjectOfType<EnemyManager>().AddEnemyList(this);

        await UniTask.WaitUntil(() => _isWakeUp);
        Init();

        this.UpdateAsObservable()
            .Subscribe(_ => Stay())
            .AddTo(this);
    }
    
    void Update()
    {
        
    }

    /// <summary>敵として機能させる</summary>
    public void WakeUp() => _isWakeUp = true;
    /// <summary>敵として機能させた時に最初の一回だけ呼ばれる</summary>
    public abstract void Init();
    /// <summary>敵として機能している間毎フレーム呼ばれる</summary>
    public abstract void Stay();
}
