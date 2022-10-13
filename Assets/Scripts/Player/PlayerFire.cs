using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ƒvƒŒƒCƒ„[‚ÌUŒ‚‚ğ§Œä‚·‚é
/// </summary>
public class PlayerFire : MonoBehaviour
{
    Animator _anim;
    /// <summary>UŒ‚‚Ég‚¤•Ší</summary>
    [SerializeField] GameObject _weapon;
    /// <summary>UŒ‚o—ˆ‚éŠÔŠu</summary>
    [SerializeField] float _attackRate;

    public GameObject Weapon => _weapon;

    void Awake()
    {
        _anim = GetComponentInChildren<Animator>();

        this.UpdateAsObservable()
            .Where(_ => Input.GetButton("Fire"))
            .ThrottleFirst(System.TimeSpan.FromSeconds(_attackRate))
            .Subscribe(_ => Fire())
            .AddTo(this);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>UŒ‚‚ğs‚¤</summary>
    void Fire()
    {
        _anim.Play("Slash");
    }
}
