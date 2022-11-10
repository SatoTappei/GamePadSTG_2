using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playerと同じ構成にするため
/// 近接攻撃してくる敵の武器の参照を専用のクラスで保持しておく
/// </summary>
public class SoliderFire : MonoBehaviour
{
    /// <summary>攻撃に使用する武器</summary>
    [SerializeField] GameObject _weapon;

    public GameObject Weapon { get => _weapon; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
