using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器のコライダーのオンオフのメソッドが宣言されたインターフェースを
/// 実装したクラス
/// </summary>
public class Weapon : MonoBehaviour, IWeaponControl
{
    Collider _weaponCollider;

    void Start()
    {
        _weaponCollider = GetComponent<Collider>();
        _weaponCollider.enabled = false;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// AnimatorにおいてAttackステートの開始時に一時的に武器のコライダーを
    /// 有効にするためのコールバックメソッド
    /// </summary>
    public void EnableWeaponCollider()
    {
        if (_weaponCollider != null)
            _weaponCollider.enabled = true;
    }

    /// <summary>
    /// AnimatorにおいてAttackステートの終了時に一時的に武器のコライダーを
    /// 無効にするためのコールバックメソッド
    /// </summary>
    public void DisableWeaponCollider()
    {
        if (_weaponCollider != null)
            _weaponCollider.enabled = false;
    }
}
