using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using System;

/// <summary>
/// レイキャストを用いたプレイヤーの移動のテスト
/// </summary>
public class PlayerMoveRaycast : MonoBehaviour
{
    Camera _mainCamera;
    [SerializeField] LayerMask _layerMask;
    Vector3 _rayOffset = new Vector3(0.0f, 5.0f, 0.0f);
    float _radius = 0.1f;
    float _rayDistance = 10.0f;
    float _lastRaycastTime = 0.0f; // 最後にレイキャストした時間
    float _intervalRaycast = 0.1f; // レイキャストをする間隔
    float _lastHitPositionY;       // 最後にレイキャストした時の高さ

    void Awake()
    {
        // レイキャストを用いた移動なのでコライダーとリジッドボディはオフにする
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

        // 線形補完を利用した移動のテスト(失敗)
        Vector3 LerpPos = transform.position;
        LerpPos.y = Mathf.Lerp(LerpPos.y, _lastHitPositionY, Time.deltaTime * 10);
        transform.position = LerpPos;

        if (inputVec != Vector3.zero)
        {
            //MatchHeightByRaycast(inputVec);
            ConstantSlopeSpeedForRaycast(inputVec);
        }


    }

    // Rayを下向きに放って高さを合わせる
    void MatchHeightByRaycast(Vector3 inputVec)
    {
        // 移動処理、物理挙動じゃないのでUpdate()内で行う
        Vector3 moveVec = inputVec * Time.deltaTime * 6.0f;
        transform.position += moveVec;

        // 一定時間(_IntervalRaycast)の間隔を開けてレイキャストする
        if (Time.time < _lastRaycastTime + _intervalRaycast)
            return;

        // 最終レイキャスト時間を記録
        _lastRaycastTime = Time.time;

        // 自身の座標にオフセットを足した座標から下向きにRayを放つ
        Vector3 rayPos = transform.position + _rayOffset;
        Ray ray = new Ray(rayPos, Vector3.down);

        // 球状のRayを放つ
        bool isHit = Physics.SphereCast(ray, _radius, out RaycastHit hit, _rayDistance, _layerMask);

        if (isHit)
        {
            // 衝突した座標に高さを合わせる
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;

            _lastHitPositionY = hit.point.y;
        }
    }

    /// <summary>斜面でも一定の速度で移動するように修正して移動する</summary>
    private void ConstantSlopeSpeedForRaycast(Vector3 inputVec)
    {
        // 移動速度、テストなので最終的に使用する場合はきちんと直す
        int speed = 10;

        // 移動前の位置を保存
        Vector3 currentPos = transform.position;
        // 目標座標を求める
        Vector3 moveDelta = inputVec * Time.deltaTime * speed;
        Vector3 targetPos = currentPos + moveDelta;
        targetPos.y = Mathf.Lerp(currentPos.y, _lastHitPositionY, Time.deltaTime * 10);
        // 目標座標から現在座標のベクトルの差を求める
        moveDelta = targetPos - currentPos;
        // normalizeして、1フレーム当たりのベクトルを求める
        moveDelta = moveDelta.normalized * Time.deltaTime * speed;
        // 目標の高さを補正
        _lastHitPositionY -= moveDelta.y;
        // ここでは高さの移動は行わない、平面移動する
        moveDelta.y = 0;
        transform.position += moveDelta;

        // 一定時間(_IntervalRaycast)の間隔を開けてレイキャストする
        if (Time.time < _lastRaycastTime + _intervalRaycast)
            return;

        // 最終レイキャスト時間を記録
        _lastRaycastTime = Time.time;

        // 自身の座標にオフセットを足した座標から下向きにRayを放つ
        Vector3 rayPos = transform.position + _rayOffset;
        Ray ray = new Ray(rayPos, Vector3.down);

        // 球状のRayを放つ
        bool isHit = Physics.SphereCast(ray, _radius, out RaycastHit hit, _rayDistance, _layerMask);

        if (isHit)
        {
            // 衝突した座標に高さを合わせる
            //Vector3 pos = transform.position;
            //pos.y = hit.point.y;
            //transform.position = pos;

            _lastHitPositionY = hit.point.y;
        }
    }

    void OnDrawGizmos()
    {
        // シーンビューに表示する
        Vector3 rayPos = transform.position + _rayOffset;
        Ray ray = new Ray(rayPos, Vector3.down);

        // 球状のRayを飛ばす
        RaycastHit hit;
        bool isHit = Physics.SphereCast(ray, _radius, out hit);

        if (isHit)
        {
            // Rayを自身の下からヒットしたRayの長さ分のRayを表示する
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayPos, -transform.up * hit.distance);
            // Rayの先っちょに球を表示する
            Gizmos.DrawWireSphere(rayPos - transform.up * hit.distance, _radius);
        }
        else
        {
            // ヒットしなかったら最大の長さ分のRayを表示する
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(rayPos, -transform.up * _rayDistance);
        }
    }
}
