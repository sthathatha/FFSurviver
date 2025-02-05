using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// かまいたち
/// </summary>
public class CycloneSystem : BaseSearchWeapon
{
    /// <summary>コピー元</summary>
    public SimpleAttack template;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitStart()
    {
        yield return base.InitStart();

        Prm_coolTime = 1f;
        Prm_searchRange = 15f;
    }

    /// <summary>
    /// 攻撃
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
            // 向き
            var direction = target.bounds.center - selfPos;

            // ファイアボール生成
            var na = Instantiate(template, main.attackParent);
            na.gameObject.SetActive(false);

            SetAttackParam(na, pprm.stat_magic.value);
            na.Shoot(selfPos, direction);
        }
    }
}
