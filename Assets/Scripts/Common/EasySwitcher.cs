using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// �ȒP�ȃX�C�b�`�A�v�\��Ȃ̂ŏ����Ă����v
/// </summary>
public class EasySwitcher : MonoBehaviour
{
    [SerializeField] UnityEvent _event;
    [SerializeField] Transform _button;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            _event.Invoke();
            _button.DOMoveY(-1.5f, 0.25f);
        }
    }
}
