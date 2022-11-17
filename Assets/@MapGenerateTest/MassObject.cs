using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ������1�}�X�ɒu���}�b�v�I�u�W�F�N�g
/// </summary>
public class MassObject : MonoBehaviour
{
    [Header("���̒����烉���_����1�\������")]
    [SerializeField] GameObject[] _objects;

    [Header("�����������_���ɕς��邩�ǂ���")]
    [SerializeField] bool _isDirRandom;

    void Awake()
    {
        SetRandom();

        if (_isDirRandom)
            RotateRandom();
        //else
        //    RotateToCenter();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>���̒����烉���_����1�\������</summary>
    void SetRandom()
    {
        int r = Random.Range(0, _objects.Length);
        for (int i = 0; i < _objects.Length; i++)
            _objects[i].SetActive(i == r ? true : false);
    }

    /// <summary>90�����݂Ń����_���ɉ�]������</summary>
    void RotateRandom()
    {
        Vector3 parent = transform.parent.position;
        parent.y = 0;
        Vector3 pos = transform.TransformPoint(transform.position);
        pos.y = 0;

        Vector3 diff = parent - pos;
        (int x, int z) anglePair = (0, 0);

        anglePair.x = diff.x > 0 ? 90 : -90;
        anglePair.z = diff.z > 0 ? 0 : 180;

        int angle = Random.Range(0, 2) == 1 ? anglePair.x : anglePair.z;
        transform.eulerAngles = new Vector3(0, angle, 0);
    }

    ///// <summary>90�����݂Œ����������悤�ɉ�]������</summary>
    //void RotateToCenter()
    //{
    //    Vector3 parent = transform.parent.position;
    //    parent.y = 0;
    //    Vector3 pos = transform.TransformPoint(transform.position);
    //    pos.y = 0;

    //    Vector3 diff = (parent - pos).normalized;
    //    if (diff.x * diff.z != 0)
    //    {
    //        if (diff.x > 0 && diff.z < 0)
    //        {
    //            // �E��
    //        }
    //        else if (diff.x > 0 && diff.z > 0)
    //        {
    //            // �E��
    //        }
    //        else if (diff.x < 0 && diff.z < 0)
    //        {
    //            // ����
    //        }
    //        else if (diff.x < 0 && diff.z > 0)
    //        {
    //            // ����
    //        }
    //    }

    //    transform.forward = (parent - pos).normalized;
    //}
}
