using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �Ə�����ˌ�������
/// </summary>
public class AimFire : MonoBehaviour
{
    AudioSource _as;
    /// <summary>�E���̃G�C���𓮂������ǂ���</summary>
    [SerializeField] bool _isRight;
    /// <summary>�e�̔��˃��[�g</summary>
    [SerializeField] float _rate;
    /// <summary>�ˌ����̉�</summary>
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
