using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// オブジェクトプールのテスト
/// ObjectPoolTestコンポーネントからプールされているオブジェクトを呼び出す
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
            // プールがすっからかんの時にnullが返ってくるのでnullチェックする
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
