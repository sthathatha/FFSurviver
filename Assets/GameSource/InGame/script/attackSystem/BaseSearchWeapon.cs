using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

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

    /// <summary>検索射程距離</summary>
    public float Prm_searchRange { get; set; } = 5f;

    /// <summary>同時攻撃数</summary>
    public int Prm_attackCount { get; set; } = 1;

    /// <summary>時間管理リスト</summary>
    private List<TimeManager> timeList;

    #endregion

    #region 更新処理

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitStart()
    {
        yield return base.InitStart();

        timeList = new List<TimeManager>
        {
            new(this)
        };
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void ExecUpdate()
    {
        base.ExecUpdate();
        var dt = OriginManager.Instance.inGameDeltaTime;

        // プレイヤーの判定更新
        timeList[0].Update(dt, GameMainSystem.Instance.GetPlayerCenter());

        var wpn = GameMainSystem.Instance.weaponManager;
        if (!wpn.HaveWeapon(WeaponManager.ID.ChildOption)) return;

        // オプションの数にあわせて作成
        var opt = wpn.GetWeaponSlot(WeaponManager.ID.ChildOption).AsChildren();

        // 更新
        for (var i = 0; i < opt.GetOptionCount(); ++i)
        {
            var idx = i + 1;
            if (timeList.Count <= idx) timeList.Add(new(this));

            var o = opt.GetOption(i);
            timeList[idx].Update(dt, o.GetCenter());
        }
    }

    #endregion

    #region 時間と敵サーチ

    /// <summary>
    /// 時間と敵サーチ管理
    /// </summary>
    private class TimeManager
    {
        /// <summary>システムアクセス用</summary>
        private BaseSearchWeapon system;

        /// <summary>判定用クールタイム</summary>
        private float coolTime = 0f;

        /// <summary>見つけた敵が少ない場合、一瞬待って撃つための残り数</summary>
        private int lessNextCount = 0;

        /// <summary>一瞬待つ間に時間が経ったらリセット</summary>
        private float lessWaitTime = 0f;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_system"></param>
        public TimeManager(BaseSearchWeapon _system)
        {
            system = _system;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="selfPos"></param>
        public void Update(float delta, Vector3 selfPos)
        {
            if (lessWaitTime > 0f)
            {
                lessWaitTime -= delta;
                if (lessWaitTime <= 0f)
                {
                    // 残り発射があってもリセット
                    coolTime = 0f;
                    lessWaitTime = 0f;
                    lessNextCount = 0;
                }
            }

            // クールタイム中
            if (coolTime > 0f)
            {
                coolTime -= delta;
            }

            if (coolTime <= 0f && system.Prm_searchRange > 0f)
            {
                // 範囲内の敵を検索
                var enemyMask = LayerMask.GetMask("Enemy");
                var cols = Physics.OverlapSphere(selfPos, system.Prm_searchRange, enemyMask);
                if (cols.Length == 0) return;

                // 今回攻撃するべき数
                var nowCnt = lessNextCount > 0 ? lessNextCount : system.Prm_attackCount;

                // 近い順にｎ件取得
                var orderd = cols.OrderBy(c => Vector3.Distance(selfPos, c.ClosestPoint(selfPos))).Take(nowCnt);

                if (orderd.Count() == nowCnt)
                {
                    // 件数分撃ったらクールタイム開始
                    lessNextCount = 0;
                    coolTime = system.Prm_coolTime;
                }
                else
                {
                    // 件数が足りなければ短時間で次に
                    lessNextCount = nowCnt - orderd.Count();
                    lessWaitTime = system.Prm_coolTime;
                    coolTime = LESS_WAIT;
                }

                // 実行
                system.ExecAttack(selfPos, orderd.ToList());
            }
        }
    }

    #endregion

    /// <summary>
    /// 攻撃呼び出し
    /// </summary>
    /// <param name="selfPos">自分の位置</param>
    /// <param name="targets"></param>
    protected virtual void ExecAttack(Vector3 selfPos, List<Collider> targets)
    {
    }
}
