using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト
/// </summary>
public class TestWeaponSystem : BaseSearchWeapon
{
    public SimpleAttack test1;

    protected override void ExecAttack(Vector3 selfPos, List<Collider> targets)
    {
        base.ExecAttack(selfPos, targets);

        // 
        var main = GameMainSystem.Instance;
        var tmpData = GlobalData.GetTemporaryData();
        var game = main.prm_Game;
        var pprm = main.prm_Player;
        var center = selfPos;

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
