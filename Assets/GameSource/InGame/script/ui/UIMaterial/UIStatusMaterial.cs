using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステータス画面１項目
/// </summary>
public class UIStatusMaterial : UIMaterialBase
{
    #region アイテム

    public TMP_Text txtValue;
    public TMP_Text txtCost;
    public Image btnUp;

    #endregion

    /// <summary>
    /// 値
    /// </summary>
    /// <param name="val"></param>
    public void SetValue(int val)
    {
        txtValue.SetText(val.ToString());
    }

    /// <summary>
    /// コスト
    /// </summary>
    /// <param name="cost"></param>
    public void SetCost(int cost)
    {
        txtCost.SetText(cost.ToString());
    }

    /// <summary>
    /// ボタン押下可能表示
    /// </summary>
    /// <param name="enable"></param>
    public void SetButtonEnable(bool enable)
    {
        if (enable)
            btnUp.color = GameConstant.ButtonEnableColor;
        else
            btnUp.color = GameConstant.ButtonDisableColor;
    }
}
