using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// オブジェクトプールのテストで使用するキャノン砲の弾
/// これも本利用しないのでテスト終わったら消すこと
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
        // 非アクティブになるときに物理挙動周りを初期化する
        _rb.velocity = default;
        _rb.angularVelocity = default;
    }

    void Update()
    {
        
    }
}
