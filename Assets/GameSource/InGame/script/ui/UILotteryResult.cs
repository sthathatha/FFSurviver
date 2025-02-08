using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ロトリー結果画面
/// </summary>
public class UILotteryResult : AppearUIBase
{
    #region メンバー

    public UILotteryResultMaterial result1;
    public UILotteryResultMaterial result2;
    public UILotteryResultMaterial result3;

    /// <summary>詳細　名前</summary>
    public TMP_Text txtDetailName;
    /// <summary>詳細　説明文</summary>
    public TMP_Text txtDetail;

    /// <summary>リザルトUI</summary>
    private LineSelectList<UILotteryResultMaterial> resultList;

    /// <summary>引いたアイテムデータ</summary>
    private List<TreasureItemBase> treasureList;

    #endregion

    #region 初期化

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();

        resultList = new LineSelectList<UILotteryResultMaterial>();
        resultList.AddItem(result1, result2, result3);

        treasureList = new List<TreasureItemBase>();
    }

    /// <summary>
    /// 表示直前初期化
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();

        // 引く
        var tMan = GameMainSystem.Instance.treasureManager;
        treasureList.Clear();
        treasureList = tMan.GetItem();
        for (var i = 0; i < resultList.GetAllCount(); ++i)
        {
            resultList.GetItem(i).ShowItem(treasureList[i]);
        }

        resultList.MoveReset();
        UpdateCursor();
    }

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UpdateMenu()
    {
        yield return base.UpdateMenu();

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuRight))
            {
                resultList.MoveNext();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuLeft))
            {
                resultList.MoveBefore();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                // 選択
                treasureList[resultList.selectIndex].ExecGetItem();
                break;
            }

            yield return null;
        }

        yield return Close();
        yield return new WaitWhile(() => ManagerSceneScript.GetInstance().IsLoadingSubScene());
    }

    /// <summary>
    /// カーソル更新かつ詳細表示
    /// </summary>
    private void UpdateCursor()
    {
        for (var i = 0; i < resultList.GetAllCount(); ++i)
        {
            if (resultList.selectIndex == i) resultList.GetItem(i).Cursor_Idle();
            else resultList.GetItem(i).Cursor_Hide();
        }

        // 詳細表示
        var idx = resultList.selectIndex;
        txtDetailName.SetText(treasureList[idx].name);
        txtDetail.SetText(treasureList[idx].description);
    }
}