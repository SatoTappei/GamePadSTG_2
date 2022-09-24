using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �v���C���[�̈ړ����s��
/// </summary>
public class PlayerMove : MonoBehaviour
{
    Rigidbody _rb;
    /// <summary>�ړ����x</summary>
    [SerializeField] float _velocity;
    /// <summary>���͂��ꂽ�x�N�g��</summary>
    Vector3 _inputVec;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void Update()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        _inputVec = new Vector3(hori, 0, vert).normalized;
    }

    void FixedUpdate()
    {
        _rb.velocity = _inputVec * _velocity;
    }
}
