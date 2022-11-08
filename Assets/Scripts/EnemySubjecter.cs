using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 後々良い感じの場所に移す
/// <summary>キャラクターを識別するためのタグとして使う</summary>
public enum CharacterTag
{
    Player,
    BlueSoldier,
    Tank,
    BossTank,
}

/// <summary>
/// 敵の情報をEnemyManagerに登録する
/// </summary>
public class EnemySubjecter : MonoBehaviour
{
    EnemyAIBase _aiBase;
    ActorDataSO _actorData;
    int _currentHP;

    [SerializeField] DamageReceiver _damageReceiver;
    [SerializeField] DamageSender _damageSender;
    [SerializeField] CharacterTag _enemyTag;

    public CharacterTag EnemyTag { get => _enemyTag; }
    public Sprite Icon { get => _actorData.Icon; }

    void Awake()
    {
        _aiBase = GetComponent<EnemyAIBase>();
        //_currentHP = /*_actorData.MaxHP*/100;
    }

    void OnEnable()
    {
        if (_damageSender != null)
            _damageSender.OnDamageSended += OnDamageSended;
        _damageReceiver.OnDamageReceived += OnDamageReceived;

    }

    void OnDisable()
    {
        if (_damageSender != null)
            _damageSender.OnDamageSended -= OnDamageSended;
        _damageReceiver.OnDamageReceived -= OnDamageReceived;
    }

    void Start()
    {
        //// TODO:現在は敵を増やした時のためにこっちから管理するよう登録しているが
        ////      EnemyManagerから登録するように変更できないか模索する
        EnemyManager em = FindObjectOfType<EnemyManager>();
        //// 機能させるかどうかを管理してもらうために自身を登録する
        //em.AddEnemyList(this);
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
    void OnDamageReceived()
    {
        transform.DOShakePosition(ConstValue.HitStopTime, 0.15f, 25, fadeOut: false);
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
    void OnDamageSended()
    {
        // 未実装
    }
}
