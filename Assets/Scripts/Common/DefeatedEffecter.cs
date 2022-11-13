using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ���j���ꂽ�Ƃ��̉��o���s�� ���ݖ��g�p
/// </summary>
public class DefeatedEffecter : MonoBehaviour,IDisposable
{
    /// <summary>���j���ꂽ���ɍĐ������G�t�F�N�g�̃v���t�@�u</summary>
    [SerializeField] GameObject _Effect;

    GameObject go;

    public void Dispose()
    {
        Destroy(go);
    }

    void OnDisable()
    {
        go = Instantiate(_Effect, transform.position, Quaternion.identity);
    }
}
