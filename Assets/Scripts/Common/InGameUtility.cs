using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>キャラクターを識別するためのタグとして使う</summary>
public enum CharacterTag
{
    Player,
    BlueSoldier,
    Tank,
    BossTank,
}

/// <summary>
/// 様々なオブジェクトで使われる共通のデータをまとめた便利クラス
/// </summary>
public class InGameUtility
{
    /// <summary>ヒットストップする時間</summary>
    public static readonly float HitStopTime = 0.17f;
}
