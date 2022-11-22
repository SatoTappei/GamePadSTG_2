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
        for (int i = 0; i < Map.Height; i++)
        {
            for (int j = 0; j < Map.Width; j++)
            {
                // 道路端に街灯を設置する
                
                char[,] array31 =
                {
                    {'n'},
                    {'l'},
                    {'l'},
                };
                char[,] array13 =
                {
                    {'n', 'l', 'l'},
                };

                // 区域の中央が広い道路かどうか
                bool isWide = areas[i, j].GetSectionFromNumKey(5).GetCharArray()[0, 0] == 'R';
                // 中央の区画から上下左右の区画を調べていく
                for (int k = 2; k <= 8; k += 2)
                {
                    // その方向に道路が伸びているか
                    bool isRoad = areas[i, j].GetSectionFromNumKey(k).GetCharArray()[0, 0] == 'r';
                    // 広い道路なら中央から1マス目は何も配置しない
                    if (isRoad && isWide)
                    {
                        if (k == 2 || k == 8)
                            props[i, j].GetSectionFromNumKey(k).SetCharArray(array31);
                        else
                            props[i, j].GetSectionFromNumKey(k).SetCharArray(array13);
                    }
                    // 普通の道路ならそのまま塗りつぶす
                    else if (isRoad)
                    {
                        props[i, j].GetSectionFromNumKey(k).Fill('l');
                    }
                    // そもそも道路じゃない場合は何も設置しない
                }
            }
        }
        // 現在のMassクラスには1つしか文字を格納するフィールドがない。
        // このメソッドは文字列の配列を生成しているため、Massクラスにもう一つ小道具を表すフィールドが必要になる
        // そのマスの建物フィールドを見てどの小道具を生成するのか決めるという手段がいる
    }
}
