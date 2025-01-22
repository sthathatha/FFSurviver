using UnityEngine;

/// <summary>
/// 定数
/// </summary>
public class GameConstant
{
    public static readonly Color ButtonEnableColor = Color.red;
    public static readonly Color ButtonDisableColor = new Color(1, 0.5f, 0.5f);

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
}
