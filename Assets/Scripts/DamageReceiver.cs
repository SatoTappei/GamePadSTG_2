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
    ///// <summary>Rigidbodyを使うかどうか</summary>
    //[SerializeField] bool _useRigidbody;
    [SerializeField] GameObject _damageEffectPrefab;
    GameObject _damageEffect;

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

        // 各種演出
        Sequence seqE = DOTween.Sequence();
        // 後方に移動
        Vector3 knockBackVec = GetAngleVec(hitPos, transform.position);
        seqE.Append(transform.DOMove(knockBackVec * _knockBackPower, 0.5f).SetRelative());
        // コールバック処理
        OnDamageReceived?.Invoke();

        //if (_useRigidbody)
        //{
        //    // RigidBodyを使った実装
        //    _rb.AddForce(knockBackVec * _knockBackPower, ForceMode.Impulse);
        //}
        //else
        //{
        //    // DotWeenを使った実装
        //    transform.DOMove(knockBackVec * _knockBackPower, 0.5f).SetRelative();
        //}
    }

    /// <summary>吹き飛ばす方向を決める</summary>
    Vector3 GetAngleVec(Vector3 from, Vector3 to)
    {
        from.y = 0;
        to.y = 0;
        return Vector3.Normalize(to - from);
    }
}
