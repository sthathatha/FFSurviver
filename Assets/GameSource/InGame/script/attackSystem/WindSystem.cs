using UnityEngine;

/// <summary>
/// 風
/// </summary>
public class WindSystem : BaseTimerWeapon
{
    /// <summary>攻撃</summary>
    public ExpandAttack template;

    /// <summary>
    /// 攻撃発生
    /// </summary>
    /// <param name="selfPos"></param>
    protected override void ExecAttack(Vector3 selfPos)
    {
        base.ExecAttack(selfPos);

        var game = GameMainSystem.Instance;
        var pprm = game.prm_Player;

        // 現在の中心から広がる
        var atk = Instantiate(template, game.attackParent);
        SetAttackParam(atk, pprm.stat_magic.value);
        atk.Shoot(selfPos);
    }
}
