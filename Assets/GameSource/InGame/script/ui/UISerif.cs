using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// セリフUI
/// </summary>
public class UISerif : MonoBehaviour
{
    private const float FADE_TIME = 0.5f;

    #region メンバー

    public CanvasGroup fadeGroup;

    public TMP_Text text;

    #endregion

    #region 処理

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        SetGroupAlpha(0f);
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="text"></param>
    public IEnumerator ShowText(string _text)
    {
        text.SetText(_text);
        yield return GroupFade(true);
    }

    /// <summary>
    /// 表示中に変更
    /// </summary>
    /// <param name="text"></param>
    public IEnumerator ChangeText(string _text)
    {
        yield return TextFade(false);
        text.SetText(_text);
        yield return TextFade(true);
    }

    /// <summary>
    /// 閉じる
    /// </summary>
    public IEnumerator Close()
    {
        yield return GroupFade(false);
    }

    #endregion

    #region 汎用メソッド

    /// <summary>
    /// 全体フェード
    /// </summary>
    /// <param name="_fadein"></param>
    /// <returns></returns>
    private IEnumerator GroupFade(bool _fadein)
    {
        var origin = OriginManager.Instance;
        var alpha = new DeltaFloat();
        var aStart = _fadein ? 0f : 1f;
        var aEnd = _fadein ? 1f : 0f;
        alpha.Set(aStart);
        alpha.MoveTo(aEnd, FADE_TIME, DeltaFloat.MoveType.BOTH);
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(origin.inGameDeltaTime);
            SetGroupAlpha(alpha.Get());
        }
    }

    /// <summary>
    /// テキストのみフェード
    /// </summary>
    /// <param name="_fadein"></param>
    /// <returns></returns>
    private IEnumerator TextFade(bool _fadein)
    {
        var origin = OriginManager.Instance;
        var alpha = new DeltaFloat();
        var aStart = _fadein ? 0f : 1f;
        var aEnd = _fadein ? 1f : 0f;
        alpha.Set(aStart);
        alpha.MoveTo(aEnd, FADE_TIME, DeltaFloat.MoveType.BOTH);
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(origin.inGameDeltaTime);
            SetTextAlpha(alpha.Get());
        }
    }

    /// <summary>
    /// 全体アルファ設定
    /// </summary>
    /// <param name="_alpha"></param>
    private void SetGroupAlpha(float _alpha)
    {
        fadeGroup.alpha = _alpha;
    }

    /// <summary>
    /// テキストアルファ設定
    /// </summary>
    /// <param name="_alpha"></param>
    private void SetTextAlpha(float _alpha)
    {
        text.alpha = _alpha;
    }

    #endregion
}
