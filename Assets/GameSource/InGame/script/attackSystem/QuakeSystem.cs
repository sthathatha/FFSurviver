using UnityEngine;

/// <summary>
/// 着地地震
/// </summary>
public class QuakeSystem : GameWeaponSystemBase
{
    /// <summary>攻撃テンプレート</summary>
    public SimpleAttack template;

    /// <summary>
    /// 地震発射
    /// </summary>
    public void CreateQuake()
    {
        var main = GameMainSystem.Instance;
        var pprm = main.prm_Player;
        var center = transform.position;

        // 地震生成
        var na = Instantiate(template, main.attackParent);
        na.gameObject.SetActive(false);

        SetAttackParam(na);
        na.Shoot(center, Vector3.zero);
    }
}
