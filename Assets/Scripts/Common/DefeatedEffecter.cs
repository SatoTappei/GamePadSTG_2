using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 撃破されたときの演出を行う
/// </summary>
public class DefeatedEffecter : MonoBehaviour
{
    /// <summary>撃破された時に再生されるエフェクトのプレファブ</summary>
    [SerializeField] GameObject _Effect;

    void OnDisable()
    {
        Instantiate(_Effect, transform.position, Quaternion.identity);
    }
}
