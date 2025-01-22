using UnityEngine;

/// <summary>
/// �萔
/// </summary>
public class GameConstant
{
    public const string DATA_PLAYERID = "PlayerID";

    public static readonly Color ButtonEnableColor = Color.red;
    public static readonly Color ButtonDisableColor = new(1, 0.5f, 0.5f);

    /// <summary>�v���C���[�L����</summary>
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
    /// TemporaryData����v���C���[ID�擾
    /// </summary>
    /// <returns></returns>
    public static PlayerID GetTempPID()
    {
        var tmp = GlobalData.GetTemporaryData();
        return (PlayerID)tmp.GetGeneralDataInt(DATA_PLAYERID);
    }

    #region �������X�e�[�^�X

    /// <summary>�X�e�[�^�X1��</summary>
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

    /// <summary>�������X�e�[�^�X</summary>
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
        new InitStatusOne(10, 3, 100, 10),
        new InitStatusOne(1, 1, 3, 2000)
        );
    public static readonly InitStatus InitStatus_Eraps = new(
        new InitStatusOne(4, 5, -1, 5),
        new InitStatusOne(2, 2, -1, 5),
        new InitStatusOne(200, 150, -1, 2),
        new InitStatusOne(6, 2, 100, 20),
        new InitStatusOne(1, 1, 2, 4000)
        );
    public static readonly InitStatus InitStatus_Exa = new(
        new InitStatusOne(7, 7, -1, 4),
        new InitStatusOne(3, 4, -1, 5),
        new InitStatusOne(180, 120, -1, 4),
        new InitStatusOne(12, 3, 100, 8),
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
        new InitStatusOne(6, 2, 100, 16),
        new InitStatusOne(1, 1, 3, 2000)
        );
    public static readonly InitStatus InitStatus_You = new(
        new InitStatusOne(8, 6, -1, 4),
        new InitStatusOne(7, 5, -1, 5),
        new InitStatusOne(120, 60, -1, 6),
        new InitStatusOne(18, 4, 100, 4),
        new InitStatusOne(1, 1, 3, 2000)
        );


    #endregion
}
