﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メテオ
/// </summary>
public class MeteorSystem : BaseSearchWeapon
{
    /// <summary>メテオ</summary>
    public MeteorAttack meteorTemplate;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitStart()
    {
        yield return base.InitStart();

        Prm_coolTime = 5f;
        Prm_searchRange = 50f;
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
            // メテオ生成
            var na = Instantiate(meteorTemplate, main.attackParent);

            SetAttackParam(na);
            na.Shoot(target.transform.position);
        }
    }
}
