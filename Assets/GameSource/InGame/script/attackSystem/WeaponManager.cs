using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器管理システム
/// </summary>
public class WeaponManager
{
    #region 定数

    /// <summary>武器スロット数</summary>
    public const int SLOT_COUNT = 5;

    /// <summary>
    /// 武器ID
    /// </summary>
    public enum ID
    {
        Empty = 0,
        /// <summary>浮遊</summary>
        FloatBody,
        /// <summary>オプション</summary>
        ChildOption,
        /// <summary>ファイアボール</summary>
        FireBall,
        /// <summary>サンダーボール</summary>
        ThunderBall,
        /// <summary>メテオ</summary>
        Meteor,
        /// <summary>木の葉乱舞</summary>
        LeafWind,
        /// <summary>着地地震</summary>
        Quake,
        /// <summary>ボム</summary>
        Bomb,
        /// <summary>かまいたち</summary>
        Cyclone,
        /// <summary>ねずみ花火</summary>
        Fireworks,
    }

    /// <summary>
    /// 武器名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetWeaponName(ID id)
    {
        return id switch
        {
            ID.ChildOption => Strings.Weapon_Option_Name,
            ID.FloatBody => Strings.Weapon_Float_Name,
            ID.FireBall => Strings.Weapon_Fireball_Name,
            ID.LeafWind => Strings.Weapon_Leaf_Name,
            ID.ThunderBall => Strings.Weapon_Thunder_Name,
            ID.Meteor => Strings.Weapon_Meteor_Name,
            ID.Cyclone => Strings.Weapon_Cyclone_Name,
            ID.Fireworks => Strings.Weapon_Fireworks_Name,
            ID.Quake => Strings.Weapon_Quake_Name,
            ID.Bomb => Strings.Weapon_Bomb_Name,

            _ => Strings.Weapon_Empty_Name,
        };
    }

    #endregion

    #region メンバー

    /// <summary>スロットデータ</summary>
    private List<SlotData> slotList;

    #endregion

    #region 管理クラス

    /// <summary>
    /// スロット1箇所分のデータ
    /// </summary>
    public class SlotData
    {
        /// <summary>武器ID</summary>
        public ID id;
        /// <summary>レベル1～</summary>
        public int lv;

        /// <summary>システムクラス</summary>
        private GameWeaponSystemBase system;

        /// <summary>
        /// スロットデータ
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_system"></param>
        public SlotData(ID _id, GameWeaponSystemBase _system)
        {
            id = _id;
            lv = 1;
            system = _system;
        }

        #region 各IDについてのシステム取得

        public FireballSystem AsFireBall() { return system as FireballSystem; }
        public RollOptionSystem AsRollOption() { return system as RollOptionSystem; }
        public WindSystem AsWind() { return system as WindSystem; }
        public MeteorSystem AsMeteor() { return system as MeteorSystem; }
        public FloatSystem AsFloat() { return system as FloatSystem; }
        public QuakeSystem AsQuake() { return system as QuakeSystem; }
        public BombSystem AsBomb() { return system as BombSystem; }
        public CycloneSystem AsCyclone() { return system as CycloneSystem; }
        public ChildOptionSystem AsChildren() { return system as ChildOptionSystem; }
        public FireworksSystem AsFireworks() { return system as FireworksSystem; }

        #endregion
    }

    #endregion

    #region 初期化

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WeaponManager()
    {
        slotList = new List<SlotData>();
    }

    #endregion

    #region 取得

    /// <summary>
    /// 所持個数
    /// </summary>
    /// <returns></returns>
    public int GetHaveCount() { return slotList.Count; }

    /// <summary>
    /// スロット内容取得
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public SlotData GetSlot(int slot)
    {
        if (slot < 0 || slot >= slotList.Count) return null;
        return slotList[slot];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public SlotData GetWeaponSlot(ID _id)
    {
        foreach (var slot in slotList)
        {
            if (slot.id == _id) return slot;
        }

        return null;
    }

    /// <summary>
    /// IDを持っているかチェック
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public bool HaveWeapon(ID _id)
    {
        foreach (var slot in slotList)
        {
            if (slot.id == _id) return true;
        }

        return false;
    }

    #endregion

    #region 操作

    /// <summary>
    /// 武器追加
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_system"></param>
    public void AddWeapon(ID _id, GameWeaponSystemBase _system)
    {
        var slot = new SlotData(_id, _system);
        slotList.Add(slot);
    }

    #endregion
}
