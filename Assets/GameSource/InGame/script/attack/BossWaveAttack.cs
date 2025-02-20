using System.Collections;
using UnityEngine;

/// <summary>
/// 水の怪物　波攻撃
/// </summary>
public class BossWaveAttack : AttackParameter
{
    #region パラメータ

    /// <summary>最初サイズ</summary>
    const float startScale = 100f;
    /// <summary>終了サイズ</summary>
    const float endScale = 1f;

    /// <summary>一番下のY</summary>
    const float min_y = -8f;
    /// <summary>一番上のY</summary>
    const float max_y = 0f;

    /// <summary>上がる時間</summary>
    const float upTime = 0.2f;
    /// <summary>上がってから下がるまでの時間</summary>
    const float mainTime = 10f;
    /// <summary>下がる時間</summary>
    const float downTime = 1f;
    const float totalTime = upTime + mainTime + downTime;

    #endregion

    #region 操作

    /// <summary>
    /// 開始
    /// </summary>
    /// <param name="targetPos"></param>
    public void Shoot(Vector3 targetPos)
    {
        transform.position = targetPos;

        UpdateScale(0);
        gameObject.SetActive(true);
        StartCoroutine(WaveCoroutine());
    }

    /// <summary>
    /// 動く
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaveCoroutine()
    {
        var origin = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();
        var time = 0f;
        while (time < totalTime)
        {
            yield return null;
            time += origin.inGameDeltaTime;
            UpdateScale(time);
        }

        // 消える
        Destroy(gameObject);
    }

    /// <summary>
    /// スケール更新
    /// </summary>
    /// <param name="time">経過時間0～totalTime</param>
    /// <returns></returns>
    private void UpdateScale(float time)
    {
        var sRate = Util.GetClampF(time / totalTime, 0f, 1f);
        var s = Util.CalcBetweenFloat(sRate, startScale, endScale);

        var yRate = 0f;
        if (yRate > upTime + mainTime) yRate = Util.GetClampF((time - upTime - mainTime) / downTime, 0f, 1f);
        else if (yRate > upTime) yRate = 1f;
        else yRate = 1f - Util.GetClampF(time / upTime, 0f, 1f);
        var y = Util.CalcBetweenFloat(yRate, min_y, max_y);

        transform.localScale = new Vector3(s, 1f, s);
        var pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }

    #endregion
}
