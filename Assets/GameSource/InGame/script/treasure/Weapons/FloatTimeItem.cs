using UnityEngine.UI;

/// <summary>
/// 浮遊時間強化
/// </summary>
public class FloatTimeItem : WeaponItemBase
{
    /// <summary>
    /// 初期化
    /// </summary>
    public FloatTimeItem() : base(WeaponManager.ID.FloatBody)
    {
        description = Strings.Item_Float_Time;
        rarelity = 10;
    }

    /// <summary>
    /// 強化
    /// </summary>
    public override void ExecGetItem()
    {
        base.ExecGetItem();
        GetSlot().AsFloat().validTime += 0.2f;
    }

    /// <summary>
    /// アイコン表示
    /// </summary>
    /// <param name="icon1"></param>
    /// <param name="icon2"></param>
    /// <param name="resource"></param>
    public override void ShowTreasureIcon(Image icon1, Image icon2, UIIconManager resource)
    {
        ShowWeaponIcon(icon1, resource);
        icon2.sprite = resource.spIconTimer;
    }
}
