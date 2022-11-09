using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ActorDataSOを管理する、各キャラクターはここから自身のSOを取得してステータスを初期化する
/// </summary>
public class ActorDataManager : MonoBehaviour
{
    /// <summary>敵の種類ごとの共通したデータの参照先のSO</summary>
    [SerializeField] ActorDataSO[] _enemyDatas;
    /// <summary>敵の共通したデータの参照先となるSOを値として保存する辞書型</summary>
    Dictionary<CharacterTag, ActorDataSO> _enemyDataDic = new Dictionary<CharacterTag, ActorDataSO>();

    void Awake()
    {
        foreach (ActorDataSO so in _enemyDatas)
        {
            _enemyDataDic.Add(so.Tag, so);
        }
    }

    /// <summary>タグを渡すと対応したSOを返す</summary>
    public ActorDataSO GetEnemyData(CharacterTag tag) => _enemyDataDic[tag];
}
