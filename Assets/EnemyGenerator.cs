using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// �G�𐶐�����
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
    /// <summary>��������G�̃v���n�u</summary>
    [SerializeField] GameObject _enemy;
    /// <summary>�������[�g</summary>
    [SerializeField] float _rate;

    void Awake()
    {

    }

    // ���Ԋu�Ő�������
    void Start()
    {
        // Interval��Subscribe�ɂ͉���ڂ̔��s�����n�����
        Observable.Interval(System.TimeSpan.FromSeconds(_rate)).Subscribe(_ =>
        {
            Instantiate(_enemy, transform.position, Quaternion.identity);
        }).AddTo(this);
    }

    void Update()
    {
        
    }
}
