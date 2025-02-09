using UnityEngine.UI;

/// <summary>
/// クールタイム全般
/// </summary>
public class GeneralCoolTime : WeaponItemBase
{
    private const float COOLTIME_RATE = 0.9f;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="_wid"></param>
    public GeneralCoolTime(WeaponManager.ID _wid) : base(_wid)
    {
        description = Strings.Item_General_CoolTime;
        rarelity = _wid switch
        {
            WeaponManager.ID.Meteor => 50,
            _ => 30,
        };
    }

    /// <summary>
    /// 入手処理
    /// </summary>
    public override void ExecGetItem()
    {
        base.ExecGetItem();
        var slot = GetSlot();
        if (weaponId == WeaponManager.ID.FireBall)
        {
            slot.AsFireBall().Prm_coolTime *= COOLTIME_RATE;
        }
        else if (weaponId == WeaponManager.ID.Meteor)
        {
            slot.AsMeteor().Prm_coolTime *= COOLTIME_RATE;
        }
        else if (weaponId == WeaponManager.ID.Fireworks)
        {
            slot.AsFireworks().Prm_coolTime *= COOLTIME_RATE;
        }
        else if (weaponId == WeaponManager.ID.Bomb)
        {
            slot.AsBomb().Prm_coolTime *= COOLTIME_RATE;
        }
        else if (weaponId == WeaponManager.ID.Cyclone)
        {
            slot.AsCyclone().Prm_coolTime *= COOLTIME_RATE;
        }
        else if (weaponId == WeaponManager.ID.LeafWind)
        {
            slot.AsWind().Prm_coolTime *= COOLTIME_RATE;
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
        ShowWeaponIcon(icon1, resource);
        icon2.sprite = resource.spIconTimer;
    }
}
