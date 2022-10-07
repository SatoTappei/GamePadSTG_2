using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 教材用のサンプルであり、今回は使わないのでエラーを消すために作ってある。
/// 教材読破完了次第このコンポーネントごと消す
/// sealed装飾子…このクラスは継承できない
/// </summary>
public sealed class WayPointManagerTest : MonoBehaviour
{
    static WayPointManagerTest instance;

    List<GameObject> _waypoints = new List<GameObject>();
    public List<GameObject> Waypoints { get => _waypoints; }

    /// <summary>
    /// シングルトンが存在しない場合は作成し、
    /// "WayPoint"タグが設定されているオブジェクトをリストに追加
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
