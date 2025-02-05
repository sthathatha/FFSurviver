using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆弾
/// </summary>
public class BombSystem : BaseSearchWeapon
{
    /// <summary>爆弾</summary>
    public BombAttack template;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitStart()
    {
        yield return base.InitStart();

        Prm_coolTime = 4.2f;
        Prm_searchRange = 30f;
    }

    /// <summary>
    /// 爆弾投げる
    /// </summary>
    /// <param name="selfPos"></param>
    /// <param name="targets"></param>
    protected override void ExecAttack(Vector3 selfPos, List<Collider> targets)
    {
        base.ExecAttack(selfPos, targets);

        var main = GameMainSystem.Instance;
        var pprm = main.prm_Player;

        foreach (var target in targets)
        {
            // 爆弾生成
            var na = Instantiate(template, main.attackParent);
            na.gameObject.SetActive(false);

            // 爆弾本体のサイズは大きくしないためSetAttackParamは使わない
            na.SetAttackRate(pprm.stat_melee.value * Prm_attackRate);
            na.scaleRate = Prm_attackSize;
            na.Shoot(selfPos, target.bounds.center);
        }
    }
}
