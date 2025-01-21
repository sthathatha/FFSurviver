using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// ���g���[���ʉ��
/// </summary>
public class UILotteryResult : AppearUIBase
{
    #region �����o�[

    public UILotteryResultMaterial result1;
    public UILotteryResultMaterial result2;
    public UILotteryResultMaterial result3;

    /// <summary>�ڍׁ@���O</summary>
    public TMP_Text txtDetailName;
    /// <summary>�ڍׁ@������</summary>
    public TMP_Text txtDetail;

    /// <summary>���U���g</summary>
    private LineSelectList<UILotteryResultMaterial> resultList;



    #endregion

    #region ������

    /// <summary>
    /// ������
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();

        resultList = new LineSelectList<UILotteryResultMaterial>();
        resultList.AddItem(result1, result2, result3);
    }

    /// <summary>
    /// �\�����O������
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();

        //todo:���U���g�\��

        resultList.MoveReset();
        UpdateCursor();
    }

    #endregion

    /// <summary>
    /// �X�V
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
                //todo:�I��
                break;
            }

            yield return null;
        }

        yield return Close();
    }

    /// <summary>
    /// �J�[�\���X�V���ڍו\��
    /// </summary>
    private void UpdateCursor()
    {
        for (var i = 0; i < resultList.GetAllCount(); ++i)
        {
            if (resultList.selectIndex == i) resultList.GetItem(i).Cursor_Idle();
            else resultList.GetItem(i).Cursor_Hide();
        }

        //todo:���g���[���ʏڍו\��

    }
}