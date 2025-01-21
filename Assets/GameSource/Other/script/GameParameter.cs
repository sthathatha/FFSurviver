using UnityEngine;

/// <summary>
/// ゲームそのもののパラメータ
/// </summary>
public class GameParameter
{
    #region メンバー

    /// <summary>経験値</summary>
    public int Exp { get; set; }

    /// <summary>ボス撃破フラグ１</summary>
    public bool Defeated_Boss1 { get; set; }
    /// <summary>ボス撃破フラグ２</summary>
    public bool Defeated_Boss2 { get; set; }
    /// <summary>ボス撃破フラグ３</summary>
    public bool Defeated_Boss3 { get; set; }
    /// <summary>ボス撃破フラグ４</summary>
    public bool Defeated_Boss4 { get; set; }
    /// <summary>ボス撃破フラグ５</summary>
    public bool Defeated_Boss5 { get; set; }

    /// <summary>くじ引き回数</summary>
    public int LotteryCount { get; set; }

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    public void InitParam()
    {
        Exp = 0;
        Defeated_Boss1 = false;
        Defeated_Boss2 = false;
        Defeated_Boss3 = false;
        Defeated_Boss4 = false;
        Defeated_Boss5 = false;
        LotteryCount = 0;
    }

}
