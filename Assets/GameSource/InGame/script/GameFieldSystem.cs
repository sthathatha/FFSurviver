using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームフィールド管理
/// </summary>
public class GameFieldSystem : SubScriptBase
{
    /// <summary>雑魚敵親オブジェクト</summary>
    public Transform smallEnemies;

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

        // 雑魚敵をメインシーンに移動
        if (smallEnemies != null)
        {
            for (var i = smallEnemies.childCount - 1; i >= 0; --i)
            {
                smallEnemies.GetChild(i).SetParent(GameMainSystem.Instance.smallEnemyParent, true);
            }
        }
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
