using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�v�[��
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /// <summary>�v�[������I�u�W�F�N�g</summary>
    [SerializeField] GameObject _object;
    /// <summary>�v�[������鐔</summary>
    [SerializeField] int _amount;
    /// <summary>�v�[�����ꂽ�I�u�W�F�N�g�̃q�G�����L�[��ł̐e</summary>
    [SerializeField] Transform _parent;
    /// <summary>�v�[�����ꂽ�I�u�W�F�N�g�̃��X�g</summary>
    List<GameObject> _pooledObjects;

    void Start()
    {
        // �v�[�����Ă���
        _pooledObjects = new List<GameObject>();
        for (int i = 0; i < _amount; i++)
        {
            GameObject go = Instantiate(_object);
            go.SetActive(false);
            go.transform.SetParent(_parent);
            _pooledObjects.Add(go);
        }
    }

    /// <summary>�I�u�W�F�N�g�v�[�������A�N�e�B�u�̃I�u�W�F�N�g��T���ĕԋp</summary>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _amount; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }

        Debug.LogWarning("�v�[�����ꂽ�I�u�W�F�N�g������܂���:" + _object.name);
        return null;
    }
}
