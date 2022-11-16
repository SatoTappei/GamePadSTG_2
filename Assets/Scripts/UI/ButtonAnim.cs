using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 汎用的なボタンのアニメーション
/// </summary>
public class ButtonAnim : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        group.DOFade(0, 0.5f).SetEase(Ease.Flash, 7);
        group.interactable = false;
        // TODO:現状ボタンをクリックするとすぐ画面が切り替わるのでフェードを作る
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
