using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �I�u�W�F�N�g�v�[���̃e�X�g
/// ObjectPoolTest�R���|�[�l���g����v�[������Ă���I�u�W�F�N�g���Ăяo��
/// </summary>
[RequireComponent(typeof(ObjectPoolTest))]
public class ObjectPoolSenderTest : MonoBehaviour
{
    [SerializeField] Transform _muzzle;

    void Start()
    {
        Observable.Interval(System.TimeSpan.FromSeconds(1.0f)).Subscribe(_ =>
        {
            GameObject go = ObjectPoolTest.Instance.GetPooledObject();
            // �v�[�����������炩��̎���null���Ԃ��Ă���̂�null�`�F�b�N����
            if (go != null)
            {
                go.transform.position = _muzzle.transform.position;
                go.transform.rotation = _muzzle.transform.rotation;
                go.SetActive(true);
            }
        }).AddTo(this);
    }

    void Update()
    {
        
    }


}
