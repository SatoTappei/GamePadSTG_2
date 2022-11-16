using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System;

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
    /// <summary>死亡時に生成する演出のプレファブ</summary>
    [SerializeField] GameObject _defeatedEffectPrefab;
    /// <summary>表示非表示を切り替えて使いまわすダメージエフェクト</summary>
    GameObject _damageEffect;
    /// <summary>現在の体力</summary>
    ReactiveProperty<int> _currentHP = new ReactiveProperty<int>(0);
    /// <summary>無敵時間中かどうかのフラグ</summary>
    bool _isInvincible = true;
    /// <summary>ダメージを受けたときに行う追加の処理</summary>
    public UnityAction OnDamageReceived;
    /// <summary>死亡時に行う追加の処理</summary>
    public UnityAction OnDeath;

    public ReactiveProperty<int> CurrentHP { get => _currentHP; }

    void Awake()
    {
        // 使いまわせるようにダメージエフェクトをインスタンス化して非アクティブにしておく
        if (_damageEffectPrefab != null)
        {
            _damageEffect = Instantiate(_damageEffectPrefab);
            _damageEffect.SetActive(false);
        }
    }

    /// <summary>初期化処理、これが呼ばれるまで敵が無敵状態のまま</summary>
    public void Init(int maxHP)
    {
        _currentHP.Value = maxHP;
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
        DOVirtual.DelayedCall(InGameUtility.HitStopTime, () => CalcKnockBack(hitPos, _knockBackPower));

        // ダメージを反映させる
        _currentHP.Value -= damage;
        if(_currentHP.Value < 0)
        {
            Instantiate(_defeatedEffectPrefab, transform.position, Quaternion.identity);
            OnDeath?.Invoke();
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
    void CalcKnockBack(Vector3 hit, float power)
    {
        // ノックバックさせる方向を求める
        Vector3 pos = transform.position;
        pos.y = 0;
        hit.y = 0;
        Vector3 dir = Vector3.Normalize(pos - hit);

        // ノックバック
        Knockback(dir * power, 0.1f);
    }

    /// <summary>
    /// ノックバック演出
    /// </summary>
    /// <param name="knockbackVector">ノックバックする方向と距離を表すベクトル</param>
    /// <param name="speed">ノックバックスピード。1mあたり何秒で移動するか？の単位</param>
    private void Knockback(Vector3 knockbackVector, float speed)
    {
        // コライダーの半径を考慮して埋まらないようにする
        float colR = 0.5f;

        // ベクトルの長さを計算して、ノックバック距離を取得
        // Vector型の"magnitude"は、ベクトルの長さを計算して返す変数
        float knockbackDistance = knockbackVector.magnitude;

        // 移動前にノックバック方向に例を飛ばして、"Field"レイヤーのコライダーにヒットするかどうかチェック
        // "maxDistance"引数にはノックバック距離を渡して、その距離より離れた位置のコライダーは無視する
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, knockbackVector, out hitInfo, knockbackDistance, LayerMask.GetMask("Ground")))
        {
            // "Field"レイヤーのコライダーにヒットしていたら、ノックバックベクトルをヒットした位置までの距離に縮める

            // 現在位置からレイがぶつかった位置までの距離を計算
            knockbackDistance = Vector3.Distance(transform.position, hitInfo.point);

            // ノックバックベクトルをヒットした位置までの距離に縮める
            // Vector型の"normalized"は、ベクトルの長さを1にして返す変数
            // それに距離を乗算することで、ベクトルの方向を維持して長さを買えている
            knockbackVector = knockbackVector.normalized * (knockbackDistance - colR);
        }

        // ノックバック距離に合わせて、アニメーション時間を調整する
        float duration = knockbackDistance * speed;

        // DOMoveを使ってノックバック移動を実施
        transform.DOMove(transform.position + knockbackVector, duration).SetEase(Ease.OutCubic);
    }
}