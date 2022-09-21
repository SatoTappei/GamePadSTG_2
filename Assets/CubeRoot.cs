using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L���[�u�̐e
/// </summary>
public class CubeRoot : MonoBehaviour
{
    /// <summary>�g�F�̃}�e���A��</summary>
    [SerializeField] Material _red;
    /// <summary>���F�̃}�e���A��</summary>
    [SerializeField] Material _blue;
    /// <summary>�ړ����x</summary>
    [SerializeField] float _speed;

    void Awake()
    {
        // ���ꂼ��̎q�I�u�W�F�N�g�̐F���g�����Ƀ����_���ŕς���
        foreach (Transform child in transform)
        {
            bool isRed = Random.Range(0, 2) == 1 ? true : false;
            child.GetComponent<MeshRenderer>().material = isRed ? _red : _blue;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(0, 0, -1 * Time.deltaTime * _speed);
    }
}
