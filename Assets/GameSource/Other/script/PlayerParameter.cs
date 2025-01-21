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
        /// <summary>強化回数</summary>
        private int upCount { get; set; }
        /// <summary>強化値</summary>
        private int upHeight { get; set; }
        /// <summary>強化コスト基本値</summary>
        private int costBase { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initVal">初期値</param>
        /// <param name="costBase">コスト基本値</param>
        /// <param name="max">最大値</param>
        /// <param name="upHeight">上がり幅</param>
        public Status(int initVal, int upHeight, int max, int costBase)
        {
            this.value = initVal;
            this.upHeight = upHeight;
            this.maxValue = max;
            this.costBase = costBase;

            this.upCount = 0;
        }

        /// <summary>
        /// 強化
        /// </summary>
        /// <param name="_noCount">true:回数にカウントしない（アイテム成長の場合）</param>
        public void Increase(bool _noCount = false)
        {
            value += upHeight;
            if (!_noCount) upCount++;
        }

        /// <summary>
        /// 次の強化コスト
        /// </summary>
        /// <returns></returns>
        public int GetNextCost()
        {
            //todo:コスト計算
            return (upCount + 1) * costBase;
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
        //todo:キャラにより初期化
        stat_melee = new Status(50, 15, -1, 1);
        stat_magic = new Status(25, 0, 25, 0);
        stat_maxHp = new Status(100, 50, -1, 5);
        stat_speed = new Status(10, 2, 100, 10);
        stat_jump = new Status(1, 1, 3, 2000);
    }
}
