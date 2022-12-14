using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 敵キャラクターの制御を行う
/// </summary>
public class EnemyUnit : ActorUnit
{
    EnemyAIBase _aiBase;
    //ActorDataSO _actorData;
    //int _currentHP;

    //[SerializeField] CharacterTag _enemyTag;

    //public CharacterTag EnemyTag { get => _enemyTag; }
    //public Sprite Icon { get => _actorData.Icon; }

    void Awake()
    {
        _aiBase = GetComponent<EnemyAIBase>();
    }

    void Update()
    {

    }

    /// <summary>敵を起こす</summary>
    public void WakeUp() => _aiBase.WakeUp();

    /// <summary>敵を動かなくする</summary>
    public void Exit() => _aiBase.Exit();

    /// <summary>ダメージを受けた際の演出</summary>
    protected override void OnDamageReceived()
    {
        transform.DOShakePosition(InGameUtility.HitStopTime, 0.15f, 25, fadeOut: false);
    }

    /// <summary>死亡した際の処理</summary>
    protected override void OnDeath()
    {
        _aiBase.Exit();
        // 非表示になることが死亡判定のトリガーなのでここは変えない
        gameObject.SetActive(false);
    }

    /// <summary>ダメージを与えた際の演出</summary>
    protected override void OnDamageSended()
    {
        // 未実装
    }
}
