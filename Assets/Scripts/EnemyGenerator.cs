using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// マップ上に敵を配置する
/// </summary>
public class EnemyGenerator
{
    readonly int MaxEnemy = 10;

    /// <summary>敵を生成する</summary>
    public void Generate(Area[,] enemies, Area[,] areas)
    {
        // 上限数まで敵を生成する
        // 敵が生成されるのは5,2,4,6,8
        // 戦車はでかい道路にのみ生成される
        // 兵士はどこにでも生成される

        // 幅の広い道路とそうでない道路に分ける
        List<Section> wideRoadList = new List<Section>();
        List<Section> roadList = new List<Section>();

        for (int i = 0; i < Map.Height; i++)
        {
            for (int j = 0; j < Map.Width; j++)
            {
                if(areas[i, j].GetSectionFromNumKey(5).GetMass(0, 0).Char == 'R')
                {
                    wideRoadList.Add(enemies[i, j].GetSectionFromNumKey(5));
                }
                else
                {
                    roadList.Add(enemies[i, j].GetSectionFromNumKey(5));
                }

                for (int k = 2; k <= 8; k += 2)
                {
                    if (areas[i, j].GetSectionFromNumKey(k).GetMass(0, 0).Char == 'R')
                    {
                        wideRoadList.Add(enemies[i, j].GetSectionFromNumKey(k));
                    }
                    else if(areas[i, j].GetSectionFromNumKey(k).GetMass(0, 0).Char == 'r')
                    {
                        roadList.Add(enemies[i, j].GetSectionFromNumKey(k));
                    }
                }

            }
        }

        Debug.Log("太い道路" + wideRoadList.Count);
        Debug.Log("道路" + roadList.Count);

        // 太い道路のリストの中からランダムな区画、かつランダムなマスに生成する
        // 1体生成した区域には生成しない
        // 生成する敵の最大数は定数で指定してある。
        // 戦車と兵士は同じ量生成する S H
        // ゲームクリアのためにターゲットも1体生成する T

        wideRoadList = wideRoadList.OrderBy(_ => System.Guid.NewGuid()).ToList();
        roadList = roadList.OrderBy(_ => System.Guid.NewGuid()).ToList();

        // 戦車を生成する
        for (int i = 0; i < MaxEnemy / 2; i++)
        {
            if (wideRoadList[i].Number == 5)
            {
                wideRoadList[i].Fill('S');
            }
            else if(wideRoadList[i].Number == 2 || wideRoadList[i].Number == 8)
            {
                char[,] array =
                {
                    {'n'},
                    {'S'},
                    {'n'},
                };

                wideRoadList[i].SetCharArray(array);
            }
            else if(wideRoadList[i].Number == 4 || wideRoadList[i].Number == 6)
            {
                char[,] array =
                {
                    {'n', 'S', 'n'},
                };

                wideRoadList[i].SetCharArray(array);
            }


        }

        //// 兵士を生成する
        //for (int i = 0; i < MaxEnemy / 2; i++)
        //{
        //    if (wideRoadList[i].Number == 5)
        //    {
        //        wideRoadList[i].Fill('H');
        //    }
        //    else if (wideRoadList[i].Number == 2 || wideRoadList[i].Number == 8)
        //    {
        //        char[,] array =
        //        {
        //            {'n'},
        //            {'H'},
        //            {'n'},
        //        };

        //        wideRoadList[i].SetCharArray(array);
        //    }
        //    else if (wideRoadList[i].Number == 4 || wideRoadList[i].Number == 6)
        //    {
        //        char[,] array =
        //        {
        //            {'n', 'H', 'n'},
        //        };

        //        wideRoadList[i].SetCharArray(array);
        //    }
        //}
    }
}
