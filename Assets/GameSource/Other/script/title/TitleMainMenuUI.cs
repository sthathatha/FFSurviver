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
        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                itemList.GetSelectItem().Cursor_Stop();
                if (itemList.selectIndex == 0)
                {
                    yield return charaUI.Open();
                    if (charaUI.result != TitleCharacterListUI.Result.Cancel)
                    {
                        // 選択したキャラでゲーム開始
                        ManagerSceneScript.GetInstance().LoadMainScene("GameSceneMain", 0);
                        yield break;
                    }

                    // キャンセルならそのまま
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
