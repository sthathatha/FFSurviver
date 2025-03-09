using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 宝箱マネージャ
/// </summary>
public class TreasureManager
{
    #region データリスト

    /// <summary>アイテムリスト</summary>
    private List<TreasureItemBase> itemDataList;

    /// <summary>
    /// 初期化
    /// </summary>
    public TreasureManager()
    {
        itemDataList = new List<TreasureItemBase>
        {
            // 新規武器入手アイテム
            new NewWeaponItem(WeaponManager.ID.FireBall)
            ,new NewWeaponItem(WeaponManager.ID.Meteor)
            ,new NewWeaponItem(WeaponManager.ID.LeafWind)
            ,new NewWeaponItem(WeaponManager.ID.ThunderBall)
            ,new NewWeaponItem(WeaponManager.ID.Bomb)
            ,new NewWeaponItem(WeaponManager.ID.Quake)
            ,new NewWeaponItem(WeaponManager.ID.Cyclone)
            ,new NewWeaponItem(WeaponManager.ID.Fireworks)
            ,new NewWeaponItem(WeaponManager.ID.FloatBody)
            ,new NewWeaponItem(WeaponManager.ID.ChildOption)
            // 回復
            ,new HealTreasure()
            // ステータス強化
            ,new AllStatusTreasure()
            ,new StatusTreasure(StatusTreasure.Status.Melee, 1)
            ,new StatusTreasure(StatusTreasure.Status.Magic, 1)
            ,new StatusTreasure(StatusTreasure.Status.Hp, 1)
            ,new StatusTreasure(StatusTreasure.Status.Speed, 1)
            ,new StatusTreasure(StatusTreasure.Status.Jump, 1)
            // 武器強化・カウント
            ,new GeneralAddCount(WeaponManager.ID.FireBall)
            ,new GeneralAddCount(WeaponManager.ID.Meteor)
            ,new GeneralAddCount(WeaponManager.ID.ThunderBall)
            ,new GeneralAddCount(WeaponManager.ID.Bomb)
            ,new GeneralAddCount(WeaponManager.ID.Cyclone)
            ,new GeneralAddCount(WeaponManager.ID.Fireworks)
            ,new GeneralAddCount(WeaponManager.ID.ChildOption)
            // 武器強化・クールタイム
            ,new GeneralCoolTime(WeaponManager.ID.FireBall)
            ,new GeneralCoolTime(WeaponManager.ID.Meteor)
            ,new GeneralCoolTime(WeaponManager.ID.LeafWind)
            ,new GeneralCoolTime(WeaponManager.ID.Bomb)
            ,new GeneralCoolTime(WeaponManager.ID.Cyclone)
            ,new GeneralCoolTime(WeaponManager.ID.Fireworks)
            // 武器強化・サイズ
            ,new GeneralSize(WeaponManager.ID.FireBall)
            ,new GeneralSize(WeaponManager.ID.Meteor)
            ,new GeneralSize(WeaponManager.ID.LeafWind)
            ,new GeneralSize(WeaponManager.ID.ThunderBall)
            ,new GeneralSize(WeaponManager.ID.Bomb)
            ,new GeneralSize(WeaponManager.ID.Quake)
            ,new GeneralSize(WeaponManager.ID.Cyclone)
            ,new GeneralSize(WeaponManager.ID.Fireworks)
            // 独自強化・浮遊
            ,new FloatTimeItem()
        };
    }

    #endregion

    #region 取得

    /// <summary>
    /// アイテム3個取得
    /// </summary>
    /// <returns></returns>
    public List<TreasureItemBase> GetItem()
    {
        var canGetList = itemDataList.Where(t => t.CanGet()).ToList();
        var ret = new List<TreasureItemBase>();
        var maxRare = canGetList.Max(t => t.GetRarelity());
        var rateList = canGetList.Select(t => maxRare - t.GetRarelity() + (maxRare / 20) + 1).ToList();

        var indexes = Util.RandomIndexList(rateList, 3);

        return indexes.Select(i => canGetList[i]).ToList();
    }

    #endregion
}
