using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �n���ꂽ������������ɕϊ����ă}�b�v�ɂ���
/// </summary>
public class ObjectConverter : MonoBehaviour
{
    /// <summary>�}�b�v�ɔz�u����I�u�W�F�N�g</summary>
    [System.Serializable]
    public class Building
    {
        public char _char;
        public GameObject _object;
    }   

    AreaGenerator _areaGenerator;
    BuildingGenerator _buildingGenerator;
    /// <summary>�}�b�v��ɐݒu���錚�z���̃��X�g</summary>
    [SerializeField] List<Building> _buildingList;
    /// <summary>���z������������p�̎����^</summary>
    Dictionary<char, Building> _buildingDic = new Dictionary<char, Building>();

    static readonly int MapHeight = MapGenerateUtility.MapHeight;
    static readonly int MapWidth = MapGenerateUtility.MapWidth;
    static readonly int AreaWide = MapGenerateUtility.AreaWide;

    void Awake()
    {
        _areaGenerator = GetComponent<AreaGenerator>();
        _buildingGenerator = GetComponent<BuildingGenerator>();
        _buildingList.ForEach(b => _buildingDic.Add(b._char, b));
    }

    void Start()
    {
        /* �}�b�v�̐����������������珑�� */

        // �����ƕ����w�肵�ă}�b�v���쐬
        Map map = new Map(MapHeight, MapWidth);
        _areaGenerator.Generate();

        /* �}�b�v�̐������������܂� */

        // �����^�̓񎟌��z�񂩂���𐶐�����
        for (int z = 0; z < MapHeight; z++)
            for (int x = 0; x < MapWidth; x++)
            {
                char[,] strMap = map.Areas[z, x].GetStringArray();
                GameObject areaRoot = BuildingFromArray(strMap);
                areaRoot.transform.position = new Vector3(z * AreaWide, 0, x * AreaWide);
            }
    }

    void Update()
    {
        
    }

    /// <summary>������^�̓񎟌��z�񂩂猚�z���𐶐����āA���Ƃ��ĕԂ�</summary>
    GameObject BuildingFromArray(char[,] strMap)
    {
        // �����������z�������Ƃ��Đݒ肷��
        GameObject root = new GameObject();
        root.name = "AreaRoot";

        for (int i = 0; i < strMap.GetLength(0); i++)
            for (int j = 0; j < strMap.GetLength(1); j++)
            {
                char key = strMap[i, j];
                bool isExist = _buildingDic.TryGetValue(key, out Building value);

                // �Ή����镶��������ΐ�������
                if (isExist)
                {
                    // ���̒����Ƌ����̐^�񒆂̃I�u�W�F�N�g�̈ʒu�����킹�邽�߃I�t�Z�b�g�𑫂�
                    int offsetX =  -1 * (MapWidth / 2) - 1;
                    int offsetY = -1 * (MapHeight / 2) - 1;
                    GameObject go = Instantiate(value._object, new Vector3(i + offsetX, 0, j + offsetY), Quaternion.identity);
                    go.transform.SetParent(root.transform);
                }
            }

        return root;
    }
}
