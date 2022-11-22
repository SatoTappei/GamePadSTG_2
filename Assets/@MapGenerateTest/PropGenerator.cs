using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップ上に装飾を生成する
/// </summary>
public class PropGenerator
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>区域に小道具を生成する</summary>
    public void Generate(Area[,] props, Area[,] areas)
    {
        props[3, 3].GetSectionFromNumKey(5).Fill('v');
        // 現在のMassクラスには1つしか文字を格納するフィールドがない。
        // このメソッドは文字列の配列を生成しているため、Massクラスにもう一つ小道具を表すフィールドが必要になる
        // そのマスの建物フィールドを見てどの小道具を生成するのか決めるという手段がいる
    }
}
