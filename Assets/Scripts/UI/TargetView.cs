using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �^�[�Q�b�g�̎c����\���E�J�E���g����UI
/// </summary>
public class TargetView : MonoBehaviour
{
    /// <summary>�G�̎c����\������J�E���^�[�e�L�X�g</summary>
    [SerializeField] Text _counter;
    /// <summary>�G�̃A�C�R����\������A�C�R���摜</summary>
    [SerializeField] Image _icon;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>������</summary>
    public void Init(int count, Sprite icon)
    {
        _counter.text = count.ToString();
        _icon.sprite = icon;
    }

    /// <summary>�J�E���^�[�̒l���Z�b�g����</summary>
    public void SetValue(int value) => _counter.text = value.ToString();
}
