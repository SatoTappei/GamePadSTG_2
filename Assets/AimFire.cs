using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 照準から射撃をする_UniTaskバージョン
/// </summary>
public class AimFire : MonoBehaviour
{
    AudioSource _as;
    /// <summary>右側のエイムを動かすかどうか</summary>
    [SerializeField] bool _isRight;
    /// <summary>射撃時の音</summary>
    [SerializeField] AudioClip _clip;

    void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    async void Start()
    {
        await Trigger();
    }

    void Update()
    {

    }

    /// <summary>射撃</summary>
    async UniTask Trigger()
    {
        while (true)
        {
            await UniTask.Delay(200);
            await UniTask.WaitUntil(() => Input.GetButton("Fire_" + (_isRight ? "Right" : "Left")));
            _as.PlayOneShot(_clip);
        }
    }
}
