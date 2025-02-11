using System.Collections;
using UnityEngine;

/// <summary>
/// タイトル画面
/// </summary>
public class TitleSystem : MainScriptBase
{
    public TitleMainMenuUI mainMenu;
    public TitleCharacterListUI charaMenu;

    /// <summary>
    /// フェードイン前
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        charaMenu.gameObject.SetActive(false);
    }
}
