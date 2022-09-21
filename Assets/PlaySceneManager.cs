using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ゲーム全体の進行を管理する
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    PlaySceneUIManager _psUIm;
    /// <summary>キューブが触れるとプレイヤーへのダメージとなるコライダー</summary>
    [SerializeField] Collider _damageZone;
    /// <summary>プレイヤーの最大体力</summary>
    [SerializeField] int _maxLife;
    /// <summary>プレイヤーの現在の体力</summary>
    IntReactiveProperty _currentLife = new IntReactiveProperty();

    void Awake()
    {
        // 最大体力を現在の体力としてセット
        _currentLife.Value = _maxLife;
        _psUIm = GetComponent<PlaySceneUIManager>();

        _damageZone.OnTriggerEnterAsObservable()
            .Where(c => c.CompareTag("Cube"))
            .Subscribe(_ => Debug.Log("ヒットした"));

        _currentLife.Subscribe(i => _psUIm.SetLifeGauge(_maxLife, i));
        _currentLife.Where(i => i == 0).Subscribe(_ => Debug.Log("死亡"));
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.anyKeyDown) _currentLife.Value--;
    }
}
