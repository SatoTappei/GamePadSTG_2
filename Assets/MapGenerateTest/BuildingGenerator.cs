using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップ上に建物を生成する
/// </summary>
public class BuildingGenerator : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>建物を生成する</summary>
    //public void Generate(Area[,] _areas)
    //{
    //    // 道路沿いに建物を生成する
    //    for (int z = 0; z < 5; z++)
    //        for (int x = 0; x < 5; x++)
    //        {
    //            // その区域の空いている方向を調べる
    //            //_areas[z, x]._roadStrs[0, 0] = "b";
    //            // 道路沿いに建物を建てる、2か4が含まれているマスがnonだったら
    //            for (int i = 0; i < 7; i++)
    //                for (int j = 0; j < 7; j++)
    //                {
    //                    if (i == 2 || i == 4 || j == 2 || j == 4)
    //                    {
    //                        if (_areas[z, x]._roadStrs[i, j] == "n" &&
    //                           (_areas[z, x]._roadStrs[i + 1, j] == "r" ||
    //                            _areas[z, x]._roadStrs[i + 1, j] == "R" ||
    //                            _areas[z, x]._roadStrs[i - 1, j] == "r" ||
    //                            _areas[z, x]._roadStrs[i - 1, j] == "R" ||
    //                            _areas[z, x]._roadStrs[i, j + 1] == "r" ||
    //                            _areas[z, x]._roadStrs[i, j + 1] == "R" ||
    //                            _areas[z, x]._roadStrs[i, j - 1] == "r" ||
    //                            _areas[z, x]._roadStrs[i, j - 1] == "R" ))
    //                            _areas[z, x]._roadStrs[i, j] = "b";
    //                    }
    //                }

    //        }
    //}
}
