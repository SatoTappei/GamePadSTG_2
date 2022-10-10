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
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="value">ダメージ量</param>
    /// <param name="hitPos">ダメージを与えたオブジェクトと接触した座標</param>
    void OnDamage(int value, Vector3 hitPos);
}
