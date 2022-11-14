using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// �J�E���g��0�ɂȂ����Ƃ��ɃR�[���o�b�N���Ă΂��^�C�}�[
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField] Text _counter;
    /// <summary>��������(��)</summary>
    [SerializeField] int _limitMinutes;

    /// <summary>�^�C���A�b�v���ɌĂ΂��C�x���g</summary>
    public UnityAction TimeUpEvent { get; set; }
    /// <summary>true�̊Ԃ̓^�C�}�[���~�܂�</summary>
    bool _isPause = true;

    float _count;

    void Awake()
    {
        _count = _limitMinutes * 60;
        ToText();
    }

    void Start()
    {

    }

    void Update()
    {
        // �f�o�b�O�p
        //if (Input.GetKeyDown(KeyCode.T)) IsPause = !IsPause;
     
        if (_isPause) return;

        _count -= Time.deltaTime;
        if (ToText() == 0)
        {
            TimeUpEvent?.Invoke();
            TimeUpEvent = null;
            _isPause = true;
        }
    }

    /// <summary>�^�C�}�[�̃J�E���g���J�n����A���ڈȍ~�ɌĂяo�����ꍇ�͍ĊJ�ɂȂ�</summary>
    public void TimerStart() => _isPause = false;
    
    /// <summary>�^�C�}�[�̃J�E���g���~�߂�</summary>
    public void TimerPause() => _isPause = true;

    /// <summary>���݂̃^�C�}�[�̌o�ߎ��Ԃ�Ԃ�</summary>
    public int GetCount() => _limitMinutes * 60 - (int)_count;

    /// <summary>�^�C�}�[�`���̕�����ɂ��ĕԂ�</summary>
    public static string Convert(int count)
    {
        TimeSpan ts = new TimeSpan(0, 0, count);
        return ts.ToString(@"mm\:ss");
    }

    /// <summary>��:�b�̌`�Ńe�L�X�g�ɕ\������int�^�ɃL���X�g����Count��Ԃ�</summary>
    int ToText()
    {
        int iCount = (int)_count;
        _counter.text = Convert(iCount);
        return iCount;
    }
}
