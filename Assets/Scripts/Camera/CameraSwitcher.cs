using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コライダーでカメラの切り替えを行う(仮)
/// </summary>
public class CameraSwitcher : MonoBehaviour
{
    CameraController _cameraCtr;
    CameraController.Parameter _prevParam;
    [SerializeField] CameraController.Parameter _param;

    void Start()
    {
        _cameraCtr = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _prevParam = _cameraCtr.Param.Clone();
            // カメラモードをLookItemにして固定にする
            _cameraCtr.SwitchCamera(_param);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // カメラモードをDefaultにしてプレイヤーに追従させる
            _cameraCtr.SwitchCamera(_prevParam);
        }
    }
}
