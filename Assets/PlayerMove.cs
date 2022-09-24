using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// プレイヤーの移動を行う
/// </summary>
public class PlayerMove : MonoBehaviour
{
    Rigidbody _rb;
    /// <summary>移動速度</summary>
    [SerializeField] float _velocity;
    /// <summary>入力されたベクトル</summary>
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
