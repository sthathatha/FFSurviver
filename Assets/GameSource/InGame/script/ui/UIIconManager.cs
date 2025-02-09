using UnityEngine;

/// <summary>
/// 武器アイコン操作
/// </summary>
public class UIIconManager : MonoBehaviour
{
    #region テクスチャ保持

    // 武器
    public Sprite spBomb;
    public Sprite spCyclone;
    public Sprite spFireball;
    public Sprite spFireworks;
    public Sprite spFloat;
    public Sprite spOption;
    public Sprite spMeteor;
    public Sprite spQuake;
    public Sprite spThunder;
    public Sprite spWind;

    // アイテム用
    public Sprite spHeal;
    public Sprite spStar;
    public Sprite spStatMelee;
    public Sprite spStatMagic;
    public Sprite spStatHp;
    public Sprite spStatSpeed;
    public Sprite spStatJump;
    public Sprite spIconPlus;
    public Sprite spIconSize;
    public Sprite spIconTimer;

    #endregion

    /// <summary>
    /// 武器アイコン取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Sprite GetWeaponIcon(WeaponManager.ID id)
    {
        return id switch
        {
            WeaponManager.ID.Bomb => spBomb,
            WeaponManager.ID.Cyclone => spCyclone,
            WeaponManager.ID.FireBall => spFireball,
            WeaponManager.ID.Fireworks => spFireworks,
            WeaponManager.ID.FloatBody => spFloat,
            WeaponManager.ID.ChildOption => spOption,
            WeaponManager.ID.Meteor => spMeteor,
            WeaponManager.ID.Quake => spQuake,
            WeaponManager.ID.ThunderBall => spThunder,
            WeaponManager.ID.LeafWind => spWind,
            _ => null,
        };
    }
}
