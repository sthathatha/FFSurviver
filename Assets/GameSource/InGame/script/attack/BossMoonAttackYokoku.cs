using System.Collections;
using UnityEngine;

/// <summary>
/// 月の怪物　攻撃前予告演出
/// </summary>
public class BossMoonAttackYokoku : MonoBehaviour
{
    /// <summary>落下速度</summary>
    private const float FALL_SPEED = 100f;

    /// <summary>
    /// 新規作成する
    /// </summary>
    /// <param name="p"></param>
    public BossMoonAttackYokoku CreateNew(Vector3 p)
    {
        // 自分を複製して表示開始
        var n = Instantiate(this, GameMainSystem.Instance.attackParent);
        n.Shoot(p);
        return n;
    }

    /// <summary>
    /// 表示開始
    /// </summary>
    /// <param name="p"></param>
    private void Shoot(Vector3 p)
    {
        p.y += 0f;
        transform.position = p;

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 処理
    /// </summary>
    /// <returns></returns>
    //private IEnumerator UpdateCoroutine()
    //{
    //    // 下まで落ちて消える
    //    var origin = OriginManager.Instance;

    //    var p = transform.position;
    //    while (p.y > -10f)
    //    {
    //        yield return null;
    //        p.y -= FALL_SPEED * origin.inGameDeltaTime;
    //        transform.position = p;
    //    }

    //    Destroy(gameObject);
    //}
}
