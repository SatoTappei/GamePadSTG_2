using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームクリアの演出を行う
/// </summary>
public class GameClearStag : MonoBehaviour
{
    [SerializeField] Text _time;

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
    public void Play(int time)
    {
        transform.localScale = Vector3.one;
        _time.text = Timer.Convert(time);
    }
}
