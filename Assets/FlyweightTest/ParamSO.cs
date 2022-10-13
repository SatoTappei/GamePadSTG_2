using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ParamSO : ScriptableObject
{
    // 同じ種類のオブジェクトで共通して使うデータ
    public string _name;
    public float _speed;
    public float _angle;
    public Color _bodyColor;
    public int _maxHp;
}
