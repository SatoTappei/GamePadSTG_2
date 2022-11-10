using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 死んだときに出るラグドールに力を加える
/// </summary>
public class RagdollControl : MonoBehaviour
{
    /// <summary>力を加える背骨のRigidBody</summary>
    [SerializeField] Rigidbody _rbSpine;
    /// <summary>一定時間経ったら物理演算をやめるための他のRigidBody</summary>
    [SerializeField] Rigidbody[] _rbOther;

    async void Start()
    {
        _rbSpine.AddForce(Vector3.up * 100, ForceMode.Impulse);
        
        // 処理負荷の軽減のために3秒後に全ての物理演算をやめる
        await UniTask.Delay(3000);
        
        _rbSpine.Sleep();
        foreach(Rigidbody rb in _rbOther)
        {
            rb.Sleep();
        }
    }

    void Update()
    {

    }
}
