using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キューブの親
/// </summary>
public class CubeRoot : MonoBehaviour
{
    /// <summary>紅色のマテリアル</summary>
    [SerializeField] Material _red;
    /// <summary>蒼色のマテリアル</summary>
    [SerializeField] Material _blue;
    /// <summary>移動速度</summary>
    [SerializeField] float _speed;

    void Awake()
    {
        // それぞれの子オブジェクトの色を紅か蒼にランダムで変える
        foreach (Transform child in transform)
        {
            bool isRed = Random.Range(0, 2) == 1 ? true : false;
            child.GetComponent<MeshRenderer>().material = isRed ? _red : _blue;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(0, 0, -1 * Time.deltaTime * _speed);
    }
}
