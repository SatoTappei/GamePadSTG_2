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
    /// <summary>UŒ‚o—ˆ‚éŠÔŠu</summary>
    [SerializeField] float _attackRate;

    void Awake()
    {

    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetButton("Fire"))
            .ThrottleFirst(System.TimeSpan.FromSeconds(_attackRate))
            .Subscribe(_ => Fire());
    }

    void Update()
    {

    }

    void Fire()
    {
        Debug.Log("UŒ‚‚µ‚½");
    }
}
