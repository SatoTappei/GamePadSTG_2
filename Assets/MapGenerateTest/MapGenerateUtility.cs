using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mass
{
    int x;
    int z;
}

public class Area
{
    enum Section
    {
        Up,
        Down,
        Right,
        Left,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft,
    }

    public string[,] _roadStrs;

    /// <summary>方角毎に区画を分けて持つ</summary>
    Dictionary<Section, string[,]> sectionDic = new Dictionary<Section, string[,]>();
    /// <summary>道路沿いのマスのリスト</summary>
    List<Mass> byRoadList = new List<Mass>();

    // 7*7のマスに道路を生成する
    // 空いたマスに建物を生成する

    // 幅が1マスの道路の場合は3*3の空きがある
    // 幅が2マスの道路の場合は2.5*2.5の空きがある
}

/// <summary>
/// 複数のスクリプトで共通して使うもの
/// </summary>
public class MapGenerateUtility
{
    /// マップの1辺は奇数かつ負荷的に大丈夫な5で固定
    public static readonly int MapWidth = 5;
    public static readonly int MapHeight = 5;
    /// <summary>区域の一辺の幅、奇数でいい感じの値である7で固定</summary>
    public static readonly int AreaWide = 7;
}
