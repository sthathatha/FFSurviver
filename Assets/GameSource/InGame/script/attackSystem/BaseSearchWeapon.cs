using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 敵を発見したら攻撃するタイプ
/// </summary>
public class BaseSearchWeapon : GameWeaponSystemBase
{
    /// <summary>見つけた敵が少ない場合、一瞬待って撃つ</summary>
    private const float LESS_WAIT = 0.2f;

    #region メンバー

    /// <summary>クールタイム</summary>
    public float Prm_coolTime { get; set; } = 1f;

    /// <summary>判定用クールタイム</summary>
    private float coolTime = 0f;

    /// <summary>検索射程距離</summary>
    public float Prm_searchRange { get; set; } = 5f;

    /// <summary>同時攻撃数</summary>
    public int Prm_attackCount { get; set; } = 1;

    /// <summary>見つけた敵が少ない場合、一瞬待って撃つための残り数</summary>
    private int lessNextCount = 0;

    #endregion

    #region 更新処理

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        ExecUpdate();
        var origin = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();
        if (coolTime > 0f)
        {
            coolTime -= origin.inGameDeltaTime;
        }

        if (coolTime <= 0f && Prm_searchRange > 0f)
        {
            var plr = GameMainSystem.Instance.GetPlayerCenter();

            // 距離が離れるとなにかの処理がかかりそうなので常について動く
            transform.position = plr;

            // 範囲内の敵を検索
            var enemyMask = LayerMask.GetMask("Enemy");
            var cols = Physics.OverlapSphere(plr, Prm_searchRange, enemyMask);
            if (cols.Length == 0) return;

            // 今回攻撃するべき数
            var nowCnt = lessNextCount > 0 ? lessNextCount : Prm_attackCount;

            // 近い順にｎ件取得
            var orderd = cols.OrderBy(c => Vector3.Distance(plr, c.ClosestPoint(plr))).Take(nowCnt);

            if (orderd.Count() == nowCnt)
            {
                // 件数分撃ったらクールタイム開始
                lessNextCount = 0;
                coolTime = Prm_coolTime;
            }
            else
            {
                // 件数が足りなければ短時間で次に
                lessNextCount = nowCnt - orderd.Count();
                coolTime = LESS_WAIT;
            }

            // 実行
            ExecAttack(orderd.ToList());
        }
    }

    #endregion

    /// <summary>
    /// 攻撃呼び出し
    /// </summary>
    /// <param name="targets"></param>
    protected virtual void ExecAttack(List<Collider> targets)
    {
    }
}
