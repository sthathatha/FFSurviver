using UnityEngine;

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
        GetSlot().AsFloat().validTime += 0.2f;
    }
}
