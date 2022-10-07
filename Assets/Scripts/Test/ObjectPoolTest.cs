using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�v�[���̎����e�X�g
/// </summary>
public class ObjectPoolTest : MonoBehaviour
{
    /// <summary>�I�u�W�F�N�g�v�[���̃C���X�^���X�Astatic�ł���</summary>
    public static ObjectPoolTest Instance;
    /// <summary>�v�[�����ꂽ�Q�[���I�u�W�F�N�g</summary>
    List<GameObject> _pooledObjects;
    /// <summary>�v�[���Ώۂ̃I�u�W�F�N�g</summary>
    [SerializeField] GameObject _objectToPool;
    /// <summary>�v�[����</summary>
    [SerializeField] int _amountToPool;

    /// <summary>�I�u�W�F�N�g�v�[�����O�̃C���X�^���X���i�[</summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>Start()���ɃI�u�W�F�N�g�v�[�����A��Active�ɂ���</summary>
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
    /// �I�u�W�F�N�g�v�[������Q�[���I�u�W�F�N�g��ԋp�B
    /// ��A�N�e�B�u�̃Q�[���I�u�W�F�N�g��T���ĕԋp�B
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
