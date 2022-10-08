using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using System;

/// <summary>
/// ���C�L���X�g��p�����v���C���[�̈ړ��̃e�X�g
/// </summary>
public class PlayerMoveRaycast : MonoBehaviour
{
    Camera _mainCamera;
    [SerializeField] LayerMask _layerMask;
    Vector3 _rayOffset = new Vector3(0.0f, 5.0f, 0.0f);
    float _radius = 0.1f;
    float _rayDistance = 10.0f;
    float _lastRaycastTime = 0.0f; // �Ō�Ƀ��C�L���X�g��������
    float _intervalRaycast = 0.1f; // ���C�L���X�g������Ԋu
    float _lastHitPositionY;       // �Ō�Ƀ��C�L���X�g�������̍���

    void Awake()
    {
        // ���C�L���X�g��p�����ړ��Ȃ̂ŃR���C�_�[�ƃ��W�b�h�{�f�B�̓I�t�ɂ���
        //GetComponent<Rigidbody>().Sleep();
        //GetComponent<CapsuleCollider>().enabled = false;
    }

    void Start()
    {
        _mainCamera = Camera.main;
        _lastHitPositionY = transform.position.y;
    }

    void Update()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Quaternion rot = Quaternion.AngleAxis(_mainCamera.transform.eulerAngles.y, Vector3.up);
        Vector3 inputVec = rot * new Vector3(hori, 0, vert).normalized;

        // ���`�⊮�𗘗p�����ړ��̃e�X�g(���s)
        Vector3 LerpPos = transform.position;
        LerpPos.y = Mathf.Lerp(LerpPos.y, _lastHitPositionY, Time.deltaTime * 10);
        transform.position = LerpPos;

        if (inputVec != Vector3.zero)
        {
            //MatchHeightByRaycast(inputVec);
            ConstantSlopeSpeedForRaycast(inputVec);
        }


    }

    // Ray���������ɕ����č��������킹��
    void MatchHeightByRaycast(Vector3 inputVec)
    {
        // �ړ������A������������Ȃ��̂�Update()���ōs��
        Vector3 moveVec = inputVec * Time.deltaTime * 6.0f;
        transform.position += moveVec;

        // ��莞��(_IntervalRaycast)�̊Ԋu���J���ă��C�L���X�g����
        if (Time.time < _lastRaycastTime + _intervalRaycast)
            return;

        // �ŏI���C�L���X�g���Ԃ��L�^
        _lastRaycastTime = Time.time;

        // ���g�̍��W�ɃI�t�Z�b�g�𑫂������W���牺������Ray�����
        Vector3 rayPos = transform.position + _rayOffset;
        Ray ray = new Ray(rayPos, Vector3.down);

        // �����Ray�����
        bool isHit = Physics.SphereCast(ray, _radius, out RaycastHit hit, _rayDistance, _layerMask);

        if (isHit)
        {
            // �Փ˂������W�ɍ��������킹��
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;

            _lastHitPositionY = hit.point.y;
        }
    }

    /// <summary>�Ζʂł����̑��x�ňړ�����悤�ɏC�����Ĉړ�����</summary>
    private void ConstantSlopeSpeedForRaycast(Vector3 inputVec)
    {
        // �ړ����x�A�e�X�g�Ȃ̂ōŏI�I�Ɏg�p����ꍇ�͂�����ƒ���
        int speed = 10;

        // �ړ��O�̈ʒu��ۑ�
        Vector3 currentPos = transform.position;
        // �ڕW���W�����߂�
        Vector3 moveDelta = inputVec * Time.deltaTime * speed;
        Vector3 targetPos = currentPos + moveDelta;
        targetPos.y = Mathf.Lerp(currentPos.y, _lastHitPositionY, Time.deltaTime * 10);
        // �ڕW���W���猻�ݍ��W�̃x�N�g���̍������߂�
        moveDelta = targetPos - currentPos;
        // normalize���āA1�t���[��������̃x�N�g�������߂�
        moveDelta = moveDelta.normalized * Time.deltaTime * speed;
        // �ڕW�̍�����␳
        _lastHitPositionY -= moveDelta.y;
        // �����ł͍����̈ړ��͍s��Ȃ��A���ʈړ�����
        moveDelta.y = 0;
        transform.position += moveDelta;

        // ��莞��(_IntervalRaycast)�̊Ԋu���J���ă��C�L���X�g����
        if (Time.time < _lastRaycastTime + _intervalRaycast)
            return;

        // �ŏI���C�L���X�g���Ԃ��L�^
        _lastRaycastTime = Time.time;

        // ���g�̍��W�ɃI�t�Z�b�g�𑫂������W���牺������Ray�����
        Vector3 rayPos = transform.position + _rayOffset;
        Ray ray = new Ray(rayPos, Vector3.down);

        // �����Ray�����
        bool isHit = Physics.SphereCast(ray, _radius, out RaycastHit hit, _rayDistance, _layerMask);

        if (isHit)
        {
            // �Փ˂������W�ɍ��������킹��
            //Vector3 pos = transform.position;
            //pos.y = hit.point.y;
            //transform.position = pos;

            _lastHitPositionY = hit.point.y;
        }
    }

    void OnDrawGizmos()
    {
        // �V�[���r���[�ɕ\������
        Vector3 rayPos = transform.position + _rayOffset;
        Ray ray = new Ray(rayPos, Vector3.down);

        // �����Ray���΂�
        RaycastHit hit;
        bool isHit = Physics.SphereCast(ray, _radius, out hit);

        if (isHit)
        {
            // Ray�����g�̉�����q�b�g����Ray�̒�������Ray��\������
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayPos, -transform.up * hit.distance);
            // Ray�̐������ɋ���\������
            Gizmos.DrawWireSphere(rayPos - transform.up * hit.distance, _radius);
        }
        else
        {
            // �q�b�g���Ȃ�������ő�̒�������Ray��\������
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(rayPos, -transform.up * _rayDistance);
        }
    }
}
