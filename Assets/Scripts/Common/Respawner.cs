using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 範囲内に入った対象(プレイヤー)をリスポーン地点に戻す
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
