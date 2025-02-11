using System.Collections;
using UnityEngine;

/// <summary>
/// キャラ選択メニュー
/// </summary>
public class TitleCharacterListUI : MonoBehaviour
{
    public UIMaterialBase itemDrows;
    public UIMaterialBase itemEraps;
    public UIMaterialBase itemExa;
    public UIMaterialBase itemWorra;
    public UIMaterialBase itemKoob;
    public UIMaterialBase itemYou;

    private LineSelectList<UIMaterialBase> itemList;

    public enum Result
    {
        Cancel = 0,
        OK,
    }
    /// <summary>選択結果</summary>
    public Result result { get; private set; }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        itemList = new LineSelectList<UIMaterialBase>();
        itemList.AddItem(itemDrows, itemEraps, itemExa, itemWorra, itemKoob, itemYou);
        itemList.MoveReset();
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <returns></returns>
    public IEnumerator Open()
    {
        itemList.MoveReset();
        UpdateCursor();
        gameObject.SetActive(true);
        result = Result.Cancel;
        yield return null;

        while (true)
        {
            if (GameInput.IsPress(GameInput.Buttons.MenuOK))
            {
                // 選んだキャラでゲーム開始
                GameConstant.SetTempPID(itemList.selectIndex switch
                {
                    0 => GameConstant.PlayerID.Drows,
                    1 => GameConstant.PlayerID.Eraps,
                    2 => GameConstant.PlayerID.Exa,
                    3 => GameConstant.PlayerID.Worra,
                    4 => GameConstant.PlayerID.Koob,
                    _ => GameConstant.PlayerID.You,
                });

                result = Result.OK;
                break;
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuCancel))
            {
                // キャンセルでメインメニューに戻る
                break;
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuRight))
            {
                itemList.MoveNext();
                UpdateCursor();
            }
            else if (GameInput.IsPress(GameInput.Buttons.MenuLeft))
            {
                itemList.MoveBefore();
                UpdateCursor();
            }

            yield return null;
        }

        gameObject.SetActive(false);
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
