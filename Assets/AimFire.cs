using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �Ə�����ˌ�������_UniTask�o�[�W����
/// </summary>
public class AimFire : MonoBehaviour
{
    AudioSource _as;
    /// <summary>�E���̃G�C���𓮂������ǂ���</summary>
    [SerializeField] bool _isRight;
    /// <summary>�ˌ����̉�</summary>
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

    /// <summary>�ˌ�</summary>
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
