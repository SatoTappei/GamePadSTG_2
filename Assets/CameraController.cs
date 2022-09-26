using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを背後から追従するカメラ
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>カメラが追従するターゲット</summary>
    [SerializeField] Transform _target;
    /// <summary>ターゲットを追従する際の位置オフセット</summary>
    //[SerializeField] Vector3 _posOffset;
    /// <summary>ターゲットを追従する際のy位置のオフセット</summary>    
    [SerializeField] float _yOffset;
    /// <summary>ターゲットを追従する際のz位置のオフセット</summary>
    [SerializeField] float _zOffset;
    /// <summary>ターゲットを追従する際の角度オフセット</summary>
    [SerializeField] float _angleOffset;
    /// <summary>カメラが追従するターゲットが停止しているときの角度</summary>
    //Quaternion _targetStopRot;
    /// <summary>カメラが追従するターゲットが移動しているときの角度</summary>
    //Quaternion _targetMoveRot;

    void Start()
    {
        
    }

    void Update()
    {
        //float hori = Input.GetAxis("Horizontal");
        //float vert = Input.GetAxis("Vertical");
        //Vector3 inputVec = new Vector3(hori, 0, vert);

        //_targetMoveRot = _target.transform.rotation;
        //transform.rotation = Quaternion.Euler(0, _targetMoveRot.eulerAngles.y, 0);

        //if (inputVec != Vector3.zero)
        //{
        //    _targetMoveRot = _target.transform.rotation;
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0, _targetMoveRot.eulerAngles.y, 0);
        //    _targetStopRot = _target.transform.rotation;
        //}
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
