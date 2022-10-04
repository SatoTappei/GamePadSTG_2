using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// カメラコントローラーにアイテムに注視する機能を付けたもの(未使用)
/// </summary> 
// 使用する場合は、空のオブジェクトParentの子にもう一つ空のオブジェクトChildを作り、
// さらにその子にメインカメラを置く。Transfromの値はリセットしておくこと。
[ExecuteInEditMode]
public class LookItemCameraController : MonoBehaviour
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

        /// <summary>
        /// MemberwiseClone関数を用いて新しくインスタンスを作成して
        /// 各メンバの値をコピーする。戻り値がobject型なのでキャストを忘れずに
        /// </summary>
        public Parameter Clone() => MemberwiseClone() as Parameter;

        /// <summary>渡されたParameterにstartとendを補完した値を代入して返す</summary>
        public static Parameter Lerp(Parameter start, Parameter end, float t, Parameter ret)
        {
            ret.position = Vector3.Lerp(start.position, end.position, t);
            ret.angles = LerpAngles(start.angles, end.angles, t);
            ret.distance = Mathf.Lerp(start.distance, end.distance, t);
            ret.fieldOfView = Mathf.Lerp(start.fieldOfView, end.fieldOfView, t);
            ret.offsetPosition = Vector3.Lerp(start.offsetPosition, end.offsetPosition, t);
            ret.offsetAngles = LerpAngles(start.offsetAngles, end.offsetAngles, t);
            return ret;

            Vector3 LerpAngles(Vector3 start, Vector3 end, float t)
            {
                Vector3 ret = Vector3.zero;
                ret.x = Mathf.LerpAngle(start.x, end.y, t);
                ret.y = Mathf.LerpAngle(start.y, end.y, t);
                ret.z = Mathf.LerpAngle(start.x, end.y, t);
                return ret;
            }
        }
    }

    /// <summary>カメラのモード</summary>
    enum ModeType
    {
        Default,  // プレイヤーに追従する
        LookItem, // アイテムをズームする
    }

    [SerializeField] Transform _parent;
    [SerializeField] Transform _child;
    [SerializeField] Camera _camera;
    [SerializeField] Parameter _parameter;
    [SerializeField] Parameter _itemCamParam;

    ModeType _modeType;
    Parameter _defaultCamParam;
    // アニメーション中にキャンセルされてもいいようにメンバー変数として保持しておく
    Sequence _cameraSeq;

    /// <summary>設定したパラメーターを外部から参照するためのプロパティ</summary>
    public Parameter Param => _parameter;

    void Awake()
    {
        _defaultCamParam = _parameter.Clone();
    }

    void Update()
    {
        /* ここに任意のカメラ操作方法を書く */
        if (_modeType == ModeType.Default && 
            (_cameraSeq == null || !(_cameraSeq.IsActive() && _cameraSeq.IsPlaying())))
        {
            float horiR = Input.GetAxis("Horizontal_R");
            float vertR = Input.GetAxis("Vertical_R");
            Vector3 camVec = new Vector3(vertR, horiR, 0) * 3;
            _parameter.angles += camVec;
        }
        /* 任意のカメラ操作方法ここまで */

        /* カメラモードの切り替え処理のテスト */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_modeType)
            {
                case ModeType.Default:
                    SwitchCamera(ModeType.LookItem);
                    break;
                case ModeType.LookItem:
                    SwitchCamera(ModeType.Default);
                    break;
            }
        }
        /* テストここまで */
    }

    void LateUpdate()
    {
        // 被写体の更新が済んだ後にカメラを更新する必要があるのでLateUpdateを使う
        if (_parent == null || _child == null || _camera == null)
            return;
        // 被写体が指定されている場合は、カメラの座標を被写体の座標で上書き
        if (_parameter._target != null)
            UpdateTargetBlend(_parameter);

        // パラメータを各種オブジェクトに反映
        _parent.position = _parameter.position;
        _parent.eulerAngles = _parameter.angles;

        var childPos = _child.localPosition;
        childPos.z = -1.0f * _parameter.distance;
        _child.localPosition = childPos;

        _camera.fieldOfView = _parameter.fieldOfView;
        _camera.transform.localPosition = _parameter.offsetPosition;
        _camera.transform.localEulerAngles = _parameter.offsetAngles;
    }

    // 少し遅れて追いかけてくるカメラを作るために線形補完を利用する
    public static void UpdateTargetBlend(Parameter parameter)
    {
        Vector3 start = parameter.position;
        Vector3 end = parameter._target.position;
        float t = Time.deltaTime * 4.0f;
        parameter.position = Vector3.Lerp(start, end, t);
    }

    /// <summary>モードに対応したパラメーターを返す</summary>
    Parameter GetCameraParameter(ModeType type)
    {
        switch (type)
        {
            case ModeType.Default:
                return _defaultCamParam;
            case ModeType.LookItem:
                return _itemCamParam;
            default:
                return null;
        }
    }

    /// <summary>カメラの切り替え処理</summary>
    void SwitchCamera(ModeType type)
    {
        // カメラモードを更新
        _modeType = type;
        // マウスを動かすと画面がぶれるので、予めターゲットをnullに設定
        _parameter._target = null;

        Parameter startCamParam = _parameter.Clone();
        Parameter endCamParam = GetCameraParameter(_modeType);

        // カメラの移動時間
        float duration = 2.0f;
        // カメラの移動
        _cameraSeq?.Kill();
        _cameraSeq = DOTween.Sequence();
        // 線形補完を用いて移動させる
        _cameraSeq.Append(DOTween.To(() => 0f, t => Parameter.Lerp(startCamParam, endCamParam, t, _parameter), 1f, duration))
            .SetEase(Ease.OutQuart);
        // アニメーション時のブレンドを実行
        _cameraSeq.OnUpdate(() => UpdateTargetBlend(_defaultCamParam));
        // 上のシーケンス終了後のコールバックで、targetを設定
        _cameraSeq.AppendCallback(() => _parameter._target = endCamParam._target);
    }
}