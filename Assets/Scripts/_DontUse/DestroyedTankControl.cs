using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 撃破された戦車の演出を行う
/// </summary>
public class DestroyedTankControl : MonoBehaviour
{
    [SerializeField] Rigidbody _rbTurret;

    IEnumerator Start()
    {
        // x,z軸方向にランダム性を持たせる
        Vector3 sideDir = Vector3.right * Random.Range(-1.5f, 1.5f);
        Vector3 forwardDir = Vector3.forward * Random.Range(-1.5f, 1.5f);

        _rbTurret.AddForce(Vector3.up * 10 + sideDir + forwardDir, ForceMode.Impulse);

        // 処理負荷軽減の為に3秒後には物理挙動をオフにする
        yield return new WaitForSeconds(3.0f);
        _rbTurret.Sleep();
    }

    void Update()
    {
        
    }
}
