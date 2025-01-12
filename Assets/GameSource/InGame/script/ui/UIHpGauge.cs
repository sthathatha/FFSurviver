using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HP�Q�[�W
/// </summary>
public class UIHpGauge : MonoBehaviour
{
    #region �萔

    /// <summary>�ő啝</summary>
    private const float MAX_WIDTH = 400f;

    #endregion

    #region �����o�[

    /// <summary>�Q�[�W</summary>
    public Image gauge_body;

    #endregion

    #region �@�\

    /// <summary>
    /// HP�\��
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
