using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 敵スクリプト共通
/// </summary>
public class EnemyScriptBase : CharacterScript
{
    #region メンバー

    /// <summary>基本体力</summary>
    public int hp_base = 1;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();

        // ゲームシステムからステータス倍率を取得
        var rate = IsBoss() ? GameMainSystem.Instance.GetBossRate() : GameMainSystem.Instance.GetEnemyRate();
        hp_max = Mathf.RoundToInt(hp_base * rate);
        hp = hp_max;

        var atk = GetComponent<AttackParameter>();
        atk.SetAttackRate(rate);
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        // 離れすぎた時
        var pCenter = GameMainSystem.Instance.GetPlayerCenter();
        var distance = pCenter - transform.position;
        if (distance.sqrMagnitude > 10000f) TooFarPlayer(pCenter);
    }

    /// <summary>
    /// プレイヤーから離れすぎた時
    /// </summary>
    /// <param name="playerCenter"></param>
    protected virtual void TooFarPlayer(Vector3 playerCenter)
    {
    }

    #endregion

    #region 属性判定用

    /// <summary>
    /// ボス
    /// </summary>
    /// <returns></returns>
    virtual protected bool IsBoss() { return false; }

    #endregion
}
