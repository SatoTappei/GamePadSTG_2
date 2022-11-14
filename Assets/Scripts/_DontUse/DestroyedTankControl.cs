using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// ���j���ꂽ��Ԃ̉��o���s��
/// </summary>
public class DestroyedTankControl : MonoBehaviour
{
    [SerializeField] Rigidbody _rbTurret;

    async void Start()
    {
        // x,z�������Ƀ����_��������������
        Vector3 sideDir = Vector3.right * Random.Range(-1.5f, 1.5f);
        Vector3 forwardDir = Vector3.forward * Random.Range(-1.5f, 1.5f);

        _rbTurret.AddForce(Vector3.up * 10 + sideDir + forwardDir, ForceMode.Impulse);

        // �������׌y���ׂ̈�3�b��ɂ͕����������I�t�ɂ���
        await UniTask.DelayFrame(3000);
        _rbTurret.Sleep();
    }

    void Update()
    {
        
    }
}
