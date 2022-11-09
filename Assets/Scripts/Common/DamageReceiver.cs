using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// DamageSenderからダメージを受けたことを受信する
/// HPの管理はこのクラスが担う
/// </summary>
public class DamageReceiver : MonoBehaviour, IDamageable
{
    /// <summary>ノックバック力</summary>
    [SerializeField] float _knockBackPower = 3.0f;
    /// <summary>無敵時間</summary>
    [SerializeField] float _invincibleTime = 2.0f;
    /// <summary>ダメージを受けた際のエフェクトのプレハブ</summary>
    [SerializeField] GameObject _damageEffectPrefab;
    /// <summary>表示非表示を切り替えて使いまわすダメージエフェクト</summary>
    GameObject _damageEffect;
    /// <summary>現在の体力</summary>
    int _currentHP = 0;
    /// <summary>無敵時間中かどうかのフラグ</summary>
    bool _isInvincible = true;
    /// <summary>ダメージを受けたときに行う追加の処理</summary>
    public UnityAction OnDamageReceived;

    void Awake()
    {
        // 使いまわせるようにダメージエフェクトをインスタンス化して非アクティブにしておく
        if (_damageEffectPrefab != null)
        {
            _damageEffect = Instantiate(_damageEffectPrefab);
            _damageEffect.SetActive(false);
        }
    }

    // TODO:レイキャストを使用したヒットバックのキャンセルをしたい
    //void Update()
    //{
    //    Vector3 rayPos = transform.position + new Vector3(0, 5f, 0);
    //    Ray ray = new Ray(rayPos, Vector3.down);

    //    bool isHit = Physics.Raycast(ray,)
    //}

    /// <summary>初期化処理、これが呼ばれるまで敵が無敵状態のまま</summary>
    public void Init(int maxHP)
    {
        _currentHP = maxHP;
        _isInvincible = false;
    }

    /// <summary>攻撃を受けたときの処理</summary>
    public void OnDamage(int damage, Vector3 hitPos)
    {
        // 無敵時間中は攻撃を受けない
        if (_isInvincible) return;
        // 無敵時間を設定する
        DOTween.Sequence()
               .AppendInterval(_invincibleTime)
               .OnStart(() => _isInvincible = true)
               .OnComplete(() => _isInvincible = false);

        // 衝突位置にパーティクルを発生させる
        if (_damageEffect != null) 
            EnabledDamageEffect(hitPos);

        // ヒットストップの後、ノックバックさせる
        DOVirtual.DelayedCall(InGameUtility.HitStopTime, () => KnockBack(hitPos, _knockBackPower));

        // ダメージを反映させる
        _currentHP -= damage;
        if(_currentHP < 0)
        {
            // 倒されたら非表示になる
            // TODO:死亡時の演出を作る
            gameObject.SetActive(false);
        }
        else
        {
            OnDamageReceived?.Invoke();
        }
    }

    /// <summary>ダメージエフェクトを発生させる</summary>
    void EnabledDamageEffect(Vector3 pos)
    {
        _damageEffect.transform.position = pos;
        _damageEffect.SetActive(true);
    }

    /// <summary>ノックバックさせる</summary>
    void KnockBack(Vector3 hit, float power)
    {
        // ノックバックさせる方向を求める
        Vector3 pos = transform.position;
        pos.y = 0;
        hit.y = 0;
        Vector3 dir = Vector3.Normalize(pos - hit);

        // ノックバック
        transform.DOMove(dir * power, 0.5f).SetRelative();
    }
}
