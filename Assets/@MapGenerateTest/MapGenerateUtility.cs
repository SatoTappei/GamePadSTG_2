using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// マップ上の1マス
/// </summary>
public class Mass
{
    /// <summary>区域上での座標、区画内での番号とは違うので注意</summary>
    public readonly (int z, int x) Pos;
    char _char;

    public Mass(int z, int x, char c)
    {
        Pos.z = z;
        Pos.x = x;
        _char = c;
    }

    public char Char { get => _char; set => _char = value; }

    /// <summary>デフォルトで割り当てられる文字</summary>
    public static readonly char Default = 'n';
}

/// <summary>
/// 区域を構成する区画
/// </summary>
public class Section
{
    // 区画の左上と右下を保存する => 各マスに割り当てるため
    public readonly int Height;
    public readonly int Width;
    readonly (int z, int x) _upperLeft;
    readonly (int z, int x) _bottomRight;
    Mass[,] _masses;

    public Section(int height, int width, (int z, int x) upperLeft, (int z, int x) bottomRight)
    {
        _masses = new Mass[height, width];
        Height = height;
        Width = width;
        _upperLeft = upperLeft;
        _bottomRight = bottomRight;

        for (int z = 0, areaZ = _upperLeft.z; z < height; z++, areaZ++)
            for (int x = 0, areaX = _upperLeft.x; x < width; x++, areaX++)
            {
                _masses[z, x] = new Mass(areaZ, areaX, Mass.Default);
            }
    }

    public Mass[,] Masses { get => _masses; }

    /// <summary>区画内の番号を渡すと対応した対応したマスを返す</summary>
    public Mass GetMass(int z, int x) => _masses[z, x];

    /// <summary>この区画を文字の二次元配列にして返す。</summary>
    public char[,] GetCharArray()
    {
        char[,] array = new char[Height, Width];
        for (int z = 0; z < Height; z++)
            for (int x = 0; x < Width; x++)
            {
                array[z, x] = _masses[z, x].Char;
            }

        return array;
    }

    /// <summary>区画を渡された文字で埋める</summary>
    public void Fill(char c)
    {
        for (int z = 0; z < Height; z++)
            for (int x = 0; x < Width; x++)
            {
                _masses[z, x].Char = c;
            }
    }

    /// <summary>文字の二次元配列を区画に反映させる</summary>
    public void SetCharArray(char[,] array)
    {
        if (array.GetLength(0) != Height || array.GetLength(1) != Width)
        {
            Debug.LogWarning("渡された配列が区画の大きさと違います。");
            return;
        }

        for (int z = 0; z < Height; z++)
            for (int x = 0; x < Width; x++)
            {
                _masses[z, x].Char = array[z, x];
            }
    }
}

/// <summary>
/// マップを構成する区域
/// </summary>
public class Area
{
    public static int Wide = 7;

    /// <summary>各区画はテンキーの番号に対応している</summary>
    Section[] _sections;

    public Area()
    {
        _sections = new Section[]
        {
            new Section(3,3,(4,0),(6,2)),   // 左下
            new Section(3,1,(4,3),(6,3)),   // 下
            new Section(3,3,(4,4),(6,6)),   // 右下
            new Section(1,3,(3,0),(3,2)),   // 左
            new Section(1,1,(3,3),(3,3)),   // 真ん中
            new Section(1,3,(3,4),(3,6)),   // 右
            new Section(3,3,(0,0),(2,2)),   // 左上
            new Section(3,1,(0,3),(2,3)),   // 上
            new Section(3,3,(0,4),(0,6)),   // 右上
         };
    }

    /// <summary>テンキーに対応した区画を返す</summary>
    public Section GetSectionFromNumKey(int numKey)
    {
        if (numKey < 1 || 9 < numKey)
        {
            Debug.LogError("区画番号は1~9です: " + numKey);
            return null;
        }

        return _sections[numKey - 1];
    }

    /// <summary>渡された方向に道路を伸ばしているかチェックする</summary>
    public bool CheckExtendToDir(int dir)
    {
        if (dir % 2 == 1 || dir == 0)
        {
            Debug.LogWarning("チェックする方向は上下左右の中から選んでください。");
            return false;
        }

        // 道路になっている区画は途中で途切れることがないので先頭だけ調べればよい。
        Mass mass = GetSectionFromNumKey(dir).GetMass(0, 0);

        return mass.Char == 'r' || mass.Char == 'R';
    }

    /// <summary>道路を伸ばしている数を返す</summary>
    public int GetExtendCount()
    {
        int count = 0;
        // 道路は途中で途切れることがないので
        // 上下左右の区画の先頭のマスが道路になっているか調べればよい
        for (int i = 2; i <= 8; i += 2)
        {
            Mass mass = GetSectionFromNumKey(i).GetMass(0, 0);
            
            if (mass.Char != 'n') count++;
        }

        return count;
    }

    /// <summary>各区画を合体させて1つの文字型の二次元配列にして返す</summary>
    public char[,] GetCharArray()
    {
        char[,] mapArray = new char[Wide, Wide];
        foreach (Section sct in _sections)
        {
            for (int z = 0; z < sct.Height; z++)
                for (int x = 0; x < sct.Width; x++)
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
    public static readonly int Height = 5;
    public static readonly int Width = 5;

    public Area[,] Areas { get; set; }
    public Area[,] Props { get; set; }

    public Map(int height, int width)
    {
        Areas = new Area[height, width];

        for (int z = 0; z < height; z++)
            for (int x = 0; x < width; x++)
                Areas[z, x] = new Area();

        Props = new Area[height, width];

        for (int z = 0; z < height; z++)
            for (int x = 0; x < width; x++)
                Props[z, x] = new Area();
    }
}
