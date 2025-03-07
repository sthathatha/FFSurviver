using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ファイアボール
/// </summary>
public class FireballSystem : BaseSearchWeapon
{
    /// <summary>ファイアボール</summary>
    public SimpleAttack fireBall;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitStart()
    {
        yield return base.InitStart();

        Prm_coolTime = 2f;
        Prm_searchRange = 20f;
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
            var na = Instantiate(fireBall, main.attackParent);
            na.gameObject.SetActive(false);

            SetAttackParam(na);
            na.Shoot(selfPos, direction);
        }
    }
}
