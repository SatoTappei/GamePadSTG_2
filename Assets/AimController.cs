using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 照準の移動と射撃を行う
/// </summary>
public class AimController : MonoBehaviour
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
        Debug.Log(_inputVec);

        //if (Input.GetButton("Fire" + (_useLeftStick ? "1" : "2")))
        //{
        //    Debug.Log("ガン射");
        //}
    }

    void FixedUpdate()
    {
        _rb.velocity = _inputVec * _moveMag;
    }

    /// <summary>射撃</summary>
    IEnumerator Trigger()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); 
        }
    }
}
