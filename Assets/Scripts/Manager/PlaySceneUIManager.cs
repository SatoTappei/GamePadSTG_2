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
    [SerializeField] Timer _timer;
    [SerializeField] TargetView _targetView;
    /// <summary>�_���[�W���󂯂������������Q�[�W</summary>
    [SerializeField] Transform _damageGauge;
    /// <summary>�X�R�A��\������e�L�X�g</summary>
    [SerializeField] Text _scoreText;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>HP�Q�[�W��ω�������</summary>
    public void SetLifeGauge(int max, int current)
    {
        Debug.Log($"�ő�l{max} ���ݒl{current}");
        float xValue = 1 - (current * 1.0f / max * 1.0f);
        Debug.Log("x�X�P�[�� " + xValue);
        xValue = Mathf.Clamp01(xValue);
        _damageGauge.DOScaleX(xValue, 0.5f);
    }

    /// <summary>�X�R�A���Z�b�g����</summary>
    public void SetScore(int score)
    {
        int prev = int.Parse(_scoreText.text.Replace(",", ""));
        _scoreText.DOCounter(prev, score, 0.5f);
    }

    /// <summary>�^�[�Q�b�g�r���[�̒l��ύX����</summary>
    public void SetTargetView(int count, Sprite icon) => _targetView.Set(count, icon);

    /// <summary>�Q�[���J�n���̉��o���s��</summary>
    public async UniTask PlayGameStartStag() => await _gsStag.Play();

    /// <summary>�R�[���o�b�N��n���ă^�C�}�[���X�^�[�g������</summary>
    public void TimerStart(UnityAction action = null)
    {
        _timer.TimeUpEvent += action;
        _timer.TimerStart();
    }
}
