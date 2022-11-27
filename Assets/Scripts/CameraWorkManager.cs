using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�������[�N�𐧌䂷��
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

    /// <summary>�^�C�g������Q�[���Ɉڂ�Ƃ��ɃJ�����̃^�[�Q�b�g��؂�ւ���</summary>
    public void MoveToInGame()
    {
        _objectRotate.enabled = false;
        _cameraController.Param._target = _player;
    }
}
