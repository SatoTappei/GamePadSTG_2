using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>区域内の道路</summary>
public struct Road
{
    GameObject _object;
}

/// <summary>区域内の建物</summary>
public struct Build
{
    GameObject _object;
}

public struct Area
{
    //Road _road;
    //Build _build;
    public string[,] _roadStrs;

    // 7*7のマスに道路を生成する
    // 空いたマスに建物を生成する

    // 幅が1マスの道路の場合は3*3の空きがある
    // 幅が2マスの道路の場合は2.5*2.5の空きがある
}

/// <summary>
/// 渡された文字列を建物に変換してマップにする
/// </summary>
public class ObjectConverter : MonoBehaviour
{
    /// <summary>マップに配置するオブジェクト</summary>
    [System.Serializable]
    public class Building
    {
        public char _char;
        public GameObject _object;
    }   

    /// <summary>
    /// マップはただ1つしかないのでstaticクラスにする
    /// 道路レイヤーと建物レイヤーからなる
    /// </summary>
    public static class Map
    {
        /// <summary>区域の二次元配列</summary>
        public static Area[,] _areas = new Area[MapWidth, MapHeight];

    }

    AreaGenerator _areaGenerator;
    /// <summary>マップ上に設置する建築物のリスト</summary>
    [SerializeField] List<Building> _buildingList;
    /// <summary>建築物を検索する用の辞書型</summary>
    Dictionary<char, Building> _buildingDic = new Dictionary<char, Building>();

    // マップの大きさは奇数じゃないと区域を綺麗に並べることが出来ない
    // 大きすぎると負荷がすごい(かも)ので最大でも5*5にしておく
    static readonly int MapWidth = 5;
    static readonly int MapHeight = 5;

    void Awake()
    {
        _areaGenerator = GetComponent<AreaGenerator>();
        _buildingList.ForEach(b => _buildingDic.Add(b._char, b));
    }

    void Start()
    {
        // 各区域を並べることででかいマップにする
        // まずは区域に道路を生成する、道路には2種類(幅1マス、幅2マス)ある
        // 建物は幅2マスを考慮して作る
        // 区域の二次元配列を作成する二次元配列の二次元配列

        // 区域を生成して二次元配列に格納する
        _areaGenerator.Generate(Map._areas);

        //// 区域型の二次元配列を作成する
        //Area[,] map = new Area[MapWidth, MapHeight];

        for (int i = 0; i < MapWidth; i++)
            for (int j = 0; j < MapHeight; j++)
            {
                // 文字列型の二次元配列から区域を生成する
                GameObject areaRoot = BuildingFromArray(Map._areas[i,j]._roadStrs);
                areaRoot.transform.position = new Vector3(i * 7, 0, j * 7);
            }
    }

    void Update()
    {
        
    }

    /// <summary>文字列型の二次元配列から建築物を生成して、区域として返す</summary>
    GameObject BuildingFromArray(string[,] strMap)
    {
        // 生成した建築物を区域として設定する
        GameObject root = new GameObject();
        root.name = "AreaRoot";

        for (int i = 0; i < strMap.GetLength(0); i++)
            for (int j = 0; j < strMap.GetLength(1); j++)
            {
                char key = strMap[i, j][0];
                bool isExist = _buildingDic.TryGetValue(key, out Building value);

                // 対応する文字があれば生成する
                if (isExist)
                {
                    // 区域の中央と区域内の真ん中のオブジェクトの位置を合わせるためオフセットを足す
                    int offsetX =  -1 * (MapWidth / 2) - 1;
                    int offsetY = -1 * (MapHeight / 2) - 1;
                    GameObject go = Instantiate(value._object, new Vector3(i + offsetX, 0, j + offsetY), Quaternion.identity);
                    go.transform.SetParent(root.transform);
                }
            }

        return root;
    }
}
