using UnityEngine.UI;

/// <summary>
/// ステータスアップ系
/// </summary>
public class StatusTreasure : TreasureItemBase
{
    /// <summary>
    /// ステータス
    /// </summary>
    public enum Status
    {
        Melee = 0,
        Magic,
        Hp,
        Speed,
        Jump,
    }
    /// <summary>ステータス</summary>
    private Status kind;

    /// <summary>上がる回数</summary>
    private int upCount;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="_kind"></param>
    /// <param name="_upCount"></param>
    public StatusTreasure(Status _kind, int _upCount)
    {
        kind = _kind;
        upCount = _upCount;

        name = _kind switch
        {
            Status.Melee => Strings.Item_Stat_Melee,
            Status.Magic => Strings.Item_Stat_Magic,
            Status.Hp => Strings.Item_Stat_Hp,
            Status.Speed => Strings.Item_Stat_Speed,
            _ => Strings.Item_Stat_Jump,
        };
        description = _kind switch
        {
            Status.Melee => Strings.Item_Stat_Melee_Desc,
            Status.Magic => Strings.Item_Stat_Magic_Desc,
            Status.Hp => Strings.Item_Stat_Hp_Desc,
            Status.Speed => Strings.Item_Stat_Speed_Desc,
            _ => Strings.Item_Stat_Jump_Desc,
        };
        rarelity = 1;
    }

    /// <summary>
    /// 入手可能か
    /// </summary>
    /// <returns></returns>
    public override bool CanGet()
    {
        var pprm = GameMainSystem.Instance.prm_Player;
        if (kind == Status.Melee)
            return pprm.stat_melee.CanPowerUp(-1);
        else if (kind == Status.Magic)
            return pprm.stat_magic.CanPowerUp(-1);
        else if (kind == Status.Hp)
            return pprm.stat_maxHp.CanPowerUp(-1);
        else if (kind == Status.Speed)
            return pprm.stat_speed.CanPowerUp(-1);
        else
            return pprm.stat_jump.CanPowerUp(-1);
    }

    /// <summary>
    /// レアリティ
    /// </summary>
    /// <returns></returns>
    public override int GetRarelity()
    {
        var pprm = GameMainSystem.Instance.prm_Player;

        // コストが大きいステータスは出にくい

        if (kind == Status.Melee)
            return pprm.stat_melee.cost * rarelity;
        else if (kind == Status.Magic)
            return pprm.stat_magic.cost * rarelity;
        else if (kind == Status.Hp)
            return pprm.stat_maxHp.cost * rarelity;
        else if (kind == Status.Speed)
            return pprm.stat_speed.cost * rarelity;
        else
            return pprm.stat_jump.cost * rarelity;
    }

    /// <summary>
    /// 入手
    /// </summary>
    public override void ExecGetItem()
    {
        var plr = GameMainSystem.Instance.playerScript;
        var pprm = GameMainSystem.Instance.prm_Player;

        // コスト使用せずアップする
        for (var i = 0; i < upCount; ++i)
        {
            if (kind == Status.Melee)
                pprm.stat_melee.PowerUp(true);
            else if (kind == Status.Magic)
                pprm.stat_magic.PowerUp(true);
            else if (kind == Status.Hp)
            {
                var hpPlus = pprm.stat_maxHp.PowerUp(true);
                plr.Heal(hpPlus);
            }
            else if (kind == Status.Speed)
                pprm.stat_speed.PowerUp(true);
            else
                pprm.stat_jump.PowerUp(true);
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
        icon1.sprite = kind switch
        {
            Status.Melee => resource.spStatMelee,
            Status.Magic => resource.spStatMagic,
            Status.Hp => resource.spStatHp,
            Status.Speed => resource.spStatSpeed,
            Status.Jump => resource.spStatJump,
            _ => null,
        };
        icon2.sprite = resource.spIconPlus;
    }
}
