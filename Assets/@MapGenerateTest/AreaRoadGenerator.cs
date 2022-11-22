using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ���ꂼ��̋��ɓ��H�𐶐�����
/// </summary>
public class AreaRoadGenerator
{
    /// <summary>�ʏ�̓��H</summary>
    readonly char Road = 'r';
    /// <summary>���̍L�����H</summary>
    readonly char WRoad = 'R';
    /// <summary>��������</summary>
    readonly char Non = 'n';
    /// <summary>���̍L�����H�̗�</summary>
    readonly int WRoadAmount = 6;

    // �ӏ�̋��Ɠ����̋��𕪂��Ă���
    List<(int, int)> _edgeAreaList = new List<(int, int)>();
    List<(int, int)> _innerAreaList = new List<(int, int)>();

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>���ɓ��H�𐶐�����</summary>
    public void Generate(Area[,] areas)
    {
        // �S�Ă̋��̒����ɓ��H�𐶐�����
        for (int z = 1; z < Map.Height - 1; z++)
            for (int x = 1; x < Map.Width - 1; x++)
                ExtendRoadToDir(areas[z, x], Road, 5);

        // �}�b�v�̊p�̋��̓��H�𒆉�����L�΂�
        ExtendRoadToDir(areas[0, 0], WRoad, 2, 5, 6);
        ExtendRoadToDir(areas[0, Map.Width - 1], WRoad, 2, 5, 4);
        ExtendRoadToDir(areas[Map.Height - 1, 0], WRoad, 6, 5, 8);
        ExtendRoadToDir(areas[Map.Height - 1, Map.Width - 1], WRoad, 4, 5, 8);

        // �}�b�v�̍��E�̋��̓��H�𒆉�����L�΂�
        for (int z = 1; z < Map.Height - 1; z++)
        {
            ExtendRoadToDir(areas[z, 0], WRoad, 2, 5, 8);
            ExtendRoadToDir(areas[z, 0], Road, 6);
            ExtendRoadToDir(areas[z, Map.Width - 1], WRoad, 2, 5, 8);
            ExtendRoadToDir(areas[z, Map.Width - 1], Road, 4);
            _edgeAreaList.Add((z, 0));
            _edgeAreaList.Add((z, Map.Width - 1));
        }

        // �}�b�v�̏㉺�̋��̓��H�𒆉�����L�΂�
        for (int x = 1; x < Map.Width - 1; x++)
        {
            ExtendRoadToDir(areas[0, x], WRoad, 4, 5, 6);
            ExtendRoadToDir(areas[0, x], Road, 2);
            ExtendRoadToDir(areas[Map.Height - 1, x], WRoad, 4, 5, 6);
            ExtendRoadToDir(areas[Map.Height - 1, x], Road, 8);
            _edgeAreaList.Add((0, x));
            _edgeAreaList.Add((Map.Height - 1, x));
        }

        // �}�b�v�̓������̓��H�𒆉�����L�΂�
        for (int z = 1; z < Map.Height - 1; z++)
            for (int x = 1; x < Map.Width - 1; x++)
            {
                ExtendRoadToDir(areas[z, x], Road, 2, 4, 6, 8);
                _innerAreaList.Add((z, x));
            }


        // �����̋��������_���ɃJ�b�g����
        foreach ((int z, int x) iPos in _innerAreaList.OrderBy(_ => System.Guid.NewGuid()))
        {
            List<int> dirList = new List<int>() { 2, 4, 6, 8 };
            for (int i = 0; i < 4; i++)
            {
                int dir = dirList[Random.Range(0, dirList.Count)];
                int reverseDir = 10 - dir;
                (int z,int x) next = GetDirTuple(dir);

                // �I�΂ꂽ���Ɨׂ̋�悪4�����ɐڑ�����Ă���΋��Ԃ̓��H������
                if (areas[iPos.z, iPos.x].GetExtendCount() == 4 &&
                    areas[iPos.z + next.z, iPos.x + next.x].GetExtendCount() == 4)
                {
                    ExtendRoadToDir(areas[iPos.z, iPos.x], Non, dir);
                    ExtendRoadToDir(areas[iPos.z + next.z, iPos.x + next.x], Non, reverseDir);

                    // �������悪�ӏ�̓_�Ȃ玟�̑��������ۂɏȂ������̂Ń��X�g����폜����
                    if (_edgeAreaList.Contains((iPos.z + next.z, iPos.x + next.x)))
                        _edgeAreaList.Remove((iPos.z + next.z, iPos.x + next.x));
                }

                dirList.Remove(dir);
            }
        }

        // ���H�������_���ɑ������H�ɕύX����
        // ������x�������H���o����܂ŌJ��Ԃ�
        int count = 0;
        while (count < WRoadAmount)
        {
            // �����_���ȕӏ�̋�悩�炻�̋��̍��W�ɉ����������ɓ��H�𑾂����Ă���
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

    /// <summary>�n�_����ʂ̕ӏ�̍��W�܂œ��H�𑾂����H�ɕύX����</summary>
    void ChangeToWideRoad(Area[,] areas, (int z, int x) pos, int dir, ref int count)
    {
        // �������H�̐����ɒ[�ɏ��Ȃ��Ȃ�Ȃ��悤����������J�E���g�𑝂₷
        count++;
        
        (int z, int x) next = GetDirTuple(dir);
        // �ׂ̃}�X�̋t�������������H�ɂ���̂Ŕ��Ε������擾����
        int reverseDir = 10 - dir;
        ExtendRoadToDir(areas[pos.z, pos.x], WRoad, dir);
        ExtendRoadToDir(areas[pos.z + next.z, pos.x + next.x], WRoad, reverseDir, 5);
        
        // �ׂ̋������̊�ɂ��A���݂̕����ɓ��H�����邩�`�F�b�N����
        pos = (pos.z + next.z, pos.x + next.x);
        if (areas[pos.z, pos.x].CheckExtendToDir(dir))
        {
            ChangeToWideRoad(areas, pos, dir, ref count);
        }
        else
        {
            // ���H���Ȃ��ꍇ�͕�����ς���̂ŕӂɓ��B���Ă��邩�`�F�b�N����
            if (pos.z == 0 || pos.z == Map.Height - 1 ||
                pos.x == 0 || pos.x == Map.Width - 1) 
                return;

            // �㉺�̏ꍇ�͍��E�ɁA���E�̏ꍇ�͏㉺�ɐi�ޕ�����ς���
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

    /// <summary>�w�肳�ꂽ�����ɓ��H��L�΂�</summary>
    void ExtendRoadToDir(Area area, char c, params int[] dir)
    {
        for (int i = 0; i < dir.Length; i++)
            area.GetSectionFromNumKey(dir[i]).Fill(c);
    }

    /// <summary>���̕�����int�^�̃y�A��Ԃ�</summary>
    (int, int) GetDirTuple(int dir)
    {
        if      (dir == 2)      return (1, 0);
        else if (dir == 4)      return (0, -1);
        else if (dir == 6)      return (0, 1);
        else    /* dir == 8 */  return (-1, 0);
    }
}
