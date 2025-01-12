using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HPゲージ
/// </summary>
public class UIHpGauge : MonoBehaviour
{
    #region 定数

    /// <summary>最大幅</summary>
    private const float MAX_WIDTH = 400f;

    #endregion

    #region メンバー

    /// <summary>ゲージ</summary>
    public Image gauge_body;

    #endregion

    #region 機能

    /// <summary>
    /// HP表示
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="hp_max"></param>
    public void SetHP(int hp, int hp_max)
    {
        var size = gauge_body.rectTransform.sizeDelta;
        var w = MAX_WIDTH * hp / hp_max;
        gauge_body.rectTransform.sizeDelta = new Vector2(w, size.y);
    }

    #endregion
}
