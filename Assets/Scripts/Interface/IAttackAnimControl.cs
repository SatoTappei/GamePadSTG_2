using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 攻撃のアニメーションの再生時に呼ばれる処理が宣言されたインターフェース
/// EventSystemによるメッセージ送信を使う
/// </summary>
public interface IAttackAnimControl : IEventSystemHandler
{
    /// <summary>攻撃のアニメーション再生時に1回だけ呼ばれる</summary>
    void OnAnimEnter();
    /// <summary>攻撃のアニメーション終了時に1回だけ呼ばれる</summary>
    void OnAnimExit();
}
