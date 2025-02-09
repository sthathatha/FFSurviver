using UnityEngine.UI;

/// <summary>
/// 回復
/// </summary>
public class HealTreasure : TreasureItemBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="rate"></param>
    public HealTreasure()
    {
        name = Strings.Item_Heal_Name;
        description = Strings.Item_Heal_Desc;
        rarelity = 100;
    }

    /// <summary>
    /// 入手可能か
    /// </summary>
    /// <returns></returns>
    public override bool CanGet()
    {
        // HPが80%以下
        var plr = GameMainSystem.Instance.playerScript;
        var pprm = GameMainSystem.Instance.prm_Player;
        var rate = (plr.hp * 100) / pprm.stat_maxHp.value;

        return rate < 80;
    }

    /// <summary>
    /// 入手
    /// </summary>
    public override void ExecGetItem()
    {
        var plr = GameMainSystem.Instance.playerScript;
        plr.HealRate(0.5f);
    }

    /// <summary>
    /// アイコン表示
    /// </summary>
    /// <param name="icon1"></param>
    /// <param name="icon2"></param>
    /// <param name="resource"></param>
    public override void ShowTreasureIcon(Image icon1, Image icon2, UIIconManager resource)
    {
        icon1.sprite = resource.spHeal;
        icon2.sprite = null;
    }
}
