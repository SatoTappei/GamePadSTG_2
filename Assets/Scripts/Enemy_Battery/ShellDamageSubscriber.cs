using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DamageSenderに砲弾がダメージを与えたときの処理を登録する
/// </summary>
public class ShellDamageSubscriber : MonoBehaviour
{
    [SerializeField] DamageSender _damageSender;

    void OnEnable()
    {
        _damageSender.OnDamageSended += OnDamageSended;
    }

    void OnDisable()
    {
        _damageSender.OnDamageSended -= OnDamageSended;
    }

    void OnDamageSended()
    {
        // オブジェクトプールに戻す
        gameObject.SetActive(false);
    }
}
