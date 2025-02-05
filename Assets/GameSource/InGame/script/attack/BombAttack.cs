using UnityEngine;

/// <summary>
/// 爆弾アタック
/// </summary>
public class BombAttack : SimpleMoveAttack
{
    /// <summary>投げ時間</summary>
    protected const float SHOOT_TIME = 0.2f;

    /// <summary>着弾後の炎上</summary>
    public EffectOneShotAttack burnTemplate;

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="start">最初の位置</param>
    /// <param name="target">着弾点</param>
    public void Shoot(Vector3 start, Vector3 target)
    {
        MoveStart(start, target, SHOOT_TIME);
    }

    /// <summary>
    /// 移動後
    /// </summary>
    /// <param name="pos"></param>
    protected override void AfterMove(Vector3 pos)
    {
        base.AfterMove(pos);
        var main = GameMainSystem.Instance;

        // 爆発エフェクトを出して自分は消える
        if (burnTemplate)
        {
            var na = Instantiate(burnTemplate, main.attackParent);

            na.SetAttackRate(attackRate);
            na.scaleRate = scaleRate;
            na.transform.localScale = new Vector3(scaleRate, scaleRate, scaleRate);
            na.Shoot(pos);
        }
    }
}
