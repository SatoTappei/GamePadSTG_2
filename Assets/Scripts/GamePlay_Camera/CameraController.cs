using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// �v���C���[���f���J�����𐧌䂷��ėp�I�J�����R���g���[���[
/// Cinemachine��p�����A���C���J�����ɃA�^�b�`���Ďg��
/// </summary> 
// �g�p����ꍇ�́A��̃I�u�W�F�N�gParent�̎q�ɂ������̃I�u�W�F�N�gChild�����A
// ����ɂ��̎q�Ƀ��C���J������u���BTransfrom�̒l�̓��Z�b�g���Ă������ƁB
[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// �J�����̃p�����[�^�[
    /// ���̃N���X����J�����𐧌䂷��Ƃ��͂��̃N���X�̃C���X�^���X��p�ӂ���
    /// </summary>
    [Serializable]
    public class Parameter
    {
        public Transform _target;

        // CameraParent�Ɏg�p
        public Vector3 position;
        public Vector3 angles = new Vector3(10f, 0f, 0f);
        
        // CameraChild�Ɏg�p
        public float distance = 7f;

        // MainCamera�Ɏg�p
        public float fieldOfView = 45f;
        public Vector3 offsetPosition = new Vector3(0f, 1f, 0f);
        public Vector3 offsetAngles;
    }

    [SerializeField] Transform _parent;
    [SerializeField] Transform _child;
    [SerializeField] Camera _camera;
    [SerializeField] Parameter _parameter;

    /// <summary>�ݒ肵���p�����[�^�[���O������Q�Ƃ��邽�߂̃v���p�e�B</summary>
    public Parameter Param => _parameter;

    void Update()
    {
        /* �����ɔC�ӂ̃J����������@������ */
        float horiR = Input.GetAxis("Horizontal_R");
        float vertR = Input.GetAxis("Vertical_R");
        Vector3 camVec = new Vector3(vertR, horiR, 0) * 3;
        _parameter.angles += camVec;
    }

    void LateUpdate()
    {
        // ��ʑ̂̍X�V���ς񂾌�ɃJ�������X�V����K�v������̂�LateUpdate���g��
        if (_parent == null || _child == null || _camera == null)
            return;
        // ��ʑ̂��w�肳��Ă���ꍇ�́A�J�����̍��W���ʑ̂̍��W�ŏ㏑��
        if (_parameter._target != null)
            UpdateTargetBlend(_parameter);

        // �p�����[�^���e��I�u�W�F�N�g�ɔ��f
        _parent.position = _parameter.position;
        _parent.eulerAngles = _parameter.angles;

        var childPos = _child.localPosition;
        childPos.z = -1.0f * _parameter.distance;
        _child.localPosition = childPos;

        _camera.fieldOfView = _parameter.fieldOfView;
        _camera.transform.localPosition = _parameter.offsetPosition;
        _camera.transform.localEulerAngles = _parameter.offsetAngles;
    }

    // �����x��Ēǂ������Ă���J��������邽�߂ɐ��`�⊮�𗘗p����
    public static void UpdateTargetBlend(Parameter parameter)
    {
        Vector3 start = parameter.position;
        Vector3 end = parameter._target.position;
        float t = Time.deltaTime * 4.0f;
        parameter.position = Vector3.Lerp(start, end, t);
    }
}