using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 照準の移動を行う
/// </summary>
public class AimMove : MonoBehaviour
{
    Rigidbody2D _rb;
    /// <summary>右側のエイムを動かすかどうか</summary>
    [SerializeField] bool _isRight;
    /// <summary>照準の移動速度</summary>
    [SerializeField] int _moveMag;

    /// <summary>入力されたベクトル</summary>
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
    }

    void FixedUpdate()
    {
        _rb.velocity = _inputVec * _moveMag;
    }
}
