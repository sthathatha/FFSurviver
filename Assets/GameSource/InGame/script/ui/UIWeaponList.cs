using System.Collections;
using UnityEngine;

/// <summary>
/// ���탊�X�g
/// </summary>
public class UIWeaponList : AppearUIBase
{
    #region �����o�[

    public UIWeaponMaterial wpn1;
    public UIWeaponMaterial wpn2;
    public UIWeaponMaterial wpn3;
    public UIWeaponMaterial wpn4;
    public UIWeaponMaterial wpn5;

    #endregion

    #region ������

    /// <summary>
    /// ������
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();
    }

    /// <summary>
    /// �\�����O������
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();

        //todo:����\��
    }

    #endregion

    /// <summary>
    /// ����
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
