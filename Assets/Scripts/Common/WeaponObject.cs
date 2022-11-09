using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクターが攻撃する際の武器となるオブジェクトにアタッチする
/// <summary>
/// 武器のコライダーのオンオフのメソッドが宣言された
/// インターフェースを実装したクラス
/// </summary>
public class WeaponObject : MonoBehaviour, IAttackAnimControl
{
    Collider _weaponCollider;

    void Awake()
    {
        _weaponCollider = GetComponent<Collider>();
        _weaponCollider.enabled = false;
    }

    void Update()
    {
        
    }

    /// <summary>コライダーを有効にして当たり判定を出す</summary>
    public void OnAnimEnter()
    {
        if (_weaponCollider != null)
            _weaponCollider.enabled = true;
    }

    /// <summary>コライダーを無効にして当たり判定を消す</summary>
    public void OnAnimExit()
    {
        if (_weaponCollider != null)
            _weaponCollider.enabled = false;
    }
}
