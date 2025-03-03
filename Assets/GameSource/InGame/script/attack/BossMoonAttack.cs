using System.Collections;
using UnityEngine;

/// <summary>
/// 月の怪物　攻撃
/// </summary>
public class BossMoonAttack : AttackParameter
{
    private const float FALL_SPEED = 100f;

    /// <summary>予告テンプレート</summary>
    public BossMoonAttackYokoku yokoku;
    /// <summary>予告表示</summary>
    public BossMoonAttackYokoku yokokuDisp = null;

    /// <summary>ターゲット</summary>
    private Vector3 targetPos;

    /// <summary>
    /// 新規作成
    /// </summary>
    /// <param name="p"></param>
    /// <param name="str_rate"></param>
    public void CreateNew(Vector3 p, float str_rate)
    {
        var n = Instantiate(this, GameMainSystem.Instance.attackParent);
        n.SetAttackRate(str_rate);
        n.Shoot(p);
    }

    /// <summary>
    /// 開始
    /// </summary>
    /// <param name="p"></param>
    private void Shoot(Vector3 p)
    {
        targetPos = p;
        p.y += 100f;
        transform.position = p;
        gameObject.SetActive(true);
        StartCoroutine(UpdateCoroutine());
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateCoroutine()
    {
        var origin = OriginManager.Instance;
        // 予告表示
        yokokuDisp = yokoku.CreateNew(targetPos);
        yield return origin.WaitIngameTime(0.5f);

        // 落下開始
        var p = transform.position;
        while (p.y > -10f)
        {
            yield return null;
            p.y -= FALL_SPEED * origin.inGameDeltaTime;
            transform.position = p;

            if (yokokuDisp != null && p.y <= 0f)
            {
                Destroy(yokokuDisp.gameObject);
                yokokuDisp = null;
            }
        }

        Destroy(gameObject);
    }
}
