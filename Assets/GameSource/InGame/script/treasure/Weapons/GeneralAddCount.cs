using UnityEngine;

/// <summary>
/// カウントアップ系
/// </summary>
public class GeneralAddCount : WeaponItemBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="_wid"></param>
    public GeneralAddCount(WeaponManager.ID _wid) : base(_wid)
    {
        description = Strings.Item_General_Count;
        rarelity = _wid switch
        {
            WeaponManager.ID.ChildOption => 80,
            _ => 50,
        };
    }

    /// <summary>
    /// 入手処理
    /// </summary>
    public override void ExecGetItem()
    {
        var slot = GetSlot();
        if (weaponId == WeaponManager.ID.FireBall)
        {
            slot.AsFireBall().Prm_attackCount++;
        }
        else if (weaponId == WeaponManager.ID.Meteor)
        {
            slot.AsMeteor().Prm_attackCount++;
        }
        else if (weaponId == WeaponManager.ID.Fireworks)
        {
            slot.AsFireworks().Prm_attackCount++;
        }
        else if (weaponId == WeaponManager.ID.Bomb)
        {
            slot.AsBomb().Prm_attackCount++;
        }
        else if (weaponId == WeaponManager.ID.Cyclone)
        {
            slot.AsCyclone().Prm_attackCount++;
        }
        else if (weaponId == WeaponManager.ID.ThunderBall)
        {
            slot.AsRollOption().AddOption();
        }
        else if (weaponId == WeaponManager.ID.ChildOption)
        {
            slot.AsChildren().AddChild();
        }
    }
}
