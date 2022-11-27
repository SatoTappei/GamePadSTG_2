using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラワークを制御する
/// </summary>
public class CameraWorkManager : MonoBehaviour
{
    [SerializeField] ObjectRotate _objectRotate;
    [SerializeField] Transform _player;
    [SerializeField] CameraController _cameraController;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>タイトルからゲームに移るときにカメラのターゲットを切り替える</summary>
    public void MoveToInGame()
    {
        _objectRotate.enabled = false;
        _cameraController.Param._target = _player;
    }
}
