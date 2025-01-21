using TMPro;
using UnityEngine;

/// <summary>
/// �X�e�[�^�X��ʂP����
/// </summary>
public class UIStatusMaterial : UIMaterialBase
{
    #region �A�C�e��

    public TMP_Text txtValue;
    public TMP_Text txtCost;

    #endregion

    /// <summary>
    /// �l
    /// </summary>
    /// <param name="val"></param>
    public void SetValue(int val)
    {
        txtValue.SetText(val.ToString());
    }

    /// <summary>
    /// �R�X�g
    /// </summary>
    /// <param name="cost"></param>
    public void SetCost(int cost)
    {
        txtCost.SetText(cost.ToString());
    }
}
