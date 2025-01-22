using System;
using System.Collections;
using System.Runtime.InteropServices;
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

        // ���݂̒l�Z�b�g
        UpdateValue();

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
                // �R�X�g����Ă���΋���
                var pp = GameMainSystem.Instance.prm_Player;
                var game = GameMainSystem.Instance.prm_Game;
                var pwAct = new Action<UIStatusMaterial, PlayerParameter.Status>((item, stat) =>
                {
                    if (stat.CanPowerUp(game.Exp))
                    {
                        game.Exp -= stat.cost;
                        GameMainSystem.Instance.UpdateExpUI();
                        stat.PowerUp();
                        UpdateValue();
                    }
                    else
                    {
                        //todo:�G���[��
                    }
                });
                if (itemList.selectIndex == 0)
                    pwAct(itemList.GetSelectItem(), pp.stat_melee);
                else if (itemList.selectIndex == 1)
                    pwAct(itemList.GetSelectItem(), pp.stat_magic);
                else if (itemList.selectIndex == 2)
                    pwAct(itemList.GetSelectItem(), pp.stat_maxHp);
                else if (itemList.selectIndex == 3)
                    pwAct(itemList.GetSelectItem(), pp.stat_speed);
                else
                    pwAct(itemList.GetSelectItem(), pp.stat_jump);
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
    /// ���݂̒l�ɍX�V
    /// </summary>
    private void UpdateValue()
    {
        var game = GameMainSystem.Instance.prm_Game;
        var prm = GameMainSystem.Instance.prm_Player;

        var setAct = new Action<UIStatusMaterial, PlayerParameter.Status>((item, stat) =>
        {
            item.SetValue(stat.value);
            item.SetCost(stat.cost);
            item.SetButtonEnable(stat.CanPowerUp(game.Exp));
        });

        setAct(itemMelee, prm.stat_melee);
        setAct(itemMagic, prm.stat_magic);
        setAct(itemHp, prm.stat_maxHp);
        setAct(itemSpeed, prm.stat_speed);
        setAct(itemJump, prm.stat_jump);
    }

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
