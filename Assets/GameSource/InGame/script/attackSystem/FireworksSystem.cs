using UnityEngine;

/// <summary>
/// ねずみ花火
/// </summary>
public class FireworksSystem : BaseTimerWeapon
{
    #region パラメータ

    /// <summary>コピー元</summary>
    public FireworksAttack template;

    /// <summary>攻撃数</summary>
    public int Prm_attackCount { get; set; } = 1;

    #endregion

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="selfPos"></param>
    protected override void ExecAttack(Vector3 selfPos)
    {
        base.ExecAttack(selfPos);

        var rotOne = Mathf.PI * 2f / Prm_attackCount;
        var rot = Util.RandomFloat(0f, Mathf.PI * 2f);

        for (var i = 0; i < Prm_attackCount; ++i)
        {
            CreateAttack(selfPos, rot);
            rot += rotOne;
        }
    }

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="selfPos">基準位置</param>
    /// <param name="rot">発射角度</param>
    private void CreateAttack(Vector3 selfPos, float rot)
    {
        var pprm = GameMainSystem.Instance.prm_Player;
        var atk = Instantiate(template, GameMainSystem.Instance.attackParent);
        SetAttackParam(atk, pprm.stat_melee.value);
        atk.Shoot(selfPos, rot);
    }
}
