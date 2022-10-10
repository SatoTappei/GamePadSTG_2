using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 武器のコライダーのオンオフを切り替えるインターフェース
/// EventSystemによるメッセージ送信を使う
/// </summary>
public interface IWeaponControl : IEventSystemHandler
{
    /// <summary>攻撃のアニメーション時にコライダーをオンにする</summary>
    void EnableCollider();
    /// <summary>攻撃のアニメーション終了時にコライダーをオフにする</summary>
    void DisableCollider();
}
