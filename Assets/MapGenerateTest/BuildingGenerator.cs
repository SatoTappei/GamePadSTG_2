using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ƒ}ƒbƒvã‚ÉŒš•¨‚ğ¶¬‚·‚é
/// </summary>
public class BuildingGenerator : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>Œš•¨‚ğ¶¬‚·‚é</summary>
    public void Generate(Area[,] _areas)
    {
        // “¹˜H‰ˆ‚¢‚ÉŒš•¨‚ğ¶¬‚·‚é
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
