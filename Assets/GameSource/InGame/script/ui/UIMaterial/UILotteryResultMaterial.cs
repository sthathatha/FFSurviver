using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ロトリー結果１個
/// </summary>
public class UILotteryResultMaterial : UIMaterialBase
{
    public Image icon;
    public Image icon2;

    public UIIconManager iconManager;

    /// <summary>
    /// アイテム表示
    /// </summary>
    /// <param name="_treasure">引いたアイテム</param>
    public void ShowItem(TreasureItemBase _treasure)
    {
        // ロトリー結果アイコン表示
        _treasure.ShowTreasureIcon(icon, icon2, iconManager);
        icon2.gameObject.SetActive(icon2.sprite != null);
    }
}
