using System.Collections;
using UnityEngine;

/// <summary>
/// 移動前後の位置を設定して動く
/// </summary>
public class SimpleMoveAttack : AttackParameter
{
    /// <summary>当たったらそこで止まるか</summary>
    public bool on_hit_stop = false;

    /// <summary>位置</summary>
    private DeltaVector3 deltaPos = null;

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="start">開始点</param>
    /// <param name="target">着弾点</param>
    /// <param name="time">移動時間</param>
    /// <param name="moveType">移動補間方法</param>
    protected void MoveStart(Vector3 start, Vector3 target, float time, DeltaFloat.MoveType moveType = DeltaFloat.MoveType.LINE)
    {
        deltaPos = new DeltaVector3();
        deltaPos.Set(start);
        deltaPos.MoveTo(target, time, moveType);

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
        BeforeMove(deltaPos.Get());

        var origin = OriginManager.Instance;

        while (deltaPos.IsActive() &&
            !(on_hit_stop && isHitted))
        {
            yield return null;
            deltaPos.Update(origin.inGameDeltaTime);

            transform.position = deltaPos.Get();
        }
        yield return null;

        AfterMove(deltaPos.Get());
        Destroy(gameObject);
    }

    #region 派生処理

    /// <summary>
    /// 動き始める直前の処理
    /// </summary>
    /// <param name="pos"></param>
    protected virtual void BeforeMove(Vector3 pos)
    {
    }

    /// <summary>
    /// 動き終わった直後の処理
    /// </summary>
    /// <param name="pos"></param>
    protected virtual void AfterMove(Vector3 pos)
    {
    }

    #endregion
}
