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
    public void Generate(Area[,] _areas)
    {
        // 道路沿いに建物を生成する
        for (int z = 0; z < Map.Height; z++)
            for (int x = 0; x < Map.Width; x++)
            {
                char[,] build =
                {
                    {'n', 'n', 'n'},
                    {'n', 'b', 'n'},
                    {'n', 'n', 'n'},
                };

                _areas[z, x].GetSectionFromNumKey(1).SetCharArray(build);
                _areas[z, x].GetSectionFromNumKey(3).SetCharArray(build);
                _areas[z, x].GetSectionFromNumKey(7).SetCharArray(build);
                _areas[z, x].GetSectionFromNumKey(9).SetCharArray(build);
            }
    }
}
