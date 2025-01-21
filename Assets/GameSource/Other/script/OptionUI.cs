using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �I�v�V�������
/// </summary>
public class OptionUI : AppearUIBase
{
    #region �����o�[

    /// <summary>BGM</summary>
    public Slider bgmSlider;
    /// <summary>SE</summary>
    public Slider seSlider;

    /// <summary>BGM�J�[�\��</summary>
    public GameObject bgmCursor;
    /// <summary>SE�J�[�\��</summary>
    public GameObject seCursor;

    /// <summary>�I����</summary>
    private LineSelectList<int> commands;

    #endregion

    #region ����

    /// <summary>
    /// ������
    /// </summary>
    protected override void InitStart()
    {
        base.InitStart();
        commands = new LineSelectList<int>();
        commands.AddItem(0, 1);
    }

    /// <summary>
    /// ������
    /// </summary>
    protected override void InitOpen()
    {
        base.InitOpen();

        var save = GlobalData.GetSaveData();
        bgmSlider.value = save.system.bgmVolume;
        seSlider.value = save.system.seVolume;

        commands.MoveReset();
        UpdateCursor();
    }

    /// <summary>
    /// �X�V
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UpdateMenu()
    {
        var save = GlobalData.GetSaveData();

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuOK) ||
                GameInput.IsPress(GameInput.Buttons.MenuCancel))
            {
                // ����
                save.SaveSystemData();

                break;
            }
            else
            {
                if (GameInput.IsPress(GameInput.Buttons.MenuUp) ||
                    GameInput.IsPress(GameInput.Buttons.MenuDown))
                {
                    commands.MoveNext();
                    UpdateCursor();
                }
                else if (GameInput.IsPress(GameInput.Buttons.MenuRight) ||
                    GameInput.IsPress(GameInput.Buttons.MenuLeft))
                {
                    var plus = GameInput.IsPress(GameInput.Buttons.MenuRight) ? 1 : -1;
                    if (commands.selectIndex == 0)
                    {
                        save.system.bgmVolume += plus;
                        if (save.system.bgmVolume > 10) save.system.bgmVolume = 10;
                        else if (save.system.bgmVolume < 0) save.system.bgmVolume = 0;

                        bgmSlider.value = save.system.bgmVolume;
                        ManagerSceneScript.GetInstance().soundManager.UpdateBgmVolume();
                    }
                    else
                    {
                        save.system.seVolume += plus;
                        if (save.system.seVolume > 10) save.system.seVolume = 10;
                        else if (save.system.seVolume < 0) save.system.seVolume = 0;

                        seSlider.value = save.system.seVolume;
                        ManagerSceneScript.GetInstance().soundManager.UpdateSeVolume();
                    }
                }
            }

            yield return null;
        }

        yield return Close(isImmediate);
    }

    /// <summary>
    /// �J�[�\���\���X�V
    /// </summary>
    private void UpdateCursor()
    {
        bgmCursor.SetActive(commands.selectIndex == 0);
        seCursor.SetActive(commands.selectIndex == 1);
    }

    #endregion
}
