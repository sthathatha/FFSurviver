using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 武器システム
/// </summary>
public class GameWeaponSystemBase : SubScriptBase
{
    #region 共通パラメータ

    /// <summary>攻撃力アップ</summary>
    public float Prm_attackRate { get; set; } = 1f;

    /// <summary>攻撃サイズ</summary>
    public float Prm_attackSize { get; set; } = 1f;

    #endregion

    #region 派生機能

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    protected virtual void ExecUpdate()
    {
    }

    /// <summary>
    /// 攻撃パラメータ設定
    /// </summary>
    /// <param name="prm">攻撃クラス</param>
    /// <param name="power"></param>
    protected void SetAttackParam(AttackParameter prm, int power)
    {
        prm.SetAttackRate(power * Prm_attackRate);
        prm.scaleRate = Prm_attackSize;
        if (prm is not ExpandAttack)
        {
            prm.transform.localScale = new Vector3(Prm_attackSize, Prm_attackSize, Prm_attackSize);
        }
    }

    #endregion
}
