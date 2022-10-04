using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// 敵を生成する(未使用)
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
    /// <summary>生成する敵のプレハブ</summary>
    [SerializeField] GameObject _enemy;
    /// <summary>生成レート</summary>
    [SerializeField] float _rate;

    void Awake()
    {

    }

    // 一定間隔で生成する
    void Start()
    {
        // IntervalのSubscribeには何回目の発行かが渡される
        Observable.Interval(System.TimeSpan.FromSeconds(_rate)).Subscribe(_ =>
        {
            Instantiate(_enemy, transform.position, Quaternion.identity);
        }).AddTo(this);
    }

    void Update()
    {
        
    }
}
