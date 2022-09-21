using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// �L���[�u�𐶐�����
/// </summary>
public class CubeGenerator : MonoBehaviour
{
    /// <summary>��������L���[�u�̃v���n�u</summary>
    [SerializeField] GameObject _cube;
    /// <summary>�L���[�u�̃f�t�H���g�̐������[�g</summary>
    [SerializeField] float _rate;

    void Awake()
    {

    }

    // ���Ԋu��3*3*3�̃L���[�u�𐶐�����
    void Start()
    {
        // Interval��Subscribe�ɂ͉���ڂ̔��s�����n�����
        Observable.Interval(System.TimeSpan.FromSeconds(_rate)).Subscribe(_ =>
        {
            Instantiate(_cube, transform.position, Quaternion.identity);
        }).AddTo(this);
    }

    void Update()
    {
        
    }
}
