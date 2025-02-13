using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 武器リスト
/// </summary>
public class UIWeaponList : AppearUIBase
{
    #region メンバー

    public UIWeaponMaterial wpn1;
    public UIWeaponMaterial wpn2;
    public UIWeaponMaterial wpn3;
    public UIWeaponMaterial wpn4;
    public UIWeaponMaterial wpn5;

    #endregion

    #region 初期化

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();
    }

    /// <summary>
    /// 表示直前初期化
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();

        // 武器表示
        var man = GameMainSystem.Instance.weaponManager;
        wpn1.SetWeapon(man.GetSlot(0));
        wpn2.SetWeapon(man.GetSlot(1));
        wpn3.SetWeapon(man.GetSlot(2));
        wpn4.SetWeapon(man.GetSlot(3));
        wpn5.SetWeapon(man.GetSlot(4));
    }

    #endregion

    /// <summary>
    /// 処理
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UpdateMenu()
    {
        yield return base.UpdateMenu();
        var sound = ManagerSceneScript.GetInstance().soundManager;

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuCancel))
            {
                sound.PlaySE(sound.commonSeCancel);
                break;
            }

            yield return null;
        }

        yield return Close();
    }
}
