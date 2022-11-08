using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �^�[�Q�b�g��\���E�J�E���g����UI
/// </summary>
public class TargetView : MonoBehaviour
{
    [SerializeField] Text _counter;
    [SerializeField] Image _icon;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Set(int count, Sprite icon)
    {
        _counter.text = count.ToString();
        _icon.sprite = icon;
    }
}
