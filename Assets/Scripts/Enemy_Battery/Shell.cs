using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ���������ꂽ�e�𐧌䂷��
/// ��������`���Ĕ��ŁA�����ɓ�����������ł���
/// </summary>
public class Shell : MonoBehaviour
{
    /// <summary>�d��</summary>
    readonly float _gravityScale = -0.003f;

    /// <summary>����</summary>
    [SerializeField] float _initVelo = 3.0f;
    /// <summary>��������</summary>
    [SerializeField] float _lifeTime = 3.0f;
    /// <summary>�_���[�W��^�����ۂ̏�����o�^����</summary>
    [SerializeField] DamageSender _damageSender;
    /// <summary>���x</summary>
    float _velocity;
    /// <summary>�d�͉����x</summary>
    float _gravity = 0;

    void OnEnable()
    {
        _damageSender.OnDamageSended += OnDamageSended;
        Init();
    }

    void OnDisable()
    {
        Return();
        _damageSender.OnDamageSended -= OnDamageSended;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 prevVec = transform.position;

        // �d�͂ɏ]���ė�������悤�ȋ������v�Z����
        Vector3 moveVec = transform.forward * Time.deltaTime * _velocity;
        moveVec.y = _gravity;
        _gravity += _gravityScale;
        _velocity *= 0.95f;
        transform.position += moveVec;

        transform.forward = transform.position - prevVec;
    }

    /// <summary>�I�u�W�F�N�g���v�[��������o���ꂽ�Ƃ��ɌĂ΂��</summary>
    void Init()
    {
        // ���x�������ɂ���
        _velocity = _initVelo;

        DOVirtual.DelayedCall(_lifeTime, () =>
        {
            gameObject.SetActive(false);
        }, ignoreTimeScale: false); // Unity�̃^�C���X�P�[���Ɉˑ�������
    }

    /// <summary>�I�u�W�F�N�g���v�[���ɕԋp�����Ƃ��ɌĂ΂��</summary>
    void Return()
    {
        transform.position = default;
        transform.rotation = default;

        // �d�͉����x���d�͂ɖ߂�
        _gravity = _gravityScale;
    }

    // �_���[�W��^�����ۂ̏���
    void OnDamageSended()
    {
        gameObject.SetActive(false);
        Debug.Log("�e�������܂���");
    }
}
