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

    AreaRoadGenerator _areaGenerator;
    BuildingGenerator _buildingGenerator;
    PropGenerator _propGenerator;
    /// <summary>�}�b�v��ɐݒu���錚�z���̃��X�g</summary>
    [SerializeField] List<Building> _buildingList;
    /// <summary>���������X�e�[�W�̐e</summary>
    [SerializeField] Transform _parent;
    /// <summary>���z������������p�̎����^</summary>
    Dictionary<char, Building> _buildingDic = new Dictionary<char, Building>();

    //static readonly int MapHeight = MapGenerateUtility.MapHeight;
    //static readonly int MapWidth = MapGenerateUtility.MapWidth;
    //static readonly int AreaWide = MapGenerateUtility.AreaWide;

    void Awake()
    {
        _areaGenerator = new AreaRoadGenerator();
        _buildingGenerator = new BuildingGenerator();
        _propGenerator = new PropGenerator();
        _buildingList.ForEach(b => _buildingDic.Add(b._char, b));
    }

    void Start()
    {
        /* �}�b�v�̐����������������珑�� */

        // �����ƕ����w�肵�ă}�b�v���쐬
        Map map = new Map(Map.Height, Map.Width);
        _areaGenerator.Generate(map.Areas);
        _buildingGenerator.Generate(map.Areas);
        _propGenerator.Generate(map.Props, map.Areas);
        /* �}�b�v�̐������������܂� */

        // �����^�̓񎟌��z�񂩂���𐶐�����
        for (int z = 0; z < Map.Height; z++)
            for (int x = 0; x < Map.Width; x++)
            {
                GameObject areaRoot = BuildingFromArray(map.Areas[z, x].GetCharArray());
                GameObject propRoot = BuildingFromArray(map.Props[z, x].GetCharArray(), "PropRoot");
                areaRoot.transform.position = new Vector3(z * Area.Wide * 4, 0, x * Area.Wide * 4);
                propRoot.transform.position = new Vector3(z * Area.Wide * 4, 1, x * Area.Wide * 4);
                areaRoot.transform.SetParent(_parent);
                propRoot.transform.SetParent(_parent);
            }
    }

    void Update()
    {
        
    }

    /// <summary>������^�̓񎟌��z�񂩂猚�z���𐶐����āA���Ƃ��ĕԂ�</summary>
    GameObject BuildingFromArray(char[,] strMap, string name = "AreaRoot")
    {
        // �����������z�������Ƃ��Đݒ肷��
        GameObject root = new GameObject();
        root.name = name;

        for (int i = 0; i < strMap.GetLength(0); i++)
            for (int j = 0; j < strMap.GetLength(1); j++)
            {
                char key = strMap[i, j];
                bool isExist = _buildingDic.TryGetValue(key, out Building value);

                // �Ή����镶��������ΐ�������
                if (isExist)
                {
                    // ���̒����Ƌ����̐^�񒆂̃I�u�W�F�N�g�̈ʒu�����킹�邽�߃I�t�Z�b�g�𑫂�
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
}