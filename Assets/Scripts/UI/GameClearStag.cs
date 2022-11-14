using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Q�[���N���A�̉��o���s��
/// </summary>
public class GameClearStag : MonoBehaviour
{
    [SerializeField] Text _time;

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
    public void Play(int time)
    {
        transform.localScale = Vector3.one;
        _time.text = Timer.Convert(time);
    }
}
