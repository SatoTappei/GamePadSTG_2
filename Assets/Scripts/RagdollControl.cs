using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// ���񂾂Ƃ��ɏo�郉�O�h�[���ɗ͂�������
/// </summary>
public class RagdollControl : MonoBehaviour
{
    /// <summary>�͂�������w����RigidBody</summary>
    [SerializeField] Rigidbody _rbSpine;
    /// <summary>��莞�Ԍo�����畨�����Z����߂邽�߂̑���RigidBody</summary>
    [SerializeField] Rigidbody[] _rbOther;

    //async void Start()
    //{
    //    _rbSpine.AddForce(Vector3.up * 100, ForceMode.Impulse);
        
    //    // �������ׂ̌y���̂��߂�3�b��ɑS�Ă̕������Z����߂�
    //    await UniTask.Delay(3000);
        
    //    _rbSpine.Sleep();
    //    foreach(Rigidbody rb in _rbOther)
    //    {
    //        rb.Sleep();
    //    }
    //}
    IEnumerator Start()
    {
        _rbSpine.AddForce(Vector3.up * 100, ForceMode.Impulse);

        // �������ׂ̌y���̂��߂�3�b��ɑS�Ă̕������Z����߂�
        yield return new WaitForSeconds(3.0f);

        _rbSpine.Sleep();
        foreach (Rigidbody rb in _rbOther)
        {
            rb.Sleep();
        }
    }

    void Update()
    {

    }
}
