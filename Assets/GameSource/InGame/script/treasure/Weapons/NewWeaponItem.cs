using UnityEngine;

/// <summary>
/// 武器新規入手系
/// </summary>
public class NewWeaponItem : TreasureItemBase
{
    /// <summary>武器ID</summary>
    protected WeaponManager.ID weaponId;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="_weaponId"></param>
    public NewWeaponItem(WeaponManager.ID _weaponId)
    {
        weaponId = _weaponId;

        name = WeaponManager.GetWeaponName(_weaponId);
        description = _weaponId switch
        {
            WeaponManager.ID.FireBall => Strings.Weapon_Fireball_Desc,
            WeaponManager.ID.ThunderBall => Strings.Weapon_Thunder_Desc,
            WeaponManager.ID.Meteor => Strings.Weapon_Meteor_Desc,
            WeaponManager.ID.LeafWind => Strings.Weapon_Leaf_Desc,
            WeaponManager.ID.Quake => Strings.Weapon_Quake_Desc,
            WeaponManager.ID.Bomb => Strings.Weapon_Bomb_Desc,
            WeaponManager.ID.FloatBody => Strings.Weapon_Float_Desc,
            WeaponManager.ID.ChildOption => Strings.Weapon_Option_Desc,
            WeaponManager.ID.Cyclone => Strings.Weapon_Cyclone_Desc,
            WeaponManager.ID.Fireworks => Strings.Weapon_Fireworks_Desc,
            _ => ""
        };

        rarelity = 10;
    }

    /// <summary>
    /// 入手可能判定
    /// </summary>
    /// <returns></returns>
    public override bool CanGet()
    {
        var wpn = GameMainSystem.Instance.weaponManager;

        // 持っておらず、かつ空きがある
        return !wpn.HaveWeapon(weaponId)
            && wpn.GetHaveCount() < WeaponManager.SLOT_COUNT;
    }

    /// <summary>
    /// 入手処理
    /// </summary>
    public override void ExecGetItem()
    {
        var manager = ManagerSceneScript.GetInstance();
        // デフォルト入手
        var sceneName = weaponId switch
        {
            WeaponManager.ID.FireBall => "GameWeaponFireball",
            WeaponManager.ID.ThunderBall => "GameWeaponThunder",
            WeaponManager.ID.LeafWind => "GameWeaponWind",
            WeaponManager.ID.Meteor => "GameWeaponMeteor",
            WeaponManager.ID.FloatBody => "GameWeaponFloat",
            WeaponManager.ID.Quake => "GameWeaponQuake",
            WeaponManager.ID.Bomb => "GameWeaponBomb",
            WeaponManager.ID.Cyclone => "GameWeaponCyclone",
            WeaponManager.ID.ChildOption => "GameWeaponChildOption",
            _ => "GameWeaponFireworks",
        };
        manager.LoadSubScene(sceneName, (int)weaponId);
    }
}
