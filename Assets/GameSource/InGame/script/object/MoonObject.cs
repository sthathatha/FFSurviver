using System.Collections;
using System.Data;
using UnityEngine;
using static UnityEngine.UI.Image;

/// <summary>
/// 月オブジェクト
/// </summary>
public class MoonObject : MonoBehaviour
{
    #region 定数

    /// <summary>上方向角度</summary>
    private const float UP_ROT = 15f;

    /// <summary>目になる時の角度差</summary>
    private const float EYE_RIGHT_ROT = 10f;

    /// <summary>下に向く時のZ回転</summary>
    private const float EYE_DOWN_ROT = 135f;

    /// <summary>フェード時間</summary>
    private const float FADE_TIME = 2f;

    #endregion

    #region 変数

    /// <summary>プレイヤーからの距離</summary>
    public float player_distance = 900f;

    /// <summary>初期位置</summary>
    public float initRot = 0f;
    /// <summary>位置の角度</summary>
    private DeltaFloat posRot;
    /// <summary>目の向きの角度</summary>
    private DeltaFloat eyeRot;

    /// <summary>表示アルファ</summary>
    private DeltaFloat alpha;

    #endregion

    #region 更新処理

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        eyeRot = new DeltaFloat();
        eyeRot.Set(0f);
        posRot = new DeltaFloat();
        posRot.Set(initRot);
        alpha = new DeltaFloat();
        alpha.Set(0f);

        UpdateAlpha();
        UpdatePos();
    }

    /// <summary>
    /// 表示する
    /// </summary>
    /// <param name="_in">true:現れる　false:消える</param>
    public void Fade(bool _in)
    {
        alpha.MoveTo(_in ? 1f : 0f, FADE_TIME, DeltaFloat.MoveType.BOTH);
        StartCoroutine(AlphaCoroutine());
    }

    /// <summary>
    /// 下に向く
    /// </summary>
    public void DownEye()
    {
        eyeRot.MoveTo(EYE_DOWN_ROT, 1f, DeltaFloat.MoveType.LINE);
        StartCoroutine(EyeRotCoroutine());
    }

    /// <summary>
    /// 位置の角度直接設定
    /// </summary>
    /// <param name="r"></param>
    public void SetPosRot(float r)
    {
        posRot.Set(r);
        UpdatePos();
    }

    /// <summary>
    /// 目の位置の本体位置に移動
    /// </summary>
    /// <param name="_move1"></param>
    public void SetEyeBack(bool _move1)
    {
        if (_move1)
            SetPosRot(180f - EYE_RIGHT_ROT / 2f);
        else
            SetPosRot(360f - EYE_RIGHT_ROT / 2f);
        UpdatePos();
    }

    /// <summary>
    /// 目の位置へ動く
    /// </summary>
    public void MoveToEye()
    {
        posRot.MoveTo(posRot.Get() + 180f - EYE_RIGHT_ROT, 10f, DeltaFloat.MoveType.LINE);
        StartCoroutine(PosRotCoroutine());
    }

    #endregion

    #region 取得系

    /// <summary>
    /// 動作中
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        return posRot.IsActive() || eyeRot.IsActive() || alpha.IsActive();
    }

    /// <summary>
    /// カメラの視界に入ってるかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsInView()
    {
        // カメラ角度
        var cam = ManagerSceneScript.GetInstance().GetCamera3D();
        var camQ = cam.transform.rotation;
        // 自分の角度
        var ownQ = transform.rotation;

        return Quaternion.Angle(camQ, ownQ) < 30f;
    }

    #endregion

    #region 動作コルーチン

    /// <summary>
    /// フェードコルーチン
    /// </summary>
    private IEnumerator AlphaCoroutine()
    {
        var origin = OriginManager.Instance;
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(origin.inGameDeltaTime);

            UpdateAlpha();
        }
    }

    /// <summary>
    /// 下に向く
    /// </summary>
    /// <returns></returns>
    private IEnumerator EyeRotCoroutine()
    {
        var origin = OriginManager.Instance;
        while (eyeRot.IsActive())
        {
            yield return null;
            eyeRot.Update(origin.inGameDeltaTime);
            UpdatePos();
        }
    }

    /// <summary>
    /// 目の位置へ動くコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator PosRotCoroutine()
    {
        var origin = OriginManager.Instance;
        while (posRot.IsActive())
        {
            yield return null;
            posRot.Update(origin.inGameDeltaTime);
            UpdatePos();
        }
    }

    #endregion

    #region 内部メソッド

    /// <summary>
    /// アルファ更新
    /// </summary>
    private void UpdateAlpha()
    {
        var mesh = GetComponent<MeshRenderer>();
        mesh.material.SetColor("_Color", new Color(0.9f, 0.86f, 0f, alpha.Get()));
    }

    /// <summary>
    /// 位置更新
    /// </summary>
    private void UpdatePos()
    {
        // Zの回転
        var eyeQ = Quaternion.Euler(0, 0, eyeRot.Get());
        // プレイヤーに向く回転
        var dist = new Vector3(0, 0, player_distance);
        dist = Quaternion.Euler(0, posRot.Get(), 0) * Quaternion.Euler(-UP_ROT, 0, 0) * dist;
        var posQ = Quaternion.LookRotation(dist, new Vector3(0, 1, 0));

        transform.rotation = posQ * eyeQ;
        transform.localPosition = dist;
    }

    #endregion
}
