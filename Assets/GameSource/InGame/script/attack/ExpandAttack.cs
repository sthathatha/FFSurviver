using System.Collections;
using UnityEngine;

/// <summary>
/// 出現した位置で広がって消える
/// </summary>
public class ExpandAttack : AttackParameter
{
    #region パラメータ

    /// <summary>最初サイズ</summary>
    public float startScale = 0.5f;
    /// <summary>最大サイズ</summary>
    public float maxScale = 5f;

    /// <summary>広がる時間</summary>
    public float expandTime = 0.2f;

    /// <summary>広がったあと留まる時間</summary>
    public float keepTime = 0.5f;

    #endregion

    #region 操作

    /// <summary>
    /// 開始
    /// </summary>
    /// <param name="startPos"></param>
    public void Shoot(Vector3 startPos)
    {
        transform.position = startPos;
        UpdateScale(0);
        gameObject.SetActive(true);

        StartCoroutine(ExpandCoroutine());
    }

    /// <summary>
    /// 広がる
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExpandCoroutine()
    {
        var origin = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();

        // 広がる
        var time = 0f;
        while (time < expandTime)
        {
            yield return null;
            time += origin.inGameDeltaTime;
            UpdateScale(time / expandTime);
        }

        // 広がったあとキープ時間
        time = 0f;
        while (time < keepTime)
        {
            yield return null;
            time += origin.inGameDeltaTime;
        }
        if (keepTime <= 0f) yield break;

        // 消える
        Destroy(gameObject);
    }

    /// <summary>
    /// スケール更新
    /// </summary>
    /// <param name="timeRate">0～1</param>
    /// <returns></returns>
    private void UpdateScale(float timeRate)
    {
        var rate = Util.GetClampF(timeRate, 0, 1);
        var scale = Util.CalcBetweenFloat(rate, startScale, maxScale) * scaleRate;
        transform.localScale = new Vector3(scale, scale / 2f, scale);
    }

    #endregion
}
