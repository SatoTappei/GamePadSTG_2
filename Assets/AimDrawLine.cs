using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʉ��̒[����G�C���J�[�\���Ɍ����ă��C��������
/// </summary>
public class AimDrawLine : MonoBehaviour
{
    /// <summary>���̖{�̂ł���摜��Width������������̂ŎQ�Ƃ������Ă���</summary>
    [SerializeField] RectTransform _line;
    /// <summary>���C���������^�[�Q�b�g</summary>
    [SerializeField] Transform _target;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 diff = _target.position - transform.position;
        Vector2 size = _line.sizeDelta;
        size.x = diff.magnitude;
        _line.sizeDelta = size;
        transform.up = diff.normalized;
    }
}
