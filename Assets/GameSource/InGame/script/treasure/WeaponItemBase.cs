using System.Collections;
using UnityEngine;

/// <summary>
/// 武器強化アイテム系
/// </summary>
public abstract class WeaponItemBase : TreasureItemBase
{
    /// <summary>武器の最大Lv</summary>
    public const int WEAPON_LV_MAX = 10;
    /// <summary>オプションの最大Lv</summary>
    public const int OPTION_LV_MAX = 4;

    /// <summary>武器ID</summary>
    protected WeaponManager.ID weaponId;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="_weaponId"></param>
    public WeaponItemBase(WeaponManager.ID _weaponId)
    {
        weaponId = _weaponId;
        name = WeaponManager.GetWeaponName(_weaponId);
    }

    /// <summary>
    /// 入手可能判定
    /// </summary>
    /// <returns></returns>
    public override bool CanGet()
    {
        var wpn = GameMainSystem.Instance.weaponManager;
        if (wpn.HaveWeapon(weaponId))
        {
            // 持っている場合は、レベル10まで
            var system = wpn.GetWeaponSlot(weaponId);

            // オプションは4以上で不可
            if (weaponId == WeaponManager.ID.ChildOption)
            {
                if (system.lv >= OPTION_LV_MAX)
                    return false;
            }
            else
            {
                // 他は10以上で不可
                if (system.lv >= WEAPON_LV_MAX) return false;
            }

            // 特殊な取得不可判定
            if (!CanGetExtra()) return false;

            return true;
        }

        // 持っていない場合は強化アイテムなし
        return false;
    }

    /// <summary>
    /// 特殊な取得可能判定
    /// </summary>
    /// <returns></returns>
    protected virtual bool CanGetExtra() { return true; }

    /// <summary>
    /// スロット取得
    /// </summary>
    /// <returns></returns>
    protected WeaponManager.SlotData GetSlot()
    {
        return GameMainSystem.Instance.weaponManager.GetWeaponSlot(weaponId);
    }
}
