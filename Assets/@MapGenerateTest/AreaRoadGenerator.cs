using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// それぞれの区域に道路を生成する
/// </summary>
public class AreaRoadGenerator
{
    /// <summary>通常の道路</summary>
    readonly char Road = 'r';
    /// <summary>幅の広い道路</summary>
    readonly char WRoad = 'R';
    /// <summary>何も無し</summary>
    readonly char Non = 'n';
    /// <summary>幅の広い道路の量</summary>
    readonly int WRoadAmount = 6;

    // 辺上の区域と内側の区域を分けておく
    List<(int, int)> _edgeAreaList = new List<(int, int)>();
    List<(int, int)> _innerAreaList = new List<(int, int)>();

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>区域に道路を生成する</summary>
    public void Generate(Area[,] areas)
    {
        // 全ての区域の中央に道路を生成する
        for (int z = 1; z < Map.Height - 1; z++)
            for (int x = 1; x < Map.Width - 1; x++)
                ExtendRoadToDir(areas[z, x], Road, 5);

        // マップの角の区域の道路を中央から伸ばす
        ExtendRoadToDir(areas[0, 0], WRoad, 2, 5, 6);
        ExtendRoadToDir(areas[0, Map.Width - 1], WRoad, 2, 5, 4);
        ExtendRoadToDir(areas[Map.Height - 1, 0], WRoad, 6, 5, 8);
        ExtendRoadToDir(areas[Map.Height - 1, Map.Width - 1], WRoad, 4, 5, 8);

        // マップの左右の区域の道路を中央から伸ばす
        for (int z = 1; z < Map.Height - 1; z++)
        {
            ExtendRoadToDir(areas[z, 0], WRoad, 2, 5, 8);
            ExtendRoadToDir(areas[z, 0], Road, 6);
            ExtendRoadToDir(areas[z, Map.Width - 1], WRoad, 2, 5, 8);
            ExtendRoadToDir(areas[z, Map.Width - 1], Road, 4);
            _edgeAreaList.Add((z, 0));
            _edgeAreaList.Add((z, Map.Width - 1));
        }

        // マップの上下の区域の道路を中央から伸ばす
        for (int x = 1; x < Map.Width - 1; x++)
        {
            ExtendRoadToDir(areas[0, x], WRoad, 4, 5, 6);
            ExtendRoadToDir(areas[0, x], Road, 2);
            ExtendRoadToDir(areas[Map.Height - 1, x], WRoad, 4, 5, 6);
            ExtendRoadToDir(areas[Map.Height - 1, x], Road, 8);
            _edgeAreaList.Add((0, x));
            _edgeAreaList.Add((Map.Height - 1, x));
        }

        // マップの内側区域の道路を中央から伸ばす
        for (int z = 1; z < Map.Height - 1; z++)
            for (int x = 1; x < Map.Width - 1; x++)
            {
                ExtendRoadToDir(areas[z, x], Road, 2, 4, 6, 8);
                _innerAreaList.Add((z, x));
            }


        // 内側の区域をランダムにカットする
        foreach ((int z, int x) iPos in _innerAreaList.OrderBy(_ => System.Guid.NewGuid()))
        {
            List<int> dirList = new List<int>() { 2, 4, 6, 8 };
            for (int i = 0; i < 4; i++)
            {
                int dir = dirList[Random.Range(0, dirList.Count)];
                int reverseDir = 10 - dir;
                (int z,int x) next = GetDirTuple(dir);

                // 選ばれた区域と隣の区域が4方向に接続されていれば区域間の道路を消す
                if (areas[iPos.z, iPos.x].GetExtendCount() == 4 &&
                    areas[iPos.z + next.z, iPos.x + next.x].GetExtendCount() == 4)
                {
                    ExtendRoadToDir(areas[iPos.z, iPos.x], Non, dir);
                    ExtendRoadToDir(areas[iPos.z + next.z, iPos.x + next.x], Non, reverseDir);

                    // 消した先が辺上の点なら次の操作をする際に省きたいのでリストから削除する
                    if (_edgeAreaList.Contains((iPos.z + next.z, iPos.x + next.x)))
                        _edgeAreaList.Remove((iPos.z + next.z, iPos.x + next.x));
                }

                dirList.Remove(dir);
            }
        }

        // 道路をランダムに太い道路に変更する
        // ある程度太い道路が出来るまで繰り返す
        int count = 0;
        while (count < WRoadAmount)
        {
            // ランダムな辺上の区域からその区域の座標に応じた方向に道路を太くしていく
            int r = Random.Range(0, _edgeAreaList.Count);
            (int z, int x) ePos = _edgeAreaList[r];

            int dir = 0;
            if      (ePos.z == 0)              dir = 2;
            else if (ePos.z == Map.Height - 1) dir = 8;
            else if (ePos.x == 0)              dir = 6;
            else if (ePos.x == Map.Width - 1)  dir = 4;

            ChangeToWideRoad(areas, ePos, dir, ref count);
            _edgeAreaList.Remove(ePos);
        }
    }

    /// <summary>始点から別の辺上の座標まで道路を太い道路に変更する</summary>
    void ChangeToWideRoad(Area[,] areas, (int z, int x) pos, int dir, ref int count)
    {
        // 太い道路の数が極端に少なくならないよう処理したらカウントを増やす
        count++;
        
        (int z, int x) next = GetDirTuple(dir);
        // 隣のマスの逆方向も太い道路にするので反対方向も取得する
        int reverseDir = 10 - dir;
        ExtendRoadToDir(areas[pos.z, pos.x], WRoad, dir);
        ExtendRoadToDir(areas[pos.z + next.z, pos.x + next.x], WRoad, reverseDir, 5);
        
        // 隣の区域を次の基準にし、現在の方向に道路があるかチェックする
        pos = (pos.z + next.z, pos.x + next.x);
        if (areas[pos.z, pos.x].CheckExtendToDir(dir))
        {
            ChangeToWideRoad(areas, pos, dir, ref count);
        }
        else
        {
            // 道路がない場合は方向を変えるので辺に到達しているかチェックする
            if (pos.z == 0 || pos.z == Map.Height - 1 ||
                pos.x == 0 || pos.x == Map.Width - 1) 
                return;

            // 上下の場合は左右に、左右の場合は上下に進む方向を変える
            switch (dir)
            {
                case 2: dir = 4; break;
                case 4: dir = 2; break;
                case 6: dir = 8; break;
                case 8: dir = 6; break;
            }

            ChangeToWideRoad(areas, pos, dir, ref count);
        }
    }

    /// <summary>指定された方向に道路を伸ばす</summary>
    void ExtendRoadToDir(Area area, char c, params int[] dir)
    {
        for (int i = 0; i < dir.Length; i++)
            area.GetSectionFromNumKey(dir[i]).Fill(c);
    }

    /// <summary>その方向のint型のペアを返す</summary>
    (int, int) GetDirTuple(int dir)
    {
        if      (dir == 2)      return (1, 0);
        else if (dir == 4)      return (0, -1);
        else if (dir == 6)      return (0, 1);
        else    /* dir == 8 */  return (-1, 0);
    }
}
