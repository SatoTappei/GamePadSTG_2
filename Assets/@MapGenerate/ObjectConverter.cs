using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    AreaRoadGenerator _areaGenerator;
    BuildingGenerator _buildingGenerator;
    PropGenerator _propGenerator;
    EnemyGenerator _enemyGenerator;
    /// <summary>マップ上に設置する建築物のリスト</summary>
    [SerializeField] List<Building> _buildingList;
    /// <summary>生成したステージの親</summary>
    [SerializeField] Transform _parent;
    /// <summary>マップ上に配置する敵のリスト</summary>
    [SerializeField] List<Building> _enemyList;
    /// <summary>建築物を検索する用の辞書型</summary>
    Dictionary<char, Building> _buildingDic = new Dictionary<char, Building>();
    /// <summary>敵を検索する用の辞書型</summary>
    Dictionary<char, Building> _enemyDic = new Dictionary<char, Building>();

    //static readonly int MapHeight = MapGenerateUtility.MapHeight;
    //static readonly int MapWidth = MapGenerateUtility.MapWidth;
    //static readonly int AreaWide = MapGenerateUtility.AreaWide;

    void Awake()
    {
        _areaGenerator = new AreaRoadGenerator();
        _buildingGenerator = new BuildingGenerator();
        _propGenerator = new PropGenerator();
        _enemyGenerator = new EnemyGenerator();
        _buildingList.ForEach(b => _buildingDic.Add(b._char, b));
        _enemyList.ForEach(e => _enemyDic.Add(e._char, e));
    }

    void Start()
    {
        /* マップの生成処理をここから書く */

        // 高さと幅を指定してマップを作成
        Map map = new Map(Map.Height, Map.Width);
        _areaGenerator.Generate(map.Areas);
        //_buildingGenerator.Generate(map.Areas);
        _propGenerator.Generate(map.Props, map.Areas);
        _enemyGenerator.Generate(map.Enemies, map.Areas);
        /* マップの生成処理ここまで */

        // 文字型の二次元配列から区域を生成する
        for (int z = 0; z < Map.Height; z++)
            for (int x = 0; x < Map.Width; x++)
            {
                GameObject areaRoot = BuildingFromArray(map.Areas[z, x].GetCharArray());
                GameObject propRoot = BuildingFromArray(map.Props[z, x].GetCharArray(), "PropRoot");
                GameObject enemyRoot = EnemiesFromArray(map.Enemies[z, x].GetCharArray());
                areaRoot.transform.position = new Vector3(z * Area.Wide * 4, 0, x * Area.Wide * 4);
                propRoot.transform.position = new Vector3(z * Area.Wide * 4, 1, x * Area.Wide * 4);
                enemyRoot.transform.position = new Vector3(z * Area.Wide * 4, 1, x * Area.Wide * 4);
                areaRoot.transform.SetParent(_parent);
                propRoot.transform.SetParent(_parent);
                enemyRoot.transform.SetParent(_parent);
            }
    }

    void Update()
    {
        
    }

    /// <summary>文字列型の二次元配列から建築物を生成して、区域として返す</summary>
    GameObject BuildingFromArray(char[,] strMap, string name = "AreaRoot")
    {
        // 生成した建築物を区域として設定する
        GameObject root = new GameObject();
        root.name = name;
        root.isStatic = true;

        for (int i = 0; i < strMap.GetLength(0); i++)
            for (int j = 0; j < strMap.GetLength(1); j++)
            {
                char key = strMap[i, j];
                bool isExist = _buildingDic.TryGetValue(key, out Building value);

                // 対応する文字があれば生成する
                if (isExist)
                {
                    // 区域の中央と区域内の真ん中のオブジェクトの位置を合わせるためオフセットを足す
                    int offsetZ = -1 * (Map.Height / 2) - 15;
                    int offsetX =  -1 * (Map.Width / 2) - 15;
                    Vector3 pos = new Vector3(i + offsetX, 0, j + offsetZ);
                    Quaternion rot = Quaternion.identity;
                    Transform parent = root.transform;

                    Instantiate(value._object, pos, rot, parent);
                }
            }

        root.transform.localScale = Vector3.one * 4;

        return root;
    }

    /// <summary>文字列型の二次元配列から敵を生成して返す</summary>
    GameObject EnemiesFromArray(char[,] strMap, string name = "Enemies")
    {
        GameObject root = new GameObject();
        root.name = name;

        for (int i = 0; i < strMap.GetLength(0); i++)
            for (int j = 0; j < strMap.GetLength(1); j++)
            {
                char key = strMap[i, j];
                bool isExist = _enemyDic.TryGetValue(key, out Building value);

                // 対応する文字があれば生成する
                if (isExist)
                {
                    int offsetZ = -1 * (Map.Height / 2) - 15;
                    int offsetX = -1 * (Map.Width / 2) - 15;
                    Vector3 pos = new Vector3(i + offsetX, 0, j + offsetZ);
                    Quaternion rot = Quaternion.identity;
                    Transform parent = root.transform;

                    GameObject go = Instantiate(value._object, pos, rot, parent);
                    go.transform.localScale = Vector3.one * 0.25f;
                }
            }

        root.transform.localScale = Vector3.one * 4;

        return root;
    }
}
