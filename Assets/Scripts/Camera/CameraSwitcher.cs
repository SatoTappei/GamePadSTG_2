using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �R���C�_�[�ŃJ�����̐؂�ւ����s��(��)
/// </summary>
public class CameraSwitcher : MonoBehaviour
{
    CameraController _cameraCtr;
    CameraController.Parameter _prevParam;
    [SerializeField] CameraController.Parameter _param;

    void Start()
    {
        _cameraCtr = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _prevParam = _cameraCtr.Param.Clone();
            // �J�������[�h��LookItem�ɂ��ČŒ�ɂ���
            _cameraCtr.SwitchCamera(_param);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // �J�������[�h��Default�ɂ��ăv���C���[�ɒǏ]������
            _cameraCtr.SwitchCamera(_prevParam);
        }
    }
}
