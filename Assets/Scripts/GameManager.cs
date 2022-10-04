using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // フレームレートを60fpsに固定する
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
