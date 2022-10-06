using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �m�b�N�o�b�N�ȂǁA�_���[�W���󂯂鏈���S�ʂ̃e�X�g
/// </summary>
public class HitDamageTest : MonoBehaviour
{
    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogWarning("Rigidbody���擾�ł��܂���ł���:" + gameObject.name);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�e�X�g:�U�����󂯂������Ƃ͋t�����ɐ������</summary>
    public void BlowOff(Vector3 damagedDir)
    {
        Debug.Log("������΂���܂�");
        Vector3 dir = damagedDir.normalized;
        _rb.AddForce(dir * 20, ForceMode.Impulse);
    }
}
