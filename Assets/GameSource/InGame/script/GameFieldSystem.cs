using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームフィールド管理
/// </summary>
public class GameFieldSystem : SubScriptBase
{
    /// <summary>足場のランダム幅</summary>
    private const float PLATFORM_RAND_RANGE = 30f;

    /// <summary>床の親オブジェクト</summary>
    public Transform platforms;

    /// <summary>中心座標</summary>
    public Vector2Int fieldCell { get; private set; }

    /// <summary>
    /// 生成時
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitStart()
    {
        objectParent.SetActive(false);
        yield return base.InitStart();

        RandomPlatform();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="paramList"></param>
    /// <returns></returns>
    protected override IEnumerator InitCoroutine(List<int> paramList)
    {
        fieldCell = new Vector2Int(paramList[0], paramList[1]);

        yield return base.InitCoroutine(paramList);

        // 中心を設定
        objectParent.transform.position = FieldUtil.GetBasePosition(fieldCell.x, fieldCell.y);
        objectParent.SetActive(true);
    }

    /// <summary>
    /// 消す
    /// </summary>
    public void ReleaseField()
    {
        Sleep();
        base.DeleteScene();
    }

    /// <summary>
    /// 足場をランダムに動かす
    /// </summary>
    private void RandomPlatform()
    {
        foreach (var p in platforms.GetComponentsInChildren<GameGround>())
        {
            var ppos = p.transform.localPosition;
            ppos.x = Util.RandomFloat(-PLATFORM_RAND_RANGE, PLATFORM_RAND_RANGE);
            ppos.z = Util.RandomFloat(-PLATFORM_RAND_RANGE, PLATFORM_RAND_RANGE);
            p.transform.localPosition = ppos;
        }
    }
}
