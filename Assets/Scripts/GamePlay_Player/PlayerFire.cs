using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// プレイヤーの攻撃を制御する
/// </summary>
public class PlayerFire : MonoBehaviour
{
    Animator _anim;
    /// <summary>
    /// オーディオの再生のテスト、終わったら子オブジェクトになっている
    /// TestAudioオブジェクトごと消す。
    /// </summary>
    [SerializeField] AudioSource _as;
    /// <summary>攻撃に使う武器</summary>
    [SerializeField] GameObject _weapon;
    /// <summary>攻撃出来る間隔</summary>
    [SerializeField] float _attackRate;
    public GameObject Weapon => _weapon;

    void Awake()
    {
        _anim = GetComponentInChildren<Animator>();

        this.UpdateAsObservable()
            .Where(_ => Input.GetButton("Fire"))
            .ThrottleFirst(System.TimeSpan.FromSeconds(_attackRate))
            .Subscribe(_ => Fire());
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void Fire()
    {
        _anim.Play("Slash");
        Debug.Log("攻撃した");
        // サウンド再生のテスト
        _as.Play();
    }
}
