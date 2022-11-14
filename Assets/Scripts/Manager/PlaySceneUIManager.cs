using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// �Q�[���{�҂�UI���Ǘ�����
/// </summary>
public class PlaySceneUIManager : MonoBehaviour
{
    [SerializeField] GameStartStag _gsStag;
    [SerializeField] GameOverStag _goStag;
    [SerializeField] GameClearStag _gcStag;
    [SerializeField] Timer _timer;
    [SerializeField] TargetView _targetView;
    /// <summary>�_���[�W���󂯂������������Q�[�W</summary>
    [SerializeField] Transform _damageGauge;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>HP�Q�[�W��ω�������</summary>
    public void SetLifeGauge(int max, int current)
    {
        float xValue = 1 - (current * 1.0f / max * 1.0f);
        xValue = Mathf.Clamp01(xValue);
        _damageGauge.DOScaleX(xValue, 0.5f);
    }

    /// <summary>�^�[�Q�b�g�r���[�̒l��ύX����</summary>
    public void SetTargetView(int count, Sprite icon)
    {
        if(_targetView != null) _targetView.Set(count, icon);
    }

    /// <summary>�Q�[���J�n���̉��o���s��</summary>
    public IEnumerator PlayGameStartStag()
    {
        yield return _gsStag.Play();
    }

    /// <summary>�Q�[���I�[�o�[���̉��o���s��</summary>
    public void PlayGameOverStag()
    {
        if (_goStag != null) _goStag.Play();
    }

    /// <summary>�N���A�^�C����n���ăQ�[���N���A�̉��o���s��</summary>
    public void PlayGameClearStag(int time)
    {
        if (_gcStag != null) _gcStag.Play(time);
    }

    /// <summary>�R�[���o�b�N��n���ă^�C�}�[���X�^�[�g������</summary>
    public void TimerStart(UnityAction action = null)
    {
        _timer.TimeUpEvent += action;
        _timer.TimerStart();
    }

    /// <summary>�^�C�}�[���~������</summary>
    public void TimerPause() => _timer.TimerPause();

    /// <summary>�^�C�}�[�̌o�ߎ��Ԃ��擾����</summary>
    public int GetTimerCount() => _timer.GetCount();
}
