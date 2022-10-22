using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>マップ上の1マス</summary>
public class Mass
{
    /// <summary>マップ上での座標、区画内での番号とは違うので注意</summary>
    (int z, int x) _pos;
    char _char;

    public (int z, int x) Pos { get => _pos; }
    public char Char { get => _char; set => _char = value; }
}

/// <summary>区域を構成する区画</summary>
public class Section
{
    readonly int _height;
    readonly int _widht;
    Mass[,] _masses;

    public Section(int height, int width)
    {
        _masses = new Mass[height, width];
    }

    public Mass[,] Masses { get => _masses; }
    public int Height { get => _height; }
    public int Widht { get => _widht; }

    /// <summary>区画内の番号を渡すと対応した対応したマスを返す</summary>
    public Mass GetMass(int z, int x) => _masses[z, x];

    /// <summary>この区画を文字列二次元配列にして返す。</summary>
    public char[,] GetStringArray()
    {
        char[,] array = new char[_height, _widht];
        for (int z = 0; z < _height; z++)
            for (int x = 0; x < _widht; x++)
            {
                array[z, x] = _masses[z, x].Char;
            }

        return array;
    }

    /// <summary>文字列二次元配列を区画に反映させる</summary>
    public void SetStringArray(char[,] array)
    {
        if (array.GetLength(0) != _height || 
            array.GetLength(1) != _widht)
        {
            Debug.LogWarning("渡された配列が区画の大きさと違います。");
            return;
        }

        for (int z = 0; z < _height; z++)
            for (int x = 0; x < _widht; x++)
            {
                _masses[z, x].Char = array[z, x];
            }
    }
}

/// <summary>マップを構成する区域</summary>
public class Area
{
    readonly int _height = 7;
    readonly int _widht = 7;

    /// <summary>各区画はテンキーの番号に対応している</summary>
    Section[] _sections = new Section[]
    {
        new Section(3,3),   // 左下
        new Section(3,1),   // 下
        new Section(3,3),   // 右下
        new Section(1,3),   // 左
        new Section(1,1),   // 真ん中
        new Section(1,3),   // 右
        new Section(3,3),   // 左上
        new Section(3,1),   // 上
        new Section(3,3),   // 右上
    };

    // 各区画を合体させて1つの文字列型の二次元配列にして返す
    public char[,] GetStringArray()
    {
        char[,] mapArray = new char[_height, _widht];
        foreach (Section sct in _sections)
        {
            for (int z = 0; z < sct.Height; z++)
                for (int x = 0; x < sct.Widht; x++)
                {
                    // 区画内の各マスをマップの文字列二次元配列に反映する
                    Mass mass = sct.GetMass(z, x);
                    mapArray[mass.Pos.z, mass.Pos.x] = mass.Char;
                }
        }

        return mapArray;
    }
}

/// <summary>
/// 多数の区域からなるマップ
/// 生成は区域ごとに行うのでマップクラスには文字列二次元配列は持たない
/// </summary>
public class Map
{
    public Area[,] Areas { get; set; }

    public Map(int height, int width)
    {
        Areas = new Area[height, width];

        for (int z = 0; z < height; z++)
            for (int x = 0; x < width; x++)
                Areas[z, x] = new Area();
    }
}

/// <summary>
/// 複数のスクリプトで共通して使うもの
/// </summary>
public class MapGenerateUtility
{
    // マップの大きさは奇数じゃないと区域を綺麗に並べることが出来ない
    // 大きすぎると負荷がすごい(かも)ので最大でも5*5にしておく
    public static readonly int MapWidth = 5;
    public static readonly int MapHeight = 5;
    /// <summary>区域の一辺の幅、奇数でいい感じの値である7で固定</summary>
    public static readonly int AreaWide = 7;
}
