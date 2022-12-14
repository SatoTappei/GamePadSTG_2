using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/// <summary>
/// プレイヤーの移動を行う
/// 常にカメラの向いている方向を正面として操作することが出来る。
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour, IAttackAnimControl
{
    Rigidbody _rb;
    Animator _anim;
    Camera _mainCamera;

    [Header("移動用")]
    /// <summary>移動速度</summary>
    [SerializeField] float _speed;
    /// <summary>振り向く速度</summary>
    [SerializeField, Range(0, 1.0f)] float _turnSpeed;
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

    /// <summary>動くことが出来るかどうか</summary>
    bool _canMove;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();

        // TODO:ダッシュ時の音や、パーティクルを出したりするのを登録する
        //_mag.Where(_ => _inputVec.Value != Vector3.zero).Subscribe(f => _anim.SetFloat("Speed", f));
        //_inputVec.Where(v => v == Vector3.zero).Subscribe(_ => _anim.SetFloat("Speed",0));
        //_inputVec.Where(v => v != Vector3.zero).Subscribe(_ => _anim.SetFloat("Speed", 1));

        this.UpdateAsObservable()
            .Where(_ => Input.GetButtonDown("Jump") && _canMove)
            .BatchFrame(0, FrameCountType.FixedUpdate)
            .Subscribe(_ => FixedUpdateJump());
    }

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (!_canMove) return;
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
    }

    void FixedUpdate()
    {
        if (_inputVec.Value != Vector3.zero)
        {
            // 球形補完を使用して時間をかけて振り向く
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_inputVec.Value), _turnSpeed);
        }

        // 移動速度のベクトルを作成、Y方向はUnity内の重力そのまま
        Vector3 veloVec = _inputVec.Value * _speed * _mag.Value;
        veloVec.y = _rb.velocity.y;

        // Rigidbodyのvelocityに弄った移動速度を反映
        _rb.velocity = veloVec;
    }

    /// <summary>動かせるようにする</summary>
    public void WakeUp() => _canMove = true;

    /// <summary>ジャンプの入力があった際にジャンプさせる</summary>
    void FixedUpdateJump()
    {
        Vector3 box = new Vector3(0.5f, 0.1f, 0.5f);
        if (Physics.BoxCast(_foot.position, box, -1 * Vector3.up, out RaycastHit _, Quaternion.identity, 0.5f, _mask))
        {
            Vector3 vec = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.velocity = vec;
            _rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    /// <summary>跳ね返る</summary>
    void Bounce(Vector3 hitPos)
    {
        // 逆方向に力を加える
        Vector3 dir = hitPos * -1;
        dir.y = 0;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir.normalized * 30, ForceMode.VelocityChange);

        // 一定時間操作できないようにする
        DOTween.Sequence()
               .AppendInterval(0.5f)
               .OnStart(() => _canMove = false)
               .OnComplete(() => _canMove = true);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 上の方からぶつかった場合は跳ね返る
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 hitPos = collision.gameObject.transform.position - transform.position;
            if (hitPos.y < -0.5) Bounce(hitPos);
        }
    }

    public void OnAnimEnter()
    {
        _canMove = false;
        _inputVec.Value = Vector3.zero;
    }

    public void OnAnimExit()
    {
        _canMove = true;
    }
}
