using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト
/// </summary>
public class TestWeaponSystem : BaseSearchWeapon
{
    public SimpleAttack test1;

    protected override void ExecAttack(List<Collider> targets)
    {
        base.ExecAttack(targets);

        // 
        var main = GameMainSystem.Instance;
        var tmpData = GlobalData.GetTemporaryData();
        var game = main.prm_Game;
        var pprm = main.prm_Player;
        var center = main.GetPlayerCenter();

        foreach (var target in targets)
        {
            // 向き
            var direction = target.bounds.center - center;

            // 攻撃
            var na = Instantiate(test1, main.attackParent);
            na.SetAttackRate(pprm.stat_magic.value);
            na.gameObject.SetActive(false);
            na.Shoot(center, direction);
        }

    }
}
