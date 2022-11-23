using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

// 使用する場合は、空のオブジェクトParentの子にもう一つ空のオブジェクトChildを作り、
// さらにその子にメインカメラを置く。Transfromの値はリセットしておくこと。
/// <summary>
/// プレイヤーを映すカメラを制御する汎用的カメラコントローラー
/// Cinemachineを用いず、CameraParentにアタッチして使う
/// </summary> 
//[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// カメラのパラメーター
    /// 他のクラスからカメラを制御するときはこのクラスのインスタンスを用意する
    /// </summary>
    [Serializable]
    public class Parameter
    {
        public Transform _target;

        // CameraParentに使用
        public Vector3 position;
        public Vector3 angles = new Vector3(10f, 0f, 0f);
        
        // CameraChildに使用
        public float distance = 7f;

        // MainCameraに使用
        public float fieldOfView = 45f;
        public Vector3 offsetPosition = new Vector3(0f, 1f, 0f);
        public Vector3 offsetAngles;

        /// <summary>現在のParameterを複製して返す</summary>
        public Parameter Clone()
        {
            return MemberwiseClone() as Parameter;
        }

        public static Parameter Lerp(Parameter a, Parameter b, float t, Parameter ret)
        {
            ret.position = Vector3.Lerp(a.position, b.position, t);
            ret.angles = LerpAngles(a.angles, b.angles, t);
            ret.distance = Mathf.Lerp(a.distance, b.distance, t);
            ret.fieldOfView = Mathf.Lerp(a.fieldOfView, b.fieldOfView, t);
            ret.offsetPosition = Vector3.Lerp(a.offsetPosition, b.offsetPosition, t);
            ret.offsetAngles = LerpAngles(a.offsetAngles, b.offsetAngles, t);
            return ret;
        }

        public static Vector3 LerpAngles(Vector3 a, Vector3 b, float t)
        {
            Vector3 ret = Vector3.zero;
            ret.x = Mathf.LerpAngle(a.x, b.x, t);
            ret.y = Mathf.LerpAngle(a.y, b.y, t);
            ret.z = Mathf.LerpAngle(a.z, b.y, t);
            return ret;
        }
    }

    public enum CameraMode
    {
        Default,
        ItemLook,
    }
    public CameraMode Mode { get; set; }

    [SerializeField] Transform _parent;
    [SerializeField] Transform _child;
    [SerializeField] Camera _camera;
    [SerializeField] Parameter _parameter;

    Sequence _cameraSeq;

    /// <summary>カメラの追従を一時停止させる</summary>
    public bool IsPause;

    /// <summary>
    /// カメラを振動させる用のベクトル
    /// 参照無しでShakeを呼べるようにするためstaticにしているが不都合があったら直す
    /// </summary>
    static Vector3 _shakeAngles;

    /// <summary>設定したパラメーターを外部から参照するためのプロパティ</summary>
    public Parameter Param => _parameter;

    void Start()
    {
        if (_parameter._target != null)
        {
            _parameter.position = _parameter._target.position;
        }
    }

    void Update()
    {
        if (IsPause) return;

        /* ここに任意のカメラ操作方法を書く */
        float horiR = Input.GetAxis("Horizontal_R");
        float vertR = Input.GetAxis("Vertical_R");
        Vector3 camVec = new Vector3(vertR, horiR, 0) * 3;
        /* 任意のカメラ操作方法ここまで */

        _parameter.angles += camVec;
    }

    void LateUpdate()
    {
        if (IsPause) return;

        // 被写体の更新が済んだ後にカメラを更新する必要があるのでLateUpdateを使う
        if (_parent == null || _child == null || _camera == null)
            return;
        // 被写体が指定されている場合は、カメラの座標を被写体の座標で上書き
        if (_parameter._target != null)
            UpdateTargetBlend(_parameter);

        SetParamToObject();
    }

    /// <summary>パラメータを各種オブジェクトに反映</summary>
    void SetParamToObject()
    {
        _parent.position = _parameter.position;
        _parent.eulerAngles = _parameter.angles;

        var childPos = _child.localPosition;
        childPos.z = -1.0f * _parameter.distance;
        _child.localPosition = childPos;

        _camera.fieldOfView = _parameter.fieldOfView;
        _camera.transform.localPosition = _parameter.offsetPosition;
        _camera.transform.localEulerAngles = _parameter.offsetAngles + _shakeAngles;
    }

    /// <summary>カメラのモードを切り替える</summary>
    public void SwitchCamera(Parameter to)
    {
        Param._target = null;
        _parameter = to;
    }

    /// <summary>少し遅れて追いかけてくるカメラを作るために線形補完を利用する</summary>
    public static void UpdateTargetBlend(Parameter parameter)
    {
        Vector3 start = parameter.position;
        Vector3 end = parameter._target.position;
        float t = Time.deltaTime * 4.0f;
        parameter.position = Vector3.Lerp(start, end, t);
    }

    /// <summary>カメラを振動させる</summary>
    public static void Shake(float duration,Vector3 strength,int vibratio)
    {
        DOTween.Shake(
            () => _shakeAngles,             // 開始時の値
            shake => _shakeAngles = shake,  // パラメータの更新
            duration,                       // 持続時間
            strength,                       // 揺れの強さ
            vibratio)                       // どのくらい振動するか
            .OnComplete(() => _shakeAngles = Vector3.zero);
    }
}