using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 撃破されたときの演出を行う 現在未使用
/// </summary>
public class DefeatedEffecter : MonoBehaviour,IDisposable
{
    /// <summary>撃破された時に再生されるエフェクトのプレファブ</summary>
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
