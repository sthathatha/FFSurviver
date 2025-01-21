using System.Collections;
using UnityEngine;

/// <summary>
/// ステータス画面
/// </summary>
public class UIStatusWindow : AppearUIBase
{
    #region メンバー

    public UIStatusMaterial itemMelee;
    public UIStatusMaterial itemMagic;
    public UIStatusMaterial itemHp;
    public UIStatusMaterial itemSpeed;
    public UIStatusMaterial itemJump;

    private LineSelectList<UIStatusMaterial> itemList;

    /// <summary>
    /// 項目番号
    /// </summary>
    private enum ItemIndex
    {
        Melee = 0,
        Magic,
        Hp,
        Speed,
        Jump,
    }

    #endregion

    #region 初期化

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();

        itemList = new LineSelectList<UIStatusMaterial>();
        itemList.AddItem(itemMelee, itemMagic, itemHp, itemSpeed, itemJump);
    }

    /// <summary>
    /// 表示直前初期化
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();
        //todo:現在の値セット

        itemList.MoveReset();
        UpdateCursor();
    }

    #endregion

    /// <summary>
    /// 更新処理
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UpdateMenu()
    {
        yield return base.UpdateMenu();

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                //todo:コスト足りていれば強化
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuCancel))
            {
                break;
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuUp))
            {
                itemList.MoveBefore();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuDown))
            {
                itemList.MoveNext();
                UpdateCursor();
            }

            yield return null;
        }

        yield return Close();
    }

    #region その他メソッド

    /// <summary>
    /// カーソル表示更新
    /// </summary>
    private void UpdateCursor()
    {
        for (var i = 0; i < itemList.GetAllCount(); ++i)
        {
            if (itemList.selectIndex == i) itemList.GetItem(i).Cursor_Idle();
            else itemList.GetItem(i).Cursor_Hide();
        }
    }

    #endregion
}
