using UnityEngine;

/// <summary>
/// 定数
/// </summary>
public class GameConstant
{
    public const string DATA_PLAYERID = "PlayerID";

    public static readonly Color ButtonEnableColor = Color.white;
    public static readonly Color ButtonDisableColor = new(1, 0, 0, 0.5f);

    /// <summary>プレイヤーキャラ</summary>
    public enum PlayerID
    {
        Drows = 0,
        Eraps,
        Exa,
        Worra,
        Koob,
        You,
    }

    /// <summary>
    /// TemporaryDataからプレイヤーID取得
    /// </summary>
    /// <returns></returns>
    public static PlayerID GetTempPID()
    {
        var tmp = GlobalData.GetTemporaryData();
        return (PlayerID)tmp.GetGeneralDataInt(DATA_PLAYERID);
    }

    /// <summary>
    /// TemporaryDataにプレイヤーIDセット
    /// </summary>
    /// <param name="id"></param>
    public static void SetTempPID(PlayerID id)
    {
        var tmp = GlobalData.GetTemporaryData();
        tmp.SetGeneralData(DATA_PLAYERID, (int)id);
    }

    #region 初期化ステータス

    /// <summary>ステータス1個</summary>
    public struct InitStatusOne
    {
        public int init;
        public int up;
        public int max;
        public int cost;

        public InitStatusOne(int _init, int _up, int _max, int _cost)
        {
            init = _init;
            up = _up;
            max = _max;
            cost = _cost;
        }
    }

    /// <summary>初期化ステータス</summary>
    public struct InitStatus
    {
        public InitStatusOne melee;
        public InitStatusOne magic;
        public InitStatusOne maxHp;
        public InitStatusOne speed;
        public InitStatusOne jump;

        public InitStatus(InitStatusOne _melee, InitStatusOne _magic, InitStatusOne _maxHp, InitStatusOne _speed, InitStatusOne _jump)
        {
            melee = _melee;
            magic = _magic;
            maxHp = _maxHp;
            speed = _speed;
            jump = _jump;
        }
    }

    public static readonly InitStatus InitStatus_Drows = new(
        new InitStatusOne(10, 10, -1, 1),
        new InitStatusOne(25, 0, 25, 0),
        new InitStatusOne(100, 50, -1, 5),
        new InitStatusOne(10, 3, 82, 10),
        new InitStatusOne(1, 1, 3, 2000)
        );
    public static readonly InitStatus InitStatus_Eraps = new(
        new InitStatusOne(4, 5, -1, 5),
        new InitStatusOne(2, 2, -1, 5),
        new InitStatusOne(400, 150, -1, 1),
        new InitStatusOne(6, 2, 50, 12),
        new InitStatusOne(1, 1, 2, 4000)
        );
    public static readonly InitStatus InitStatus_Exa = new(
        new InitStatusOne(7, 7, -1, 4),
        new InitStatusOne(3, 4, -1, 5),
        new InitStatusOne(180, 120, -1, 4),
        new InitStatusOne(12, 3, 72, 8),
        new InitStatusOne(1, 1, 3, 1500)
        );
    public static readonly InitStatus InitStatus_Worra = new(
        new InitStatusOne(3, 3, -1, 6),
        new InitStatusOne(6, 5, -1, 3),
        new InitStatusOne(120, 40, -1, 6),
        new InitStatusOne(20, 5, 100, 1),
        new InitStatusOne(2, 1, 5, 2000)
        );
    public static readonly InitStatus InitStatus_Koob = new(
        new InitStatusOne(1, 3, -1, 10),
        new InitStatusOne(12, 11, -1, 2),
        new InitStatusOne(70, 40, -1, 8),
        new InitStatusOne(6, 2, 60, 16),
        new InitStatusOne(1, 1, 3, 2000)
        );
    public static readonly InitStatus InitStatus_You = new(
        new InitStatusOne(8, 6, -1, 4),
        new InitStatusOne(7, 5, -1, 5),
        new InitStatusOne(120, 60, -1, 6),
        new InitStatusOne(18, 4, 90, 4),
        new InitStatusOne(1, 1, 3, 2000)
        );


    #endregion
}
