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
        // 街灯を設置し終わった時点で草が生えている区画のリスト
        List<(Section, bool)> grassList = new List<(Section, bool)>();

        for (int i = 0; i < Map.Height; i++)
        {
            for (int j = 0; j < Map.Width; j++)
            {
                // 道路端に街灯を設置する際に使う配列
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
                bool isWide = areas[i, j].GetSectionFromNumKey(5).GetMass(0,0).Char == 'R';
                // 中央の区画から上下左右の区画を調べていく
                for (int k = 2; k <= 8; k += 2)
                {
                    // その方向に道路が伸びているか
                    char lead = areas[i, j].GetSectionFromNumKey(k).GetMass(0, 0).Char;
                    // 広い道路なら中央から1マス目は何も配置しない
                    if (isWide && lead == 'r')
                    {
                        if (k == 2 || k == 8)
                            props[i, j].GetSectionFromNumKey(k).SetCharArray(array31);
                        else
                            props[i, j].GetSectionFromNumKey(k).SetCharArray(array13);
                    }
                    // 普通の道路ならそのまま塗りつぶす
                    else if (lead == 'r')
                    {
                        props[i, j].GetSectionFromNumKey(k).Fill('l');
                    }
                    // そもそも草
                    else if (lead == 'g')
                    {
                        // 草が生えている区画のリストに追加する
                        // その区画がある区域の中央が広い道路になっているかのフラグとペアで追加する
                        grassList.Add((props[i, j].GetSectionFromNumKey(k), isWide));
                    }
                }
            }
        }

        // 草が生えている区画に壁を設置する
        foreach ((Section sec, bool isWide) pair in grassList)
        {
            // その区画が属する区域の中央が広い道路になっている場合は壁がはみ出さないようにする必要がある
            if (pair.isWide)
            {
                // 中央から1マスの位置の壁を消す
                switch (pair.sec.Number)
                {
                    case 2:
                        pair.sec.SetCharArray(new char[3, 1]
                        {
                            {'n'},
                            {'w'},
                            {'w'},
                        });
                        break;
                    case 4:
                        pair.sec.SetCharArray(new char[1, 3]
                        {
                            { 'i', 'i', 'n' },
                        });
                        break;
                    case 6:
                        pair.sec.SetCharArray(new char[1, 3]
                        {
                            { 'n', 'i', 'i' },
                        });
                        break;
                    case 8:
                        pair.sec.SetCharArray(new char[3, 1]
                        {
                            {'w'},
                            {'w'},
                            {'n'},
                        });
                        break;
                }
            }
            else
            {
                // 中央が道路の場合はそのまま塗りつぶす
                int num = pair.sec.Number;
                pair.sec.Fill(num == 2 || num == 8 ? 'w' : 'i');
            }
        }
    }
}
