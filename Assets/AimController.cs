using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ə��̈ړ��Ǝˌ����s��
/// </summary>
public class AimController : MonoBehaviour
{
    Rigidbody2D _rb;
    /// <summary>�E���̃G�C���𓮂������ǂ���</summary>
    [SerializeField] bool _isRight;
    /// <summary>�Ə��̈ړ����x</summary>
    [SerializeField] int _moveMag;

    /// <summary>���͂��ꂽ�x�N�g��</summary>
    Vector3 _inputVec;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        float hori = Input.GetAxisRaw("Horizontal_" + (_isRight ? "Right" : "Left"));
        float vert = Input.GetAxisRaw("Vertical_" + (_isRight ? "Right" : "Left"));

        _inputVec = new Vector3(hori, -1 * vert, 0).normalized;
        Debug.Log(_inputVec);

        //if (Input.GetButton("Fire" + (_useLeftStick ? "1" : "2")))
        //{
        //    Debug.Log("�K����");
        //}
    }

    void FixedUpdate()
    {
        _rb.velocity = _inputVec * _moveMag;
    }

    /// <summary>�ˌ�</summary>
    IEnumerator Trigger()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); 
        }
    }
}
