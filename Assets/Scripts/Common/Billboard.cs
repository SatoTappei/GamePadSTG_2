using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ビルボード機能
/// </summary>
public class Billboard : MonoBehaviour
{
    Camera _main;

    void Start()
    {
        _main = Camera.main;
    }

    void Update()
    {
        Vector3 rot = _main.transform.position;
        rot.y = 0;
        transform.LookAt(rot);
    }
}
