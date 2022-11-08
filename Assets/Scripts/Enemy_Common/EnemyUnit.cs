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
    ActorDataSO _actorData;
    int _currentHP;

    [SerializeField] CharacterTag _enemyTag;

    public CharacterTag EnemyTag { get => _enemyTag; }
    public Sprite Icon { get => _actorData.Icon; }

    void Awake()
    {
        _aiBase = GetComponent<EnemyAIBase>();
    }

    void Start()
    {
        EnemyManager em = FindObjectOfType<EnemyManager>();
        //// 共通したデータの参照先を取得する
        _actorData = em.GetEnemyData(EnemyTag);
        _currentHP = _actorData.MaxHP;
    }

    void Update()
    {
        
    }

    // 敵を起こす
    public void WakeUp()
    {
        _aiBase.WakeUp();
    }

    /// <summary>ダメージを受けた際の演出</summary>
    protected override void OnDamageReceived()
    {
        transform.DOShakePosition(InGameUtility.HitStopTime, 0.15f, 25, fadeOut: false);
        // ダメージを受けたときに死んだかどうか判定したい
        // HPを減らして0以下だったらと否かで分岐する
        _currentHP -= 30; // テスト固定値
        if (_currentHP <= 0)
        {
            // 倒されたら非表示になる
            gameObject.SetActive(false);
        }
    }

    /// <summary>ダメージを与えた際の演出</summary>
    protected override void OnDamageSended()
    {
        // 未実装
    }
}
