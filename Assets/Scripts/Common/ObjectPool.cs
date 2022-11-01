using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトプール
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /// <summary>プールするオブジェクト</summary>
    [SerializeField] GameObject _object;
    /// <summary>プールされる数</summary>
    [SerializeField] int _amount;
    /// <summary>プールされたオブジェクトのヒエラルキー上での親</summary>
    [SerializeField] Transform _parent;
    /// <summary>プールされたオブジェクトのリスト</summary>
    List<GameObject> _pooledObjects;

    void Start()
    {
        // プールしておく
        _pooledObjects = new List<GameObject>();
        for (int i = 0; i < _amount; i++)
        {
            GameObject go = Instantiate(_object);
            go.SetActive(false);
            go.transform.SetParent(_parent);
            _pooledObjects.Add(go);
        }
    }

    /// <summary>オブジェクトプールから非アクティブのオブジェクトを探して返却</summary>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _amount; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }

        Debug.LogWarning("プールされたオブジェクトが足りません:" + _object.name);
        return null;
    }
}
