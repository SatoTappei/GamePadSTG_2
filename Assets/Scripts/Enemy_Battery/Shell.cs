using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 撃ちだされた弾を制御する
/// 放物線を描いて飛んで、何かに当たったら消滅する
/// </summary>
public class Shell : MonoBehaviour
{
    /// <summary>重力</summary>
    readonly float _gravityScale = -0.003f;

    /// <summary>初速</summary>
    [SerializeField] float _initVelo = 3.0f;
    /// <summary>生存時間</summary>
    [SerializeField] float _lifeTime = 3.0f;
    /// <summary>ダメージを与えた際の処理を登録する</summary>
    [SerializeField] DamageSender _damageSender;
    /// <summary>速度</summary>
    float _velocity;
    /// <summary>重力加速度</summary>
    float _gravity = 0;

    /// <summary>現在の生存時間のタイマー</summary>
    float _timer;

    void OnEnable()
    {
        _damageSender.OnDamageSended += OnDamageSended;
        Init();
    }

    void OnDisable()
    {
        Return();
        _damageSender.OnDamageSended -= OnDamageSended;
    }

    void Start()
    {
        // 仮
        _damageSender.Init(10, "Player");
    }

    void Update()
    {
        // 時間がきたら消える
        _timer += Time.deltaTime;
        if (_timer > _lifeTime)
            gameObject.SetActive(false);

        Vector3 prevVec = transform.position;

        // 重力に従って落下するような挙動を計算する
        Vector3 moveVec = transform.forward * Time.deltaTime * _velocity;
        moveVec.y = _gravity;
        _gravity += _gravityScale;
        _velocity *= 0.95f;
        transform.position += moveVec;

        transform.forward = transform.position - prevVec;
    }

    /// <summary>オブジェクトがプールから取り出されたときに呼ばれる</summary>
    void Init()
    {
        // 速度を初速にする
        _velocity = _initVelo;
        _timer = 0;
    }

    /// <summary>オブジェクトがプールに返却されるときに呼ばれる</summary>
    void Return()
    {
        transform.position = default;
        transform.rotation = default;

        // 重力加速度を重力に戻す
        _gravity = _gravityScale;
    }

    // ダメージを与えた際の処理
    void OnDamageSended()
    {
        gameObject.SetActive(false);
    }
}
