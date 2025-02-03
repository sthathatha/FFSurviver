using System.Collections;
using UnityEngine;

/// <summary>
/// メテオの石部分
/// </summary>
public class MeteorAttack : AttackParameter
{
    /// <summary>高さ</summary>
    protected const float FALL_HEIGHT = 10f;

    /// <summary>落下時間</summary>
    protected const float FALL_TIME = 0.3f;

    /// <summary>着弾後の炎上</summary>
    public ExpandAttack burnTemplate;

    /// <summary>位置</summary>
    private DeltaVector3 deltaPos = null;

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="target">着弾点</param>
    public void Shoot(Vector3 target)
    {
        target.y = 0f;

        const float RANDX = FALL_HEIGHT * 0.05f;
        var heightPos = new Vector3(Util.RandomFloat(-RANDX, RANDX), FALL_HEIGHT, Util.RandomFloat(-RANDX, RANDX));
        deltaPos = new DeltaVector3();
        deltaPos.Set(target + heightPos);
        deltaPos.MoveTo(target, FALL_TIME, DeltaFloat.MoveType.LINE);

        transform.position = deltaPos.Get();
        gameObject.SetActive(true);
        StartCoroutine(UpdateCoroutine());
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateCoroutine()
    {
        var main = GameMainSystem.Instance;
        var origin = OriginManager.Instance;
        var pprm = main.prm_Player;

        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(origin.inGameDeltaTime);

            transform.position = deltaPos.Get();
        }
        yield return null;

        // 炎上エフェクトを出して自分は消える
        if (burnTemplate)
        {
            var na = Instantiate(burnTemplate, main.attackParent);

            na.SetAttackRate(attackRate);
            na.scaleRate = scaleRate;
            na.Shoot(deltaPos.Get());
        }

        Destroy(gameObject);
    }
}
