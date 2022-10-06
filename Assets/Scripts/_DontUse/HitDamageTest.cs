using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ノックバックなど、ダメージを受ける処理全般のテスト
/// </summary>
public class HitDamageTest : MonoBehaviour
{
    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogWarning("Rigidbodyを取得できませんでした:" + gameObject.name);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>テスト:攻撃を受けた方向とは逆向きに吹っ飛ぶ</summary>
    public void BlowOff(Vector3 damagedDir)
    {
        Debug.Log("吹っ飛ばされます");
        Vector3 dir = damagedDir.normalized;
        _rb.AddForce(dir * 20, ForceMode.Impulse);
    }
}
