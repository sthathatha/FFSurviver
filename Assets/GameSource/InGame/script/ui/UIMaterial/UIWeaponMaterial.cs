﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器１個
/// </summary>
public class UIWeaponMaterial : UIMaterialBase
{
    #region 項目

    public Image icon;
    public TMP_Text txtLv;
    public TMP_Text txtName;

    #endregion

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="_slotData"></param>
    public void SetWeapon(WeaponManager.SlotData _slotData)
    {
        if (_slotData == null)
        {
            // 空
            icon.gameObject.SetActive(false);
            txtLv.SetText("");
            txtName.SetText(WeaponManager.GetWeaponName(WeaponManager.ID.Empty));
        }
        else
        {
            // 武器の表示
            //todo:武器アイコン
            icon.gameObject.SetActive(true);
            txtLv.SetText(_slotData.lv.ToString());
            txtName.SetText(WeaponManager.GetWeaponName(_slotData.id));
        }
    }
}
