using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Y������ɉ�]��������R���|�[�l���g</summary>
public class ObjectRotate : MonoBehaviour
{
    [SerializeField] float _speed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0, _speed, 0);
    }
}
