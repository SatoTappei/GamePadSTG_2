using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームに登場するアクターのデータをまとめたSO
/// </summary>
[CreateAssetMenu]
public class ActorDataSO : ScriptableObject
{
    [SerializeField] CharacterTag _tag;
    [SerializeField] Sprite _icon;
    [SerializeField] int _maxHP;
    [SerializeField] int _attack;
    [SerializeField] int _speed;

    public CharacterTag Tag { get => _tag; }
    public Sprite Icon { get => _icon; }
    public int MaxHP { get => _maxHP; }
    public int Attack { get => _attack; }
    public int Speed { get => _speed; }
}
