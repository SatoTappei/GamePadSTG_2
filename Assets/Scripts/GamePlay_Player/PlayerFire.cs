using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �v���C���[�̍U���𐧌䂷��
/// </summary>
public class PlayerFire : MonoBehaviour
{
    Animator _anim;
    /// <summary>
    /// �I�[�f�B�I�̍Đ��̃e�X�g�A�I�������q�I�u�W�F�N�g�ɂȂ��Ă���
    /// TestAudio�I�u�W�F�N�g���Ə����B
    /// </summary>
    [SerializeField] AudioSource _as;
    /// <summary>�U���Ɏg������</summary>
    [SerializeField] GameObject _weapon;
    /// <summary>�U���o����Ԋu</summary>
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
        Debug.Log("�U������");
        // �T�E���h�Đ��̃e�X�g
        _as.Play();
    }
}
