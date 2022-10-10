using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

// コライダーが付いたオブジェクトにアタッチするだけ
/// <summary>
/// DamageSenderからダメージを受けたことを受信する
/// </summary>
public class DamageReceiver : MonoBehaviour, IDamageable
{
    //Rigidbody _rb;
    /// <summary>ノックバック力</summary>
    [SerializeField] float _knockBackPower = 3.0f;
    /// <summary>無敵時間</summary>
    [SerializeField] float _invincibleTime = 2.0f;
    [SerializeField] GameObject _damageEffectPrefab;
    GameObject _damageEffect;

    /// <summary>ダメージを受けたときに行う追加の処理</summary>
    public event Action OnDamageReceived;

    bool _isInvincible;

    void Awake()
    {
        //_rb = GetComponent<Rigidbody>();

        // Awake時点でダメージエフェクトをインスタンス化して
        // 非アクティブにしておく
        if (_damageEffectPrefab != null)
        {
            _damageEffect = Instantiate(_damageEffectPrefab);
            _damageEffect.SetActive(false);
        }
    }

    /// <summary>敵から攻撃を受けたときの処理</summary>
    public void OnDamage(int damageValue, Vector3 hitPos)
    {
        // 無敵中の場合は攻撃を受けない
        if (_isInvincible)
        {
            Debug.Log("無敵時間中:" + gameObject.name);
            return;
        }
        else
        {
            Debug.Log("ダメージを受けた:" + gameObject.name);
        }

        // 衝突位置にパーティクルを発生
        if (_damageEffect != null)
        {
            _damageEffect.transform.position = hitPos;
            _damageEffect.SetActive(true);
        }

        // 一定時間無敵
        Sequence sequence = DOTween.Sequence()
            .AppendInterval(_invincibleTime)
            .OnStart(() => _isInvincible = true)
            .OnComplete(() => _isInvincible = false);

        // TODO:ノックバックさせると壁を貫通してしまうのでレイキャストを用いるなどして壁に埋まらないようにする
        KnockBack(hitPos, _knockBackPower);

        OnDamageReceived?.Invoke();
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
