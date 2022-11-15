using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�X�e�[�g�}�V���̊��N���X�̊��N���X
/// </summary>
public abstract class StateMachineBase
{
    /// <summary>�X�e�[�g���̃C�x���g</summary>
    protected enum Event
    {
        Enter, Stay, Exit
    };

    protected Event _event;
    protected StateMachineBase _nextState;
    protected GameObject _character;
    protected Transform _target;
    protected Animator _anim;
    protected GameObject _findIcon;

    // �L�����N�^�[�̍��W���X�V����̂Ɏg�����C����̒萔
    protected readonly LayerMask Mask = 1 << 6;
    readonly float RayRadius = 0.01f;
    readonly float RayDistance = 10.0f;
    readonly Vector3 RayOffset = new Vector3(0.0f, 2.5f, 0.0f);

    // �U���Ɏg���萔�͒��ۃN���X�̎d�l��A�v���p�e�B�ł��������o���Ȃ�
    protected abstract float SightRange { get; }
    protected abstract float SightAngle { get; }
    protected abstract float AttackRange { get; }

    /// <summary>State�ɐ��ڂ����ہA1�x�����Ă΂��</summary>
    public virtual void Enter() => _event = Event.Stay;

    /// <summary>Enter���Ă΂ꂽ��AExit�ɂȂ�܂Ŗ��t���[���Ă΂��</summary>
    public virtual void Update() => _event = Event.Stay;

    /// <summary>����State�ɐ��ڂ���ہA1�x�����Ă΂��</summary>
    public virtual void Exit() => _event = Event.Exit;

    /// <summary>�^�[�Q�b�g�����A�C�R���̕\����؂�ւ���</summary>
    protected void ActiveFindIcon(bool value)
    {
        if (_findIcon != null)
            _findIcon.SetActive(value);
        // TODO: ����炷
    }

    /// <summary>
    /// ���݂̃C�x���g�ɑΉ��������\�b�h���Ăяo����
    /// ���t���[���ł̏�ԃN���X��Ԃ�
    /// </summary>
    public StateMachineBase Process()
    {
        if (_event == Event.Enter) Enter();
        else if (_event == Event.Stay) Update();
        else if (_event == Event.Exit)
        {
            Exit();
            return _nextState;
        }
        return this;
    }

    /// <summary>Y���W���Z�b�g����</summary>
    protected void SetCharacterPosY()
    {
        Vector3 rayPos = _character.transform.position + RayOffset;
        Ray ray = new Ray(rayPos, Vector3.down);
        if (Physics.SphereCast(ray, RayRadius, out RaycastHit hit, RayDistance, Mask))
        {
            Vector3 pos = _character.transform.position;
            pos.y = hit.point.y;
            _character.transform.position = pos;
        }
        else
        {
            // �������Ȃ�
        }
    }

    /// <summary>�^�[�Q�b�g�����E�ɓ����Ă��邩���肷��</summary>
    protected abstract bool FindTarget();

    /// <summary>�O�����瓮�������S�Ɏ~�߂�</summary>
    public abstract void ToCompleted();

    /// <summary>��Ԃ𐄈ڂ�����</summary>
    protected void ChangeState<T>(T next) where T : StateMachineBase
    {
        _nextState = next;
        _event = Event.Exit;
    }
}
