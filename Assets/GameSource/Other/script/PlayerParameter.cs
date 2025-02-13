using UnityEngine;

/// <summary>
/// プレイヤーパラメータ
/// </summary>
public class PlayerParameter
{
    #region パラメータ

    /// <summary>
    /// ステータス管理クラス
    /// </summary>
    public class Status
    {
        /// <summary>現在値</summary>
        public int value { get; set; }
        /// <summary>最大値</summary>
        private int maxValue { get; set; }
        /// <summary>強化値</summary>
        private int upHeight { get; set; }
        /// <summary>強化コスト基本値</summary>
        private int costBase { get; set; }
        /// <summary>次の強化コスト</summary>
        public int cost { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_initVal">初期値</param>
        /// <param name="_costBase">コスト基本値</param>
        /// <param name="_max">最大値</param>
        /// <param name="_upHeight">上がり幅</param>
        public Status(int _initVal, int _upHeight, int _max, int _costBase)
        {
            value = _initVal;
            upHeight = _upHeight;
            maxValue = _max;
            costBase = _costBase;
            cost = _costBase;
        }

        /// <summary>
        /// 強化
        /// </summary>
        /// <param name="_noCount">true:回数にカウントしない（アイテム成長の場合）</param>
        /// <returns>上昇量</returns>
        public int PowerUp(bool _noCount = false)
        {
            var oldVal = value;
            value += upHeight;
            if (maxValue > 0 && value >= maxValue)
            {
                // MAXになったらコスト0
                value = maxValue;
                cost = 0;
            }
            else if (!_noCount)
            {
                // コスト増加計算
                cost += costBase;
            }

            return value - oldVal;
        }

        /// <summary>
        /// 現在のExpで強化可能かどうか
        /// </summary>
        /// <param name="nowExp">現在のExp、マイナスの場合Max判定のみ</param>
        /// <returns></returns>
        public bool CanPowerUp(int nowExp)
        {
            if (maxValue > 0 && value >= maxValue) return false;
            if (nowExp >= 0 && cost > nowExp) return false;

            return true;
        }
    }

    /// <summary>物理攻撃力</summary>
    public Status stat_melee { get; set; }
    /// <summary>魔法攻撃力</summary>
    public Status stat_magic { get; set; }
    /// <summary>最大HP</summary>
    public Status stat_maxHp { get; set; }
    /// <summary>移動速度</summary>
    public Status stat_speed { get; set; }
    /// <summary>ジャンプ回数</summary>
    public Status stat_jump { get; set; }

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        // キャラにより初期化
        var pid = GameConstant.GetTempPID();
        var status = pid switch
        {
            GameConstant.PlayerID.Drows => GameConstant.InitStatus_Drows,
            GameConstant.PlayerID.Eraps => GameConstant.InitStatus_Eraps,
            GameConstant.PlayerID.Exa => GameConstant.InitStatus_Exa,
            GameConstant.PlayerID.Worra => GameConstant.InitStatus_Worra,
            GameConstant.PlayerID.Koob => GameConstant.InitStatus_Koob,
            _ => GameConstant.InitStatus_You,
        };

        stat_melee = new Status(status.melee.init, status.melee.up, status.melee.max, status.melee.cost);
        stat_magic = new Status(status.magic.init, status.magic.up, status.magic.max, status.magic.cost);
        stat_maxHp = new Status(status.maxHp.init, status.maxHp.up, status.maxHp.max, status.maxHp.cost);
        stat_speed = new Status(status.speed.init, status.speed.up, status.speed.max, status.speed.cost);
        stat_jump = new Status(status.jump.init, status.jump.up, status.jump.max, status.jump.cost);
    }

    /// <summary>
    /// speedの値から移動速度を決定
    /// </summary>
    /// <returns></returns>
    public float GetSpeedVelocity()
    {
        var tmp = stat_speed.value;

        return 4f + tmp / 7f;
    }
}
