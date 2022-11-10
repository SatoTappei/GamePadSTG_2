using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// このスクリプトが付いたオブジェクトのコライダーが対象とぶつかった場合
/// 対象についているDamageReceiverにダメージを与えたことを通知する
/// </summary>
public class DamageSender : MonoBehaviour
{
    /// <summary>ダメージの値</summary>
    int _damage;
    /// <summary>判定するタグ</summary>
    string _hitTag;

    /// <summary>ダメージを与えた時に行う追加の処理</summary>
    public UnityAction OnDamageSended;

    /// <summary>初期化処理、これを呼ぶまでダメージを与えることが出来ない</summary>
    public void Init(int damage, string tag)
    {
        _damage = damage;
        _hitTag = tag;
        Debug.Log("しょきか");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("よばれた");
        if (other.gameObject.CompareTag(_hitTag))
        {
            Debug.Log("たげっとに当たった");
            // コライダーがぶつかった位置を取得
            Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
            // ダメージを与えた時の処理を呼び出す
            ExecuteEvents.Execute<IDamageable>(other.gameObject, null, (reciever, _) => reciever.OnDamage(_damage, hitPos));
            OnDamageSended?.Invoke();
        }
    }
}
