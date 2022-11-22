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
                // 区域の中央が広い道路か否かで分岐する
                bool isWide = areas[i, j].GetSectionFromNumKey(5).GetCharArray()[0, 0] == 'R';
                
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

                for (int k = 2; k <= 8; k += 2)
                {
                    if (areas[i, j].GetSectionFromNumKey(k).GetCharArray()[0, 0] == 'r')
                    {
                        if (isWide)
                        {
                            if (k == 2 || k == 8)
                            {
                                props[i, j].GetSectionFromNumKey(k).SetCharArray(array31);
                            }
                            else
                            {
                                props[i, j].GetSectionFromNumKey(k).SetCharArray(array13);
                            }
                        }
                        else
                        {
                            props[i, j].GetSectionFromNumKey(k).Fill('l');
                        }
                    }
                }
            }
        }
        // 現在のMassクラスには1つしか文字を格納するフィールドがない。
        // このメソッドは文字列の配列を生成しているため、Massクラスにもう一つ小道具を表すフィールドが必要になる
        // そのマスの建物フィールドを見てどの小道具を生成するのか決めるという手段がいる
    }
}
