using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���I�[�o�[�̉��o���s��
/// </summary>
public class GameOverStag : MonoBehaviour
{
    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>���o���Đ�����</summary>
    public void Play()
    {
        // TODO:���o�����
        transform.localScale = Vector3.one;
    }
}
