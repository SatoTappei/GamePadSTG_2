using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

// コライダーが付いたオブジェクトにアタッチするだけ
// プレイヤーと敵で共通の演出はこっちに書いてある
/// <summary>
/// DamageSenderからダメージを受けたことを受信する
/// </summary>
public class DamageReceiver : MonoBehaviour, IDamageable
{
    /// <summary>ノックバック力</summary>
    [SerializeField] float _knockBackPower = 3.0f;
    /// <summary>無敵時間</summary>
    [SerializeField] float _invincibleTime = 2.0f;
    /// <summary>ダメージを受けた際のエフェクトのプレハブ</summary>
    [SerializeField] GameObject _damageEffectPrefab;
    /// <summary>最大HP</summary>
    [SerializeField] int _MaxHP; // 現在はインスペクタから設定するがいずれは別のデータ箇所から設定するようにすることを考慮する
    /// <summary>表示非表示を切り替えて使いまわすダメージエフェクト</summary>
    GameObject _damageEffect;
    /// <summary>無敵時間中かどうかのフラグ</summary>
    bool _isInvincible;
    /// <summary>現在のHP</summary>
    int _currentHp;
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

        _currentHp = _MaxHP;
    }

    /// <summary>攻撃を受けたときの処理</summary>
    public void OnDamage(int damageValue, Vector3 hitPos)
    {
        // 無敵時間中は攻撃を受けない
        if (_isInvincible) return;
        // 無敵時間を設定する
        Sequence sequence = DOTween.Sequence()
            .AppendInterval(_invincibleTime)
            .OnStart(() => _isInvincible = true)
            .OnComplete(() => _isInvincible = false);

        // 衝突位置にパーティクルを発生させる
        if (_damageEffect != null) 
            EnabledDamageEffect(hitPos);

        // ヒットストップの後、ノックバックさせる
        DOVirtual.DelayedCall(ConstValue.HitStopTime, () => KnockBack(hitPos, _knockBackPower));

        // ダメージ処理
        _currentHp -= damageValue;
        if(_currentHp <= 0)
        {
            // 死亡演出、こっちからEnemyManager側に通知することをしたくないので
            // こっちの状態を監視させておいてこのオブジェクトが消えるなり死亡アニメーションするなり
            // したときにカウントを減らすようにする
        }

        OnDamageReceived?.Invoke();
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
