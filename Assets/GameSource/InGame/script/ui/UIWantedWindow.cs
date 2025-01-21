using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// ���q�ˎ҉��
/// </summary>
public class UIWantedWindow : AppearUIBase
{
    #region �����o�[

    public UIWantedMaterial wanted1;
    public UIWantedMaterial wanted2;
    public UIWantedMaterial wanted3;
    public UIWantedMaterial wanted4;
    public UIWantedMaterial wanted5;

    public TMP_Text txtName;
    public TMP_Text txtDescription;

    private LineSelectList<UIWantedMaterial> wantedList;

    #endregion

    #region ������

    /// <summary>
    /// ������
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();

        wantedList = new LineSelectList<UIWantedMaterial>();
        wantedList.AddItem(wanted1, wanted2, wanted3, wanted4, wanted5);
    }

    /// <summary>
    /// �\�����O������
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();

        //todo:���q�ˎ҃A�C�R���\���X�V

        wantedList.MoveReset();
        UpdateCursor();
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
    /// �J�[�\���X�V�Əڍו\��
    /// </summary>
    private void UpdateCursor()
    {
        for (var i = 0; i < wantedList.GetAllCount(); ++i)
        {
            if (wantedList.selectIndex == i) wantedList.GetItem(i).Cursor_Idle();
            else wantedList.GetItem(i).Cursor_Hide();
        }

        //todo:���q�ˎҏڍו\��
    }
}
