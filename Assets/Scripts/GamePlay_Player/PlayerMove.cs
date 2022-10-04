using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// �v���C���[�̈ړ����s��
/// ��ɃJ�����̌����Ă�������𐳖ʂƂ��đ��삷�邱�Ƃ��o����B
/// </summary>
public class PlayerMove : MonoBehaviour
{
    Rigidbody _rb;
    Animator _anim;
    Camera _mainCamera;

    [Header("�ړ��p")]
    /// <summary>�ړ����x</summary>
    [SerializeField] float _velocity;
    /// <summary>�U��������x</summary>
    [SerializeField] float _turnSpeed;
    /// <summary>
    /// �ړ��̃A�j���[�V�����𐧌䂷��p�����[�^�[��
    /// �e���[�V�����̊�ƂȂ�l ��~:0 ����:1 �_�b�V��:2
    /// </summary>
    [SerializeField] string _animParameterName;

    /// <summary>�ړ����x�̔{���A���݂̓_�b�V���p</summary>
    FloatReactiveProperty _mag = new FloatReactiveProperty();
    /// <summary>���X�e�B�b�N������͂��ꂽ�ړ��x�N�g��</summary>
    Vector3ReactiveProperty _inputVec = new Vector3ReactiveProperty();

    [Header("�W�����v�p")]
    /// <summary>�W�����v�\���𔻒肷��Ray���΂���ƂȂ���W</summary>
    [SerializeField] Transform _foot;
    /// <summary>�W�����v�\���𔻒肷��Ray���q�b�g���郌�C���[</summary>
    [SerializeField] LayerMask _mask;
    /// <summary>�W�����v�̓��͂����������ǂ���</summary>
    bool _isJump;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();

        // TODO:�_�b�V�����̉���A�p�[�e�B�N�����o�����肷��̂�o�^����
        //_mag.Where(_ => _inputVec.Value != Vector3.zero).Subscribe(f => _anim.SetFloat("Speed", f));
        //_inputVec.Where(v => v == Vector3.zero).Subscribe(_ => _anim.SetFloat("Speed",0));
        //_inputVec.Where(v => v != Vector3.zero).Subscribe(_ => _anim.SetFloat("Speed", 1));
    }

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // �ړ��̓��͂��󂯎��
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        _mag.Value = Input.GetButton("Dash") ? 2 : 1;

        // �v���C���[�̏�����̃x�N�g�������ɃJ������Y�����̉�]�ɉ����ē��͂���]������
        Quaternion rot = Quaternion.AngleAxis(_mainCamera.transform.eulerAngles.y, Vector3.up);
        _inputVec.Value = rot * new Vector3(hori, 0, vert).normalized;
        
        // ���݂̑��x��Animator�ɓ`����
        float speed = _inputVec.Value.magnitude * _mag.Value;
        _anim.SetFloat(_animParameterName, speed);

        // �W�����v�̓��͂��󂯎��
        _isJump = Input.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        if (_inputVec.Value != Vector3.zero)
        {
            // ���`�⊮���g�p���Ď��Ԃ������ĐU�����
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_inputVec.Value), _turnSpeed);
        }

        Vector3 veloVec = _inputVec.Value * _velocity * _mag.Value;
        veloVec.y = _rb.velocity.y;

        if (_isJump)
        {
            if (Physics.BoxCast(_foot.position, Vector3.one * 0.5f, -1 * Vector3.up, out RaycastHit hist, Quaternion.identity, 0.5f, _mask))
            {
                veloVec.y = _rb.velocity.y + 10;
            }

            _isJump = false;
        }

        _rb.velocity = veloVec;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(-1.0f * transform.up, Vector3.one);
    //}
}
