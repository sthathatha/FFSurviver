using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

/// <summary>
/// 時間ごとに発生する攻撃
/// </summary>
public class BaseTimerWeapon : GameWeaponSystemBase
{
    #region パラメータ

    /// <summary>インターバル秒</summary>
    public float Prm_coolTime = 4f;

    /// <summary>時間管理リスト</summary>
    private List<TimeManager> timeList;

    #endregion

    #region 時間管理

    /// <summary>
    /// 時間管理
    /// </summary>
    private class TimeManager
    {
        /// <summary>システムアクセス用</summary>
        private BaseTimerWeapon system;

        /// <summary>制御時間</summary>
        private float time = 0f;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_system"></param>
        public TimeManager(BaseTimerWeapon _system)
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
            time -= delta;
            if (time <= 0f)
            {
                system.ExecAttack(selfPos);
                time = system.Prm_coolTime;
            }
        }
    }

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

    /// <summary>
    /// 攻撃呼び出し
    /// </summary>
    /// <param name="selfPos"></param>
    protected virtual void ExecAttack(Vector3 selfPos)
    {
    }
}
