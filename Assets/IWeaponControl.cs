using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// プレイヤーの武器のコライダーの制御をするためのインターフェース
/// EventSystemによるメッセージ送信を使う
/// </summary>
public interface IWeaponControl : IEventSystemHandler
{
    void EnableWeaponCollider();

    void DisableWeaponCollider();
}
