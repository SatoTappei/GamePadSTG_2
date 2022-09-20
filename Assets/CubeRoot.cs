using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キューブの親
/// </summary>
public class CubeRoot : MonoBehaviour
{
    /// <summary>紅色のキューブ</summary>
    [SerializeField] GameObject _red;
    /// <summary>蒼色のキューブ</summary>
    [SerializeField] GameObject _blue;

    void Awake()
    {
        // 生成されたら3*3*3の27個のキューブを生成して位置を設定して子に設定する
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
