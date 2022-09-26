using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[��w�ォ��Ǐ]����J����
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>�J�������Ǐ]����^�[�Q�b�g</summary>
    [SerializeField] Transform _target;
    /// <summary>�^�[�Q�b�g��Ǐ]����ۂ̈ʒu�I�t�Z�b�g</summary>
    //[SerializeField] Vector3 _posOffset;
    /// <summary>�^�[�Q�b�g��Ǐ]����ۂ�y�ʒu�̃I�t�Z�b�g</summary>    
    [SerializeField] float _yOffset;
    /// <summary>�^�[�Q�b�g��Ǐ]����ۂ�z�ʒu�̃I�t�Z�b�g</summary>
    [SerializeField] float _zOffset;
    /// <summary>�^�[�Q�b�g��Ǐ]����ۂ̊p�x�I�t�Z�b�g</summary>
    [SerializeField] float _angleOffset;
    /// <summary>�J�������Ǐ]����^�[�Q�b�g����~���Ă���Ƃ��̊p�x</summary>
    //Quaternion _targetStopRot;
    /// <summary>�J�������Ǐ]����^�[�Q�b�g���ړ����Ă���Ƃ��̊p�x</summary>
    //Quaternion _targetMoveRot;

    void Start()
    {
        
    }

    void Update()
    {
        //float hori = Input.GetAxis("Horizontal");
        //float vert = Input.GetAxis("Vertical");
        //Vector3 inputVec = new Vector3(hori, 0, vert);

        //_targetMoveRot = _target.transform.rotation;
        //transform.rotation = Quaternion.Euler(0, _targetMoveRot.eulerAngles.y, 0);

        //if (inputVec != Vector3.zero)
        //{
        //    _targetMoveRot = _target.transform.rotation;
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0, _targetMoveRot.eulerAngles.y, 0);
        //    _targetStopRot = _target.transform.rotation;
        //}
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
