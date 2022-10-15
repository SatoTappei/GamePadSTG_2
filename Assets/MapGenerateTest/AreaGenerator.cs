using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1つ1つの区域を生成する
/// </summary>
public class AreaGenerator : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>7*7の区域を生成する</summary>
    public string[,] Generate()
    {
        string[,] area = new string[7, 7];

        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 7; j++)
                area[i, j] = "g";

        area[3, 3] = "r";

        // 真ん中は必ず道路になる
        // 上下左右にランダムに道を伸ばす

        return area;
    }
}
