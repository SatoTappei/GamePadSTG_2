using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Y軸を基準に回転し続けるコンポーネント</summary>
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
