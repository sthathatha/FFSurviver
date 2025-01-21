using TMPro;
using UnityEngine;

/// <summary>
/// ステータス画面１項目
/// </summary>
public class UIStatusMaterial : UIMaterialBase
{
    #region アイテム

    public TMP_Text txtValue;
    public TMP_Text txtCost;

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
}
