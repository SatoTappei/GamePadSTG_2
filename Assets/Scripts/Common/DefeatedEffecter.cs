using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���j���ꂽ�Ƃ��̉��o���s��
/// </summary>
public class DefeatedEffecter : MonoBehaviour
{
    /// <summary>���j���ꂽ���ɍĐ������G�t�F�N�g�̃v���t�@�u</summary>
    [SerializeField] GameObject _Effect;

    void OnDisable()
    {
        Instantiate(_Effect, transform.position, Quaternion.identity);
    }
}
