using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ���ޗp�̃T���v���ł���A����͎g��Ȃ��̂ŃG���[���������߂ɍ���Ă���B
/// ���ޓǔj�������悱�̃R���|�[�l���g���Ə���
/// sealed�����q�c���̃N���X�͌p���ł��Ȃ�
/// </summary>
public sealed class WayPointManagerTest : MonoBehaviour
{
    static WayPointManagerTest instance;

    List<GameObject> _waypoints = new List<GameObject>();
    public List<GameObject> Waypoints { get => _waypoints; }

    /// <summary>
    /// �V���O���g�������݂��Ȃ��ꍇ�͍쐬���A
    /// "WayPoint"�^�O���ݒ肳��Ă���I�u�W�F�N�g�����X�g�ɒǉ�
    /// </summary>
    public static WayPointManagerTest Singleton
    {
        get
        {
            if(instance == null)
            {
                instance = new WayPointManagerTest();
                instance._waypoints.AddRange(GameObject.FindGameObjectsWithTag("WayPoint"));
                instance._waypoints = instance.Waypoints.OrderBy(waypoint => waypoint.name).ToList();
            }
            return instance;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
