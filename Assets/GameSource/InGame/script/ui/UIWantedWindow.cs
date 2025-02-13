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

        // お尋ね者アイコン表示更新
        var game = GameMainSystem.Instance.prm_Game;
        wanted1.ShowWanted(game.Defeated_Boss1);
        wanted2.ShowWanted(game.Defeated_Boss2);
        wanted3.ShowWanted(game.Defeated_Boss3);
        wanted4.ShowWanted(game.Defeated_Boss4);
        wanted5.ShowWanted(game.Defeated_Boss5);

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
        var sound = ManagerSceneScript.GetInstance().soundManager;

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuRight))
            {
                sound.PlaySE(sound.commonSeMove);
                wantedList.MoveNext();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuLeft))
            {
                sound.PlaySE(sound.commonSeMove);
                wantedList.MoveBefore();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuCancel))
            {
                sound.PlaySE(sound.commonSeCancel);
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

        // お尋ね者詳細表示
        var game = GameMainSystem.Instance.prm_Game;
        switch (wantedList.selectIndex)
        {
            case 0:
                txtName.SetText(Strings.Wanted_Name_Boss1);
                txtDescription.SetText(Strings.Wanted_Detail_Boss1);
                break;
            case 1:
                txtName.SetText(Strings.Wanted_Name_Boss2);
                txtDescription.SetText(Strings.Wanted_Detail_Boss2);
                break;
            case 2:
                txtName.SetText(Strings.Wanted_Name_Boss3);
                txtDescription.SetText(Strings.Wanted_Detail_Boss3);
                break;
            case 3:
                txtName.SetText(Strings.Wanted_Name_Boss4);
                txtDescription.SetText(Strings.Wanted_Detail_Boss4);
                break;
            default:
                if (game.Defeated_Boss1)
                    txtName.SetText(Strings.Wanted_Name_Boss5);
                else
                    txtName.SetText(Strings.Wanted_Name_Boss5_X);
                txtDescription.SetText(Strings.Wanted_Detail_Boss5);
                break;

        }
    }
}
