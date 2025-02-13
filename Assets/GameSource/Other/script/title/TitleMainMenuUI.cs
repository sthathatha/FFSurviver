using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// メインメニュー
/// </summary>
public class TitleMainMenuUI : MonoBehaviour
{
    public UIMaterialBase itemStart;
    public UIMaterialBase itemOption;

    public TitleCharacterListUI charaUI;

    public AudioClip seGameStart;

    private LineSelectList<UIMaterialBase> itemList;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        itemList = new LineSelectList<UIMaterialBase>();
        itemList.AddItem(itemStart, itemOption);
        itemList.MoveReset();
        UpdateCursor();

        StartCoroutine(UpdateCoroutine());
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        var sound = manager.soundManager;

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                itemList.GetSelectItem().Cursor_Stop();
                sound.PlaySE(sound.commonSeSelect);
                if (itemList.selectIndex == 0)
                {
                    yield return charaUI.Open();
                    if (charaUI.result != TitleCharacterListUI.Result.Cancel)
                    {
                        // 選択したキャラでゲーム開始
                        sound.PlaySE(seGameStart);
                        manager.LoadMainScene("GameSceneMain", 0);
                        yield break;
                    }

                    // キャンセルならそのまま
                    sound.PlaySE(sound.commonSeCancel);
                }
                else
                {
                    // オプション
                    var opt = OriginManager.Instance.optionUI;
                    yield return opt.Open(true);
                    yield return new WaitWhile(() => opt.isActive);
                }
                itemList.GetSelectItem().Cursor_Idle();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuUp))
            {
                sound.PlaySE(sound.commonSeMove);
                itemList.MoveBefore();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuDown))
            {
                sound.PlaySE(sound.commonSeMove);
                itemList.MoveNext();
                UpdateCursor();
            }

            yield return null;
        }
    }

    /// <summary>
    /// カーソル表示更新
    /// </summary>
    private void UpdateCursor()
    {
        for (var i = 0; i < itemList.GetAllCount(); ++i)
        {
            if (itemList.selectIndex == i) itemList.GetItem(i).Cursor_Idle();
            else itemList.GetItem(i).Cursor_Hide();
        }
    }
}
