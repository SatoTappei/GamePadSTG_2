using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// キューブを生成する
/// </summary>
public class CubeGenerator : MonoBehaviour
{
    /// <summary>生成するキューブのプレハブ</summary>
    [SerializeField] GameObject _cube;
    /// <summary>キューブのデフォルトの生成レート</summary>
    [SerializeField] float _rate;

    void Awake()
    {

    }

    // 一定間隔で3*3*3のキューブを生成する
    void Start()
    {
        // IntervalのSubscribeには何回目の発行かが渡される
        Observable.Interval(System.TimeSpan.FromSeconds(_rate)).Subscribe(_ =>
        {
            Instantiate(_cube, transform.position, Quaternion.identity);
        }).AddTo(this);
    }

    void Update()
    {
        
    }
}
