using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flyweightパターンで実装する敵のパラメータのデータ
/// </summary>
[CreateAssetMenu]
public class EnemyParamTest : ScriptableObject
{
    public string _name;
    public float _speed;
    public float _angle;
    public Color _bodyColor;
    public int _maxHp;
}
