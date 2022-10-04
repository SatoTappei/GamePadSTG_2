using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �J�����R���g���[���[�ɃA�C�e���ɒ�������@�\��t��������(���g�p)
/// </summary> 
// �g�p����ꍇ�́A��̃I�u�W�F�N�gParent�̎q�ɂ������̃I�u�W�F�N�gChild�����A
// ����ɂ��̎q�Ƀ��C���J������u���BTransfrom�̒l�̓��Z�b�g���Ă������ƁB
[ExecuteInEditMode]
public class LookItemCameraController : MonoBehaviour
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

        /// <summary>
        /// MemberwiseClone�֐���p���ĐV�����C���X�^���X���쐬����
        /// �e�����o�̒l���R�s�[����B�߂�l��object�^�Ȃ̂ŃL���X�g��Y�ꂸ��
        /// </summary>
        public Parameter Clone() => MemberwiseClone() as Parameter;

        /// <summary>�n���ꂽParameter��start��end��⊮�����l�������ĕԂ�</summary>
        public static Parameter Lerp(Parameter start, Parameter end, float t, Parameter ret)
        {
            ret.position = Vector3.Lerp(start.position, end.position, t);
            ret.angles = LerpAngles(start.angles, end.angles, t);
            ret.distance = Mathf.Lerp(start.distance, end.distance, t);
            ret.fieldOfView = Mathf.Lerp(start.fieldOfView, end.fieldOfView, t);
            ret.offsetPosition = Vector3.Lerp(start.offsetPosition, end.offsetPosition, t);
            ret.offsetAngles = LerpAngles(start.offsetAngles, end.offsetAngles, t);
            return ret;

            Vector3 LerpAngles(Vector3 start, Vector3 end, float t)
            {
                Vector3 ret = Vector3.zero;
                ret.x = Mathf.LerpAngle(start.x, end.y, t);
                ret.y = Mathf.LerpAngle(start.y, end.y, t);
                ret.z = Mathf.LerpAngle(start.x, end.y, t);
                return ret;
            }
        }
    }

    /// <summary>�J�����̃��[�h</summary>
    enum ModeType
    {
        Default,  // �v���C���[�ɒǏ]����
        LookItem, // �A�C�e�����Y�[������
    }

    [SerializeField] Transform _parent;
    [SerializeField] Transform _child;
    [SerializeField] Camera _camera;
    [SerializeField] Parameter _parameter;
    [SerializeField] Parameter _itemCamParam;

    ModeType _modeType;
    Parameter _defaultCamParam;
    // �A�j���[�V�������ɃL�����Z������Ă������悤�Ƀ����o�[�ϐ��Ƃ��ĕێ����Ă���
    Sequence _cameraSeq;

    /// <summary>�ݒ肵���p�����[�^�[���O������Q�Ƃ��邽�߂̃v���p�e�B</summary>
    public Parameter Param => _parameter;

    void Awake()
    {
        _defaultCamParam = _parameter.Clone();
    }

    void Update()
    {
        /* �����ɔC�ӂ̃J����������@������ */
        if (_modeType == ModeType.Default && 
            (_cameraSeq == null || !(_cameraSeq.IsActive() && _cameraSeq.IsPlaying())))
        {
            float horiR = Input.GetAxis("Horizontal_R");
            float vertR = Input.GetAxis("Vertical_R");
            Vector3 camVec = new Vector3(vertR, horiR, 0) * 3;
            _parameter.angles += camVec;
        }
        /* �C�ӂ̃J����������@�����܂� */

        /* �J�������[�h�̐؂�ւ������̃e�X�g */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_modeType)
            {
                case ModeType.Default:
                    SwitchCamera(ModeType.LookItem);
                    break;
                case ModeType.LookItem:
                    SwitchCamera(ModeType.Default);
                    break;
            }
        }
        /* �e�X�g�����܂� */
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

    /// <summary>���[�h�ɑΉ������p�����[�^�[��Ԃ�</summary>
    Parameter GetCameraParameter(ModeType type)
    {
        switch (type)
        {
            case ModeType.Default:
                return _defaultCamParam;
            case ModeType.LookItem:
                return _itemCamParam;
            default:
                return null;
        }
    }

    /// <summary>�J�����̐؂�ւ�����</summary>
    void SwitchCamera(ModeType type)
    {
        // �J�������[�h���X�V
        _modeType = type;
        // �}�E�X�𓮂����Ɖ�ʂ��Ԃ��̂ŁA�\�߃^�[�Q�b�g��null�ɐݒ�
        _parameter._target = null;

        Parameter startCamParam = _parameter.Clone();
        Parameter endCamParam = GetCameraParameter(_modeType);

        // �J�����̈ړ�����
        float duration = 2.0f;
        // �J�����̈ړ�
        _cameraSeq?.Kill();
        _cameraSeq = DOTween.Sequence();
        // ���`�⊮��p���Ĉړ�������
        _cameraSeq.Append(DOTween.To(() => 0f, t => Parameter.Lerp(startCamParam, endCamParam, t, _parameter), 1f, duration))
            .SetEase(Ease.OutQuart);
        // �A�j���[�V�������̃u�����h�����s
        _cameraSeq.OnUpdate(() => UpdateTargetBlend(_defaultCamParam));
        // ��̃V�[�P���X�I����̃R�[���o�b�N�ŁAtarget��ݒ�
        _cameraSeq.AppendCallback(() => _parameter._target = endCamParam._target);
    }
}