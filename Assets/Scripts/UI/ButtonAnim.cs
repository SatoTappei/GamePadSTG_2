using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// �ėp�I�ȃ{�^���̃A�j���[�V����
/// </summary>
public class ButtonAnim : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        group.DOFade(0, 0.5f).SetEase(Ease.Flash, 7);
        group.interactable = false;
        // TODO:����{�^�����N���b�N����Ƃ�����ʂ��؂�ւ��̂Ńt�F�[�h�����
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
