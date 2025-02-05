using System.Collections;
using UnityEngine;

/// <summary>
/// メテオの石部分
/// </summary>
public class MeteorAttack : SimpleMoveAttack
{
    /// <summary>高さ</summary>
    protected const float FALL_HEIGHT = 10f;

    /// <summary>落下時間</summary>
    protected const float FALL_TIME = 0.3f;

    /// <summary>着弾後の炎上</summary>
    public ExpandAttack burnTemplate;

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="target">着弾点</param>
    public void Shoot(Vector3 target)
    {
        target.y = 0f;

        const float RANDX = FALL_HEIGHT * 0.05f;
        var heightPos = new Vector3(Util.RandomFloat(-RANDX, RANDX), FALL_HEIGHT, Util.RandomFloat(-RANDX, RANDX));

        MoveStart(target + heightPos, target, FALL_TIME);
    }

    /// <summary>
    /// 移動後
    /// </summary>
    /// <param name="pos"></param>
    protected override void AfterMove(Vector3 pos)
    {
        base.AfterMove(pos);
        var main = GameMainSystem.Instance;

        // 炎上エフェクトを出して自分は消える
        if (burnTemplate)
        {
            var na = Instantiate(burnTemplate, main.attackParent);

            na.SetAttackRate(attackRate);
            na.scaleRate = scaleRate;
            na.Shoot(pos);
        }
    }
}
