using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// ロトリー
/// </summary>
public class UILotteryWindow : AppearUIBase
{
    #region メンバー

    public Animator cursor;
    public TMP_Text txtCost;

    /// <summary>引く</summary>
    public bool isGetLottery {  get; private set; }

    #endregion

    #region 初期化

    /// <summary>
    /// 表示直前初期化
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();
        isGetLottery = false;

        //todo:コスト表示
        txtCost.SetText("1");
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
            if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                //todo:コスト足りていれば引く
                isGetLottery = true;
                break;
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuCancel))
            {
                break;
            }

            yield return null;
        }

        yield return Close();
    }
}
