using UnityEngine.UI;

/// <summary>
/// シェリフスター
/// </summary>
public class AllStatusTreasure : TreasureItemBase
{

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AllStatusTreasure()
    {
        name = Strings.Item_Stat_All;
        description = Strings.Item_Stat_All_Desc;
        rarelity = 1;
    }

    /// <summary>
    /// 入手可能か
    /// </summary>
    /// <returns></returns>
    public override bool CanGet()
    {
        var wpn = GameMainSystem.Instance.weaponManager;

        // 武器が埋まってなければ出ない
        if (wpn.GetHaveCount() < WeaponManager.SLOT_COUNT) return false;

        // 武器全部MAXでなければ出ない
        for (var i = 0; i < WeaponManager.SLOT_COUNT; ++i)
        {
            var slot = wpn.GetSlot(i);
            if (slot.id == WeaponManager.ID.ChildOption && slot.lv == 4) return false;
            if (slot.lv == 10) return false;
        }

        return true;
    }

    /// <summary>
    /// レアリティ
    /// </summary>
    /// <returns></returns>
    public override int GetRarelity()
    {
        var pprm = GameMainSystem.Instance.prm_Player;

        // 全ステータスのコスト掛け算レアリティ
        var rare = pprm.stat_melee.cost == 0 ? 1 : pprm.stat_melee.cost
            * pprm.stat_magic.cost == 0 ? 1 : pprm.stat_magic.cost
            * pprm.stat_maxHp.cost == 0 ? 1 : pprm.stat_maxHp.cost
            * pprm.stat_speed.cost == 0 ? 1 : pprm.stat_speed.cost
            * pprm.stat_jump.cost == 0 ? 1 : pprm.stat_jump.cost
            ;

        return rare;
    }

    /// <summary>
    /// 入手
    /// </summary>
    public override void ExecGetItem()
    {
        var plr = GameMainSystem.Instance.playerScript;
        var pprm = GameMainSystem.Instance.prm_Player;

        // コスト使用せずアップする
        for (var i = 0; i < 2; ++i)
        {
            pprm.stat_melee.PowerUp(true);
            pprm.stat_magic.PowerUp(true);
            var hpPlus = pprm.stat_maxHp.PowerUp(true);
            pprm.stat_speed.PowerUp(true);
            pprm.stat_jump.PowerUp(true);

            plr.Heal(hpPlus);
        }

    }

    /// <summary>
    /// アイコン表示
    /// </summary>
    /// <param name="icon1"></param>
    /// <param name="icon2"></param>
    /// <param name="resource"></param>
    public override void ShowTreasureIcon(Image icon1, Image icon2, UIIconManager resource)
    {
        icon1.sprite = resource.spStar;
        icon2.sprite = null;
    }
}
