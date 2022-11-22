using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �Q�[���N���A�̉��o���s��
/// </summary>
public class GameClearStag : MonoBehaviour
{
    [SerializeField] Transform _clearText;
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

    /// <summary>���o���Đ�����</summary>
    public void Play(int time)
    {
        transform.localScale = Vector3.one;
        _time.text = Timer.Convert(time);
        _time.transform.localScale = Vector3.zero;

        DOTween.Sequence()
            .Append(_clearText.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.OutBounce))
            .Append(_time.transform.DOScale(Vector3.one,0.25f).SetEase(Ease.OutBounce).SetDelay(0.5f));
    }
}
