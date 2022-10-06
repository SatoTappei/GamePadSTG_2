using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ダメージを受ける処理が宣言されたインターフェース
/// EventSystemによるメッセージ送信を使う
/// </summary>
public interface IDamageable : IEventSystemHandler
{
    void OnDamage(int damageValue, Vector3 hitPos);
}
