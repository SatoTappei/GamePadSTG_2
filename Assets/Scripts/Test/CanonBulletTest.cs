using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �I�u�W�F�N�g�v�[���̃e�X�g�Ŏg�p����L���m���C�̒e
/// ������{���p���Ȃ��̂Ńe�X�g�I��������������
/// </summary>
public class CanonBulletTest : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField] float _power = 3.0f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    async void OnEnable()
    {
        _rb.AddForce(transform.up * _power, ForceMode.Impulse);
        await UniTask.Delay(System.TimeSpan.FromSeconds(3.0f));
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        // ��A�N�e�B�u�ɂȂ�Ƃ��ɕ����������������������
        _rb.velocity = default;
        _rb.angularVelocity = default;
    }

    void Update()
    {
        
    }
}
