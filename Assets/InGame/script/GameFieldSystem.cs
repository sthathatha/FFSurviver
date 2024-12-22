using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームフィールド管理
/// </summary>
public class GameFieldSystem : SubScriptBase
{
    /// <summary>中心座標</summary>
    public Vector2Int fieldCell { get; private set; }

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
}
