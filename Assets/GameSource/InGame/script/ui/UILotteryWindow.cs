using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���g���[
/// </summary>
public class UILotteryWindow : AppearUIBase
{
    #region �����o�[

    public RuntimeAnimatorController cursor;
    public TMP_Text txtCost;
    public Image btnGet;

    /// <summary>����</summary>
    public bool isGetLottery { get; private set; }

    #endregion

    #region ������

    /// <summary>
    /// �\�����O������
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();
        isGetLottery = false;
        var game = GameMainSystem.Instance.prm_Game;

        // �R�X�g�\��
        txtCost.SetText(game.LotteryCost.ToString());
        if (game.Exp >= game.LotteryCost)
            btnGet.color = GameConstant.ButtonEnableColor;
        else
            btnGet.color = GameConstant.ButtonDisableColor;
    }

    #endregion

    /// <summary>
    /// ����
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
                // �R�X�g����Ă���Έ���
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
