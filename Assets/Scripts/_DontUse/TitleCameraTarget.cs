using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ���ݖ��g�p
/// �^�C�g���̃J�������Ǐ]����I�u�W�F�N�g�̉����ړ���������R���|�[�l���g
/// </summary>
public class TitleCameraTarget : MonoBehaviour
{
    // �����̃|�C���g�ƂȂ�2�_
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _endPos;
    // ��������
    [SerializeField] float _duration;

    Sequence seq;

    void Start()
    {
        seq = DOTween.Sequence();
        seq.Append(transform.DOMove(_endPos.position, _duration));
        seq.Append(transform.DOMove(_startPos.position, _duration));
        seq.SetLoops(-1);
    }

    void Update()
    {
        
    }

    void OnDisable()
    {
        seq.Kill();
    }
}
