using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// ���g���[
/// </summary>
public class UILotteryWindow : AppearUIBase
{
    #region �����o�[

    public Animator cursor;
    public TMP_Text txtCost;

    /// <summary>����</summary>
    public bool isGetLottery {  get; private set; }

    #endregion

    #region ������

    /// <summary>
    /// �\�����O������
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();
        isGetLottery = false;

        //todo:�R�X�g�\��
        txtCost.SetText("1");
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
            if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                //todo:�R�X�g����Ă���Έ���
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
