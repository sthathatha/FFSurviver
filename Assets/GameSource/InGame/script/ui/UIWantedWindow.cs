using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// お尋ね者画面
/// </summary>
public class UIWantedWindow : AppearUIBase
{
    #region メンバー

    public UIWantedMaterial wanted1;
    public UIWantedMaterial wanted2;
    public UIWantedMaterial wanted3;
    public UIWantedMaterial wanted4;
    public UIWantedMaterial wanted5;

    public TMP_Text txtName;
    public TMP_Text txtDescription;

    private LineSelectList<UIWantedMaterial> wantedList;

    #endregion

    #region 初期化

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();

        wantedList = new LineSelectList<UIWantedMaterial>();
        wantedList.AddItem(wanted1, wanted2, wanted3, wanted4, wanted5);
    }

    /// <summary>
    /// 表示直前初期化
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();

        //todo:お尋ね者アイコン表示更新

        wantedList.MoveReset();
        UpdateCursor();
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
            if (GameInput.IsPress(GameInput.Buttons.MenuRight))
            {
                wantedList.MoveNext();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuLeft))
            {
                wantedList.MoveBefore();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuCancel))
            {
                break;
            }

            yield return null;
        }

        yield return Close();
    }

    /// <summary>
    /// カーソル更新と詳細表示
    /// </summary>
    private void UpdateCursor()
    {
        for (var i = 0; i < wantedList.GetAllCount(); ++i)
        {
            if (wantedList.selectIndex == i) wantedList.GetItem(i).Cursor_Idle();
            else wantedList.GetItem(i).Cursor_Hide();
        }

        //todo:お尋ね者詳細表示
    }
}
