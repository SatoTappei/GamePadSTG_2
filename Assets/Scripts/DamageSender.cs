using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// このスクリプトが付いたオブジェクトのコライダーが対象とぶつかった場合
/// 対象についているDamageReceiverにダメージを与えたことを通知する
/// </summary>
public class DamageSender : MonoBehaviour
{
    /// <summary>ダメージの値</summary>
    [SerializeField] int _damage;
    /// <summary>ダメージを与える敵として認識するタグ</summary>
    [SerializeField] string _effectiveTag;

    public event Action OnDamageSended;

    void OnTriggerEnter(Collider other)
    {
        // 敵にヒットしたかどうか
        if (other.gameObject.CompareTag(_effectiveTag))
        {
            // コライダーがぶつかった位置を取得
            Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
            // ダメージ処理を呼び出す
            ExecuteEvents.Execute<IDamageable>(other.gameObject, null, (reciever, eventData) => reciever.OnDamage(_damage, hitPos));
            // コールバック処理
            OnDamageSended?.Invoke();
        }
    }
}
