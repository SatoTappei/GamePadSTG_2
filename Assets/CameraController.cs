using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[��w�ォ��Ǐ]����J����(���g�p)
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>�J�������Ǐ]����^�[�Q�b�g</summary>
    [SerializeField] Transform _target;
    /// <summary>�^�[�Q�b�g��Ǐ]����ۂ�y�ʒu�̃I�t�Z�b�g</summary>    
    [SerializeField] float _yOffset = 2.22f;
    /// <summary>�^�[�Q�b�g��Ǐ]����ۂ�z�ʒu�̃I�t�Z�b�g</summary>
    [SerializeField] float _zOffset = -3.33f;
    /// <summary>�^�[�Q�b�g��Ǐ]����ۂ̊p�x�I�t�Z�b�g</summary>
    [SerializeField] float _angleOffset = 2.0f;

    void Start()
    {

    }

    void Update()
    {

    }

    void LateUpdate()
    {
        // �^�[�Q�b�g�̑O�����̈ʒu�x�N�g������I�t�Z�b�g�����炵���ꏊ�Ɉړ�����
        transform.position = _target.position + (_target.forward * _zOffset) + (Vector3.up * _yOffset);
        // �^�[�Q�b�g����I�t�Z�b�g�����炵���ʒu���f��
        Vector3 lookCam = _target.position + Vector3.up * _angleOffset;
        transform.LookAt(lookCam);
    }
}
