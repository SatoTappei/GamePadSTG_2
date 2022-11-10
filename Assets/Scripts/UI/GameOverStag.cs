using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームオーバーの演出を行う
/// </summary>
public class GameOverStag : MonoBehaviour
{
    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>演出を再生する</summary>
    public void Play()
    {
        // TODO:演出を作る
        transform.localScale = Vector3.one;
    }
}
