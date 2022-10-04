using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// プレイヤーの移動を行う
/// 常にカメラの向いている方向を正面として操作することが出来る。
/// </summary>
public class PlayerMove : MonoBehaviour
{
    Rigidbody _rb;
    Animator _anim;
    Camera _mainCamera;

    [Header("移動用")]
    /// <summary>移動速度</summary>
    [SerializeField] float _velocity;
    /// <summary>振り向く速度</summary>
    [SerializeField] float _turnSpeed;
    /// <summary>
    /// 移動のアニメーションを制御するパラメーター名
    /// 各モーションの基準となる値 停止:0 歩き:1 ダッシュ:2
    /// </summary>
    [SerializeField] string _animParameterName;

    /// <summary>移動速度の倍率、現在はダッシュ用</summary>
    FloatReactiveProperty _mag = new FloatReactiveProperty();
    /// <summary>左スティックから入力された移動ベクトル</summary>
    Vector3ReactiveProperty _inputVec = new Vector3ReactiveProperty();

    [Header("ジャンプ用")]
    /// <summary>ジャンプ可能かを判定するRayを飛ばす基準となる座標</summary>
    [SerializeField] Transform _foot;
    /// <summary>ジャンプ可能かを判定するRayがヒットするレイヤー</summary>
    [SerializeField] LayerMask _mask;
    /// <summary>ジャンプの入力があったかどうか</summary>
    bool _isJump;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();

        // TODO:ダッシュ時の音や、パーティクルを出したりするのを登録する
        //_mag.Where(_ => _inputVec.Value != Vector3.zero).Subscribe(f => _anim.SetFloat("Speed", f));
        //_inputVec.Where(v => v == Vector3.zero).Subscribe(_ => _anim.SetFloat("Speed",0));
        //_inputVec.Where(v => v != Vector3.zero).Subscribe(_ => _anim.SetFloat("Speed", 1));
    }

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // 移動の入力を受け取る
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        _mag.Value = Input.GetButton("Dash") ? 2 : 1;

        // プレイヤーの上向きのベクトルを軸にカメラのY方向の回転に応じて入力を回転させる
        Quaternion rot = Quaternion.AngleAxis(_mainCamera.transform.eulerAngles.y, Vector3.up);
        _inputVec.Value = rot * new Vector3(hori, 0, vert).normalized;
        
        // 現在の速度をAnimatorに伝える
        float speed = _inputVec.Value.magnitude * _mag.Value;
        _anim.SetFloat(_animParameterName, speed);

        // ジャンプの入力を受け取る
        _isJump = Input.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        if (_inputVec.Value != Vector3.zero)
        {
            // 球形補完を使用して時間をかけて振り向く
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_inputVec.Value), _turnSpeed);
        }

        Vector3 veloVec = _inputVec.Value * _velocity * _mag.Value;
        veloVec.y = _rb.velocity.y;

        if (_isJump)
        {
            if (Physics.BoxCast(_foot.position, Vector3.one * 0.5f, -1 * Vector3.up, out RaycastHit hist, Quaternion.identity, 0.5f, _mask))
            {
                veloVec.y = _rb.velocity.y + 10;
            }

            _isJump = false;
        }

        _rb.velocity = veloVec;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(-1.0f * transform.up, Vector3.one);
    //}
}
