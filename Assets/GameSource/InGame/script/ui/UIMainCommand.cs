using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���C���R�}���h
/// </summary>
public class UIMainCommand : AppearUIBase
{
    #region �����o�[

    public UIStatusWindow statusWindow;
    public UIWeaponList weaponList;
    public UILotteryWindow lotteryWindow;
    public UILotteryResult lotteryResult;
    public UIWantedWindow wantedWindow;

    public UIMaterialBase itemStatus;
    public UIMaterialBase itemWeapon;
    public UIMaterialBase itemLottery;
    public UIMaterialBase itemWanted;
    public UIMaterialBase itemOption;

    private enum MainCommand
    {
        Status = 0,
        Weapon,
        Lottery,
        Wanted,
        Option,
    }
    private LineSelectList<MainCommand> commands;

    #endregion

    /// <summary>
    /// ������
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();

        commands = new LineSelectList<MainCommand>();
        commands.AddItem(MainCommand.Status, MainCommand.Weapon, MainCommand.Lottery, MainCommand.Wanted, MainCommand.Option);

        StartCoroutine(statusWindow.Close(true));
        StartCoroutine(weaponList.Close(true));
        StartCoroutine(lotteryWindow.Close(true));
        StartCoroutine(lotteryResult.Close(true));
        StartCoroutine(wantedWindow.Close(true));
    }

    /// <summary>
    /// �J�����O
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();
        UpdateCursor();
    }

    /// <summary>
    /// �X�V
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UpdateMenu()
    {
        yield return base.UpdateMenu();
        var manager = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuDown))
            {
                commands.MoveNext();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuUp))
            {
                commands.MoveBefore();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuCancel))
            {
                break;
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                // �R�}���h����
                if (commands.GetSelectItem() == MainCommand.Status)
                {
                    itemStatus.Cursor_Stop();
                    // �X�e�[�^�X
                    yield return statusWindow.Open();
                    yield return new WaitWhile(() => statusWindow.isActive);
                }
                else if (commands.GetSelectItem() == MainCommand.Weapon)
                {
                    itemWeapon.Cursor_Stop();
                    // ����
                    yield return weaponList.Open();
                    yield return new WaitWhile(() => weaponList.isActive);
                }
                else if (commands.GetSelectItem() == MainCommand.Lottery)
                {
                    itemLottery.Cursor_Stop();
                    // ���g���[
                    yield return lotteryWindow.Open();
                    yield return new WaitWhile(() => lotteryWindow.isActive);
                    if (lotteryWindow.isGetLottery)
                    {
                        // ����
                        yield return lotteryResult.Open();
                        yield return new WaitWhile(() => lotteryResult.isActive);
                    }
                }
                else if (commands.GetSelectItem() == MainCommand.Wanted)
                {
                    itemWanted.Cursor_Stop();
                    // ���q�ˎ�
                    yield return wantedWindow.Open();
                    yield return new WaitWhile(() => wantedWindow.isActive);
                }
                else
                {
                    itemOption.Cursor_Stop();
                    // �I�v�V����
                    yield return manager.optionUI.Open();
                    yield return new WaitWhile(() => manager.optionUI.isActive);
                }

                UpdateCursor();
            }

            yield return null;
        }

        // ����
        yield return Close();
    }

    /// <summary>
    /// �J�[�\���\���X�V
    /// </summary>
    private void UpdateCursor()
    {
        var sel = commands.GetSelectItem();
        if (sel == MainCommand.Status) itemStatus.Cursor_Idle();
        else itemStatus.Cursor_Hide();

        if (sel == MainCommand.Weapon) itemWeapon.Cursor_Idle();
        else itemWeapon.Cursor_Hide();

        if (sel == MainCommand.Lottery) itemLottery.Cursor_Idle();
        else itemLottery.Cursor_Hide();

        if (sel == MainCommand.Wanted) itemWanted.Cursor_Idle();
        else itemWanted.Cursor_Hide();

        if (sel == MainCommand.Option) itemOption.Cursor_Idle();
        else itemOption.Cursor_Hide();
    }
}
