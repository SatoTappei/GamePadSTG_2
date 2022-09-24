using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// “G‚ğ¶¬‚·‚é
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
    /// <summary>¶¬‚·‚é“G‚ÌƒvƒŒƒnƒu</summary>
    [SerializeField] GameObject _enemy;
    /// <summary>¶¬ƒŒ[ƒg</summary>
    [SerializeField] float _rate;

    void Awake()
    {

    }

    // ˆê’èŠÔŠu‚Å¶¬‚·‚é
    void Start()
    {
        // Interval‚ÌSubscribe‚É‚Í‰½‰ñ–Ú‚Ì”­s‚©‚ª“n‚³‚ê‚é
        Observable.Interval(System.TimeSpan.FromSeconds(_rate)).Subscribe(_ =>
        {
            Instantiate(_enemy, transform.position, Quaternion.identity);
        }).AddTo(this);
    }

    void Update()
    {
        
    }
}
