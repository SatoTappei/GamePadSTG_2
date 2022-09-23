using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʉ��̍��E�̒[����G�C���Ɍ������Đ�������
/// </summary>
public class AimDrawLine : MonoBehaviour
{
    /// <summary>���̒�����ύX����̂�width��ύX����̂�RectTransform�^���K�v</summary>
    [SerializeField] RectTransform _line;
    /// <summary>����������^�[�Q�b�g</summary>
    [SerializeField] Transform _target;

    void Start()
    {

    }

    void Update()
    {
        Vector3 diff = _target.position - transform.position;
        // RectTransform��Width��Height���i�[����Ă���̂�sizeDelta�v���p�e�B
        Vector2 size = _line.sizeDelta;
        // Width���^�[�Q�b�g�Ƃ̋����Ɠ����ɂ���
        size.x = diff.magnitude;
        _line.sizeDelta = size;
        // �����^�[�Q�b�g�Ɍ�����
        transform.up = diff.normalized;
    }
}
