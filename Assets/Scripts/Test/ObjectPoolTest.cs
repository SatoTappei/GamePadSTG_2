using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトプールの実装テスト
/// </summary>
public class ObjectPoolTest : MonoBehaviour
{
    /// <summary>オブジェクトプールのインスタンス、staticである</summary>
    public static ObjectPoolTest Instance;
    /// <summary>プールされたゲームオブジェクト</summary>
    List<GameObject> _pooledObjects;
    /// <summary>プール対象のオブジェクト</summary>
    [SerializeField] GameObject _objectToPool;
    /// <summary>プール数</summary>
    [SerializeField] int _amountToPool;

    /// <summary>オブジェクトプーリングのインスタンスを格納</summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>Start()時にオブジェクトプールし、非Activeにする</summary>
    void Start()
    {
        _pooledObjects = new List<GameObject>();
        GameObject go;
        for (int i = 0; i < _amountToPool; i++)
        {
            go = Instantiate(_objectToPool);
            go.SetActive(false);
            _pooledObjects.Add(go);
        }
    }

    void Update()
    {
        
    }

    /// <summary>
    /// オブジェクトプールからゲームオブジェクトを返却。
    /// 非アクティブのゲームオブジェクトを探して返却。
    /// </summary>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }
        return null;
    }
}
