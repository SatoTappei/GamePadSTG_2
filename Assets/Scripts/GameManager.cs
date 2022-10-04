using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���}�l�[�W���[
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // �t���[�����[�g��60fps�ɌŒ肷��
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
