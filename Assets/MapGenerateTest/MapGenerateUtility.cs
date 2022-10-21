using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>マップ上の1マス</summary>
public class Mass
{
    int _x;
    int _z;
    public char _char;
}

/// <summary>区域を構成する区画</summary>
public class Section
{
    int _height;
    int _widht;
    Mass[,] _masses;

    public Section(int height, int width)
    {
        _masses = new Mass[height, width];
    }

    /// <summary>この区画の文字を二次元配列にして返す</summary>
    public char[,] GetStringArray()
    {
        char[,] array = new char[_height, _widht];
        for (int z = 0; z < _height; z++)
            for (int x = 0; x < _widht; x++)
            {
                array[z, x] = _masses[z, x]._char;
            }

        return array;
    }
}

/// <summary>マップを構成する区域</summary>
public class Area
{
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
        // このメソッドを呼び出して返した二次元配列をもとに
        // マップをオブジェクトとして生成することを留意する
        // TODO:ここから
        char[,] mapArray = new char[7, 7];
        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 7; j++)
                mapArray[i, j] = 'r';
        // ここまでを書き直す
        return mapArray;
    }
}

/// <summary>マップ</summary>
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
