using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ターゲットの残数を表示・カウントするUI
/// </summary>
public class TargetView : MonoBehaviour
{
    /// <summary>敵の残数を表示するカウンターテキスト</summary>
    [SerializeField] Text _counter;
    /// <summary>敵のアイコンを表示するアイコン画像</summary>
    [SerializeField] Image _icon;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>初期化</summary>
    public void Init(int count, Sprite icon)
    {
        _counter.text = count.ToString();
        _icon.sprite = icon;
    }

    /// <summary>カウンターの値をセットする</summary>
    public void SetValue(int value) => _counter.text = value.ToString();
}
