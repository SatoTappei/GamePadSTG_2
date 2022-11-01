using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// コライダーが付いたオブジェクトにアタッチするだけ
/// <summary>
/// このスクリプトが付いたオブジェクトのコライダーが対象とぶつかった場合
/// 対象についているDamageReceiverにダメージを与えたことを通知する
/// </summary>
public class DamageSender : MonoBehaviour
{
    /// <summary>ダメージの値</summary>
    [SerializeField] int _damage;
    /// <summary>判定するタグ</summary>
    [SerializeField] string _hitTag;

    /// <summary>ダメージを与えた時に行う追加の処理</summary>
    public UnityAction OnDamageSended;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_hitTag))
        {
            // コライダーがぶつかった位置を取得
            Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
            // ダメージを与えた時の処理を呼び出す
            ExecuteEvents.Execute<IDamageable>(other.gameObject, null, (reciever, _) => reciever.OnDamage(_damage, hitPos));
            OnDamageSended?.Invoke();
        }
    }
}
