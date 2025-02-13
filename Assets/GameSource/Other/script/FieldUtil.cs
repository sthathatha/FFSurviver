using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド計算系メソッド
/// </summary>
public class FieldUtil
{
    /// <summary>フィールドの半径サイズ</summary>
    public const float FIELD_CELL_R = 500f;

    /// <summary>ザコ出現位置</summary>
    public const float ENEMY_POP_DISTANCE = FIELD_CELL_R / 3f;

    /// <summary>敵離れすぎの位置</summary>
    public const float ENEMY_FAR_DISTANCE_SQ = FIELD_CELL_R * FIELD_CELL_R / 2f;

    private static readonly Vector2Int NORMAL_RIGHT = new(1, 0);
    private static readonly Vector2Int NORMAL_RIGHTUP = new(1, 1);
    private static readonly Vector2Int NORMAL_RIGHTDOWN = new(0, -1);
    private static readonly Vector2Int NORMAL_LEFT = new(-1, 0);
    private static readonly Vector2Int NORMAL_LEFTUP = new(0, 1);
    private static readonly Vector2Int NORMAL_LEFTDOWN = new(-1, -1);

    /// <summary>
    /// フィールド座標からワールド座標を計算
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Vector3 GetBasePosition(int x, int z)
    {
        var posZ = FIELD_CELL_R * 2 * z;
        var posX = FIELD_CELL_R * 2 * x - posZ / 2f;

        return new Vector3(posX, 0, posZ);
    }

    /// <summary>
    /// ワールド座標からフィールド座標を計算
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Vector2Int GetFieldLoc(float x, float z)
    {
        var fldZ = Mathf.RoundToInt(z / (FIELD_CELL_R * 2));
        var fldX = Mathf.RoundToInt(x / (FIELD_CELL_R * 2) + (fldZ / 2f));

        return new Vector2Int(fldX, fldZ);
    }

    /// <summary>
    /// ２地点の距離を計算
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <returns></returns>
    public static int CalcLocationDistance(Vector2Int pos1, Vector2Int pos2)
    {
        Vector2Int dist = pos2 - pos1;
        int absX = Mathf.Abs(dist.x);
        int absY = Mathf.Abs(dist.y);

        // 符号が逆の場合は絶対値の合計
        if (dist.x > 0 && dist.y < 0 || dist.x < 0 && dist.y > 0)
        {
            return absX + absY;
        }

        // それ以外の方向は大きい方
        return absX > absY ? absX : absY;
    }

    /// <summary>
    /// 周囲６マスを取得
    /// </summary>
    /// <returns></returns>
    public static List<Vector2Int> GetAroundLocations(Vector2Int _center)
    {
        var ret = new List<Vector2Int>
        {
            _center + NORMAL_RIGHT,
            _center + NORMAL_RIGHTDOWN,
            _center + NORMAL_LEFTDOWN,
            _center + NORMAL_LEFT,
            _center + NORMAL_LEFTUP,
            _center + NORMAL_RIGHTUP
        };

        return ret;
    }

    /// <summary>
    /// ベクトルを回転させたい時の近い回転方向を計算
    /// Y座標は0としてY軸回転
    /// </summary>
    /// <param name="own"></param>
    /// <param name="target"></param>
    /// <returns>1:左回り　-1:右周り</returns>
    public static int CalcNearRotation(Vector3 own, Vector3 target)
    {
        own.y = 0;
        target.y = 0;

        var vY = new Vector3(0, 1, 0);
        var ownRot = Quaternion.LookRotation(own, vY).eulerAngles.y;
        var targetRot = Quaternion.LookRotation(target, vY).eulerAngles.y;

        // 自分のほうが小さい角度にする
        if (targetRot < ownRot) targetRot += 360f;

        var rot = targetRot - ownRot;
        // 180度以下なら左回りで
        return rot <= 180f ? 1 : -1;
    }
}
