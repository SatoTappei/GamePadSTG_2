using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// カウントが0になったときにコールバックが呼ばれるタイマー
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField] Text _counter;
    /// <summary>制限時間(分)</summary>
    [SerializeField] int _limitMinutes;

    /// <summary>タイムアップ時に呼ばれるイベント</summary>
    public UnityAction TimeUpEvent { get; set; }
    /// <summary>trueの間はタイマーが止まる</summary>
    bool _isPause = true;

    float _count;

    void Awake()
    {
        _count = _limitMinutes * 60;
        ToText();
    }

    void Start()
    {

    }

    void Update()
    {
        // デバッグ用
        //if (Input.GetKeyDown(KeyCode.T)) IsPause = !IsPause;
     
        if (_isPause) return;

        _count -= Time.deltaTime;
        if (ToText() == 0)
        {
            TimeUpEvent?.Invoke();
            TimeUpEvent = null;
            _isPause = true;
        }
    }

    /// <summary>タイマーのカウントを開始する、二回目以降に呼び出した場合は再開になる</summary>
    public void TimerStart() => _isPause = false;
    
    /// <summary>タイマーのカウントを止める</summary>
    public void TimerPause() => _isPause = true;

    /// <summary>現在のタイマーの経過時間を返す</summary>
    public int GetCount() => _limitMinutes * 60 - (int)_count;

    /// <summary>タイマー形式の文字列にして返す</summary>
    public static string Convert(int count)
    {
        TimeSpan ts = new TimeSpan(0, 0, count);
        return ts.ToString(@"mm\:ss");
    }

    /// <summary>分:秒の形でテキストに表示してint型にキャストしたCountを返す</summary>
    int ToText()
    {
        int iCount = (int)_count;
        _counter.text = Convert(iCount);
        return iCount;
    }
}
