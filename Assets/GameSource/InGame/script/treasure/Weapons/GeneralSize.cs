using UnityEngine.UI;

/// <summary>
/// サイズアップ全般
/// </summary>
public class GeneralSize : WeaponItemBase
{
    private const float SIZE_RATE = 1.2f;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="_wid"></param>
    public GeneralSize(WeaponManager.ID _wid) : base(_wid)
    {
        description = Strings.Item_General_Size;
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
            slot.AsFireBall().Prm_attackSize *= SIZE_RATE;
        }
        else if (weaponId == WeaponManager.ID.Meteor)
        {
            slot.AsMeteor().Prm_attackSize *= SIZE_RATE;
        }
        else if (weaponId == WeaponManager.ID.Fireworks)
        {
            slot.AsFireworks().Prm_attackSize *= SIZE_RATE;
        }
        else if (weaponId == WeaponManager.ID.Bomb)
        {
            slot.AsBomb().Prm_attackSize *= SIZE_RATE;
        }
        else if (weaponId == WeaponManager.ID.Cyclone)
        {
            slot.AsCyclone().Prm_attackSize *= SIZE_RATE;
        }
        else if (weaponId == WeaponManager.ID.ThunderBall)
        {
            slot.AsRollOption().Prm_attackSize *= SIZE_RATE;
        }
        else if (weaponId == WeaponManager.ID.Quake)
        {
            slot.AsQuake().Prm_attackSize *= SIZE_RATE;
        }
        else if (weaponId == WeaponManager.ID.LeafWind)
        {
            slot.AsWind().Prm_attackSize *= SIZE_RATE;
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
        icon2.sprite = resource.spIconSize;
    }
}
