using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを背後から追従するカメラ(未使用)
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>カメラが追従するターゲット</summary>
    [SerializeField] Transform _target;
    /// <summary>ターゲットを追従する際のy位置のオフセット</summary>    
    [SerializeField] float _yOffset = 2.22f;
    /// <summary>ターゲットを追従する際のz位置のオフセット</summary>
    [SerializeField] float _zOffset = -3.33f;
    /// <summary>ターゲットを追従する際の角度オフセット</summary>
    [SerializeField] float _angleOffset = 2.0f;

    void Start()
    {

    }

    void Update()
    {

    }

    void LateUpdate()
    {
        // ターゲットの前方向の位置ベクトルからオフセット分ずらした場所に移動する
        transform.position = _target.position + (_target.forward * _zOffset) + (Vector3.up * _yOffset);
        // ターゲットからオフセット分ずらした位置を映す
        Vector3 lookCam = _target.position + Vector3.up * _angleOffset;
        transform.LookAt(lookCam);
    }
}
