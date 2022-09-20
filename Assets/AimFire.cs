using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 照準から射撃をする
/// </summary>
public class AimFire : MonoBehaviour
{
    AudioSource _as;
    /// <summary>右側のエイムを動かすかどうか</summary>
    [SerializeField] bool _isRight;
    /// <summary>弾の発射レート</summary>
    [SerializeField] float _rate;
    /// <summary>射撃時の音</summary>
    [SerializeField] AudioClip _clip;

    void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetButton("Fire_" + (_isRight ? "Right" : "Left")))
            .ThrottleFirst(System.TimeSpan.FromSeconds(_rate))
            .Subscribe(_ => Fire());
    }

    void Update()
    {

    }

    void Fire()
    {
        _as.PlayOneShot(_clip);
    }
}
