using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flyweight�p�^�[���Ŏ�������G�̃p�����[�^�̃f�[�^
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
