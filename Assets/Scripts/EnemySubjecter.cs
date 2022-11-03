using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 後々良い感じの場所に移す
/// <summary>敵を識別するためのタグとして使う</summary>
public enum EnemyTag
{
    BlueSoldier,
    Tank,
    BossTank,
}

/// <summary>
/// 敵の情報をEnemyManagerに登録する
/// </summary>
public class EnemySubjecter : MonoBehaviour
{
    EnemyAIBase _aiBase;
    [SerializeField] EnemyTag _tag;

    public EnemyTag Tag { get; private set; }

    void Awake()
    {
        _aiBase = GetComponent<EnemyAIBase>();
    }

    void Start()
    {
        // 機能させるかどうかを管理してもらうために自身を登録する
        FindObjectOfType<EnemyManager>().AddEnemyList(this);
    }

    void Update()
    {
        
    }

    // 敵を起こす
    public void WakeUp()
    {
        _aiBase.WakeUp();
    }
}
