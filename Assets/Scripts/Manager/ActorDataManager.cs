using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ActorDataSO���Ǘ�����A�e�L�����N�^�[�͂������玩�g��SO���擾���ăX�e�[�^�X������������
/// </summary>
public class ActorDataManager : MonoBehaviour
{
    /// <summary>�G�̎�ނ��Ƃ̋��ʂ����f�[�^�̎Q�Ɛ��SO</summary>
    [SerializeField] ActorDataSO[] _enemyDatas;
    /// <summary>�G�̋��ʂ����f�[�^�̎Q�Ɛ�ƂȂ�SO��l�Ƃ��ĕۑ����鎫���^</summary>
    Dictionary<CharacterTag, ActorDataSO> _enemyDataDic = new Dictionary<CharacterTag, ActorDataSO>();

    void Awake()
    {
        foreach (ActorDataSO so in _enemyDatas)
        {
            _enemyDataDic.Add(so.Tag, so);
        }
    }

    /// <summary>�^�O��n���ƑΉ�����SO��Ԃ�</summary>
    public ActorDataSO GetEnemyData(CharacterTag tag) => _enemyDataDic[tag];
}
