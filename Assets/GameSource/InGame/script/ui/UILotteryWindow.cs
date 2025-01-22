using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ロトリー
/// </summary>
public class UILotteryWindow : AppearUIBase
{
    #region メンバー

    public RuntimeAnimatorController cursor;
    public TMP_Text txtCost;
    public Image btnGet;

    /// <summary>引く</summary>
    public bool isGetLottery { get; private set; }

    #endregion

    #region 初期化

    /// <summary>
    /// 表示直前初期化
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();
        isGetLottery = false;
        var game = GameMainSystem.Instance.prm_Game;

        // コスト表示
        txtCost.SetText(game.LotteryCost.ToString());
        if (game.Exp >= game.LotteryCost)
            btnGet.color = GameConstant.ButtonEnableColor;
        else
            btnGet.color = GameConstant.ButtonDisableColor;
    }

    #endregion

    /// <summary>
    /// 処理
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UpdateMenu()
    {
        yield return base.UpdateMenu();
        var game = GameMainSystem.Instance.prm_Game;
        //var expUI = GameMainSystem.Instance.txt_exp

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                // コスト足りていれば引く
                if (game.Exp >= game.LotteryCost)
                {
                    game.Exp -= game.LotteryCost;
                    GameMainSystem.Instance.UpdateExpUI();
                    game.LotteryCostUp();

                    isGetLottery = true;
                    break;
                }
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
