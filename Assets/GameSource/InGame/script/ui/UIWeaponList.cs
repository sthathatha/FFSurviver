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

        //todo:武器表示
    }

    #endregion

    /// <summary>
    /// 処理
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UpdateMenu()
    {
        yield return base.UpdateMenu();

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuCancel))
            {
                break;
            }

            yield return null;
        }

        yield return Close();
    }
}
