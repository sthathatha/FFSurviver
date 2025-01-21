using System.Collections;
using UnityEngine;

/// <summary>
/// �X�e�[�^�X���
/// </summary>
public class UIStatusWindow : AppearUIBase
{
    #region �����o�[

    public UIStatusMaterial itemMelee;
    public UIStatusMaterial itemMagic;
    public UIStatusMaterial itemHp;
    public UIStatusMaterial itemSpeed;
    public UIStatusMaterial itemJump;

    private LineSelectList<UIStatusMaterial> itemList;

    /// <summary>
    /// ���ڔԍ�
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

    #region ������

    /// <summary>
    /// ������
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();

        itemList = new LineSelectList<UIStatusMaterial>();
        itemList.AddItem(itemMelee, itemMagic, itemHp, itemSpeed, itemJump);
    }

    /// <summary>
    /// �\�����O������
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();
        //todo:���݂̒l�Z�b�g

        itemList.MoveReset();
        UpdateCursor();
    }

    #endregion

    /// <summary>
    /// �X�V����
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UpdateMenu()
    {
        yield return base.UpdateMenu();

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                //todo:�R�X�g����Ă���΋���
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

    #region ���̑����\�b�h

    /// <summary>
    /// �J�[�\���\���X�V
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
