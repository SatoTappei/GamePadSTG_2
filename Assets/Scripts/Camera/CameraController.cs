using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

// �g�p����ꍇ�́A��̃I�u�W�F�N�gParent�̎q�ɂ������̃I�u�W�F�N�gChild�����A
// ����ɂ��̎q�Ƀ��C���J������u���BTransfrom�̒l�̓��Z�b�g���Ă������ƁB
/// <summary>
/// �v���C���[���f���J�����𐧌䂷��ėp�I�J�����R���g���[���[
/// Cinemachine��p�����ACameraParent�ɃA�^�b�`���Ďg��
/// </summary> 
//[ExecuteInEditMode]
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

        /// <summary>���݂�Parameter�𕡐����ĕԂ�</summary>
        public Parameter Clone()
        {
            return MemberwiseClone() as Parameter;
        }

        public static Parameter Lerp(Parameter a, Parameter b, float t, Parameter ret)
        {
            ret.position = Vector3.Lerp(a.position, b.position, t);
            ret.angles = LerpAngles(a.angles, b.angles, t);
            ret.distance = Mathf.Lerp(a.distance, b.distance, t);
            ret.fieldOfView = Mathf.Lerp(a.fieldOfView, b.fieldOfView, t);
            ret.offsetPosition = Vector3.Lerp(a.offsetPosition, b.offsetPosition, t);
            ret.offsetAngles = LerpAngles(a.offsetAngles, b.offsetAngles, t);
            return ret;
        }

        public static Vector3 LerpAngles(Vector3 a, Vector3 b, float t)
        {
            Vector3 ret = Vector3.zero;
            ret.x = Mathf.LerpAngle(a.x, b.x, t);
            ret.y = Mathf.LerpAngle(a.y, b.y, t);
            ret.z = Mathf.LerpAngle(a.z, b.y, t);
            return ret;
        }
    }

    public enum CameraMode
    {
        Default,
        ItemLook,
    }
    public CameraMode Mode { get; set; }

    [SerializeField] Transform _parent;
    [SerializeField] Transform _child;
    [SerializeField] Camera _camera;
    [SerializeField] Parameter _parameter;

    Sequence _cameraSeq;

    /// <summary>�J�����̒Ǐ]���ꎞ��~������</summary>
    public bool IsPause;

    /// <summary>
    /// �J������U��������p�̃x�N�g��
    /// �Q�Ɩ�����Shake���Ăׂ�悤�ɂ��邽��static�ɂ��Ă��邪�s�s�����������璼��
    /// </summary>
    static Vector3 _shakeAngles;

    /// <summary>�ݒ肵���p�����[�^�[���O������Q�Ƃ��邽�߂̃v���p�e�B</summary>
    public Parameter Param => _parameter;

    void Start()
    {
        if (_parameter._target != null)
        {
            _parameter.position = _parameter._target.position;
        }
    }

    void Update()
    {
        if (IsPause) return;

        /* �����ɔC�ӂ̃J����������@������ */
        float horiR = Input.GetAxis("Horizontal_R");
        float vertR = Input.GetAxis("Vertical_R");
        Vector3 camVec = new Vector3(vertR, horiR, 0) * 3;
        /* �C�ӂ̃J����������@�����܂� */

        _parameter.angles += camVec;
    }

    void LateUpdate()
    {
        if (IsPause) return;

        // ��ʑ̂̍X�V���ς񂾌�ɃJ�������X�V����K�v������̂�LateUpdate���g��
        if (_parent == null || _child == null || _camera == null)
            return;
        // ��ʑ̂��w�肳��Ă���ꍇ�́A�J�����̍��W���ʑ̂̍��W�ŏ㏑��
        if (_parameter._target != null)
            UpdateTargetBlend(_parameter);

        SetParamToObject();
    }

    /// <summary>�p�����[�^���e��I�u�W�F�N�g�ɔ��f</summary>
    void SetParamToObject()
    {
        _parent.position = _parameter.position;
        _parent.eulerAngles = _parameter.angles;

        var childPos = _child.localPosition;
        childPos.z = -1.0f * _parameter.distance;
        _child.localPosition = childPos;

        _camera.fieldOfView = _parameter.fieldOfView;
        _camera.transform.localPosition = _parameter.offsetPosition;
        _camera.transform.localEulerAngles = _parameter.offsetAngles + _shakeAngles;
    }

    /// <summary>�J�����̃��[�h��؂�ւ���</summary>
    public void SwitchCamera(Parameter to)
    {
        Param._target = null;
        _parameter = to;
    }

    /// <summary>�����x��Ēǂ������Ă���J��������邽�߂ɐ��`�⊮�𗘗p����</summary>
    public static void UpdateTargetBlend(Parameter parameter)
    {
        Vector3 start = parameter.position;
        Vector3 end = parameter._target.position;
        float t = Time.deltaTime * 4.0f;
        parameter.position = Vector3.Lerp(start, end, t);
    }

    /// <summary>�J������U��������</summary>
    public static void Shake(float duration,Vector3 strength,int vibratio)
    {
        DOTween.Shake(
            () => _shakeAngles,             // �J�n���̒l
            shake => _shakeAngles = shake,  // �p�����[�^�̍X�V
            duration,                       // ��������
            strength,                       // �h��̋���
            vibratio)                       // �ǂ̂��炢�U�����邩
            .OnComplete(() => _shakeAngles = Vector3.zero);
    }
}