using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �͈͓��ɓ������Ώ�(�v���C���[)�����X�|�[���n�_�ɖ߂�
/// </summary>
public class Respawner : MonoBehaviour
{
    [SerializeField] Transform _point;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = _point.position;
        }
    }
}
