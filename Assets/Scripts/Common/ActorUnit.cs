using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

/// <summary>
/// 各Unitクラス(EnemyUnit,PlayerUnit)の基底クラス
/// </summary>
public abstract class ActorUnit : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReciever;
    [SerializeField] DamageSender _damageSender;
    [SerializeField] CharacterTag _characterTag;
    [Header("Unityの機能のタグ")] // TODO:ここをCharacterTagに直したい、staticメソッドで判定して…みたいな
    [SerializeField] string _targetTag;

    /// <summary>ダメージを受けて体力が減った際にUI等に反映するため</summary>
    public IObservable<int> OnDamageObservable => _damageReciever.CurrentHP;

    public ActorDataSO ActorData { get; private set; }

    void Start()
    {
        // TODO:PlaySceneManagerもStart()でメソッドを実行しているので処理順によってはぬるぽ
        // アクターデータを参照先からとってくる、要CharacterTag
        ActorDataManager adm = FindObjectOfType<ActorDataManager>();
        ActorData = adm.GetEnemyData(_characterTag);

        // 体力はレシーバーに設定
        if (_damageReciever != null)
            _damageReciever.Init(ActorData.MaxHP);
        // 攻撃力とターゲットはセンダーに設定
        if (_damageSender != null)
            _damageSender.Init(ActorData.Attack, _targetTag);
    }

    void OnEnable()
    {
        if(_damageReciever != null)
        {
            _damageReciever.OnDamageReceived += OnDamageReceived;
            _damageReciever.OnDeath += OnDeath;
        }
        if (_damageSender != null)
            _damageSender.OnDamageSended += OnDamageSended;
    }

    void OnDisable()
    {
        if (_damageReciever != null)
        {
            _damageReciever.OnDamageReceived -= OnDamageReceived;
            _damageReciever.OnDeath -= OnDeath;
        }
        if (_damageSender != null)
            _damageSender.OnDamageSended -= OnDamageSended;
    }

    protected abstract void OnDamageReceived();
    protected abstract void OnDeath();
    protected abstract void OnDamageSended();
}
