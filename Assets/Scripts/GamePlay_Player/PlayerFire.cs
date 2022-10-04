using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �v���C���[�̍U���𐧌䂷��
/// </summary>
public class PlayerFire : MonoBehaviour
{
    /// <summary>�U���o����Ԋu</summary>
    [SerializeField] float _attackRate;

    void Awake()
    {

    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetButton("Fire"))
            .ThrottleFirst(System.TimeSpan.FromSeconds(_attackRate))
            .Subscribe(_ => Fire());
    }

    void Update()
    {

    }

    void Fire()
    {
        Debug.Log("�U������");
    }
}
