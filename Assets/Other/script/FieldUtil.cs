using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド計算系メソッド
/// </summary>
public class FieldUtil
{
    /// <summary>フィールドの半径サイズ</summary>
    public const float FIELD_CELL_R = 2f;

    private static readonly Vector2Int NORMAL_RIGHT = new Vector2Int(1, 0);
    private static readonly Vector2Int NORMAL_RIGHTUP = new Vector2Int(1, 1);
    private static readonly Vector2Int NORMAL_RIGHTDOWN = new Vector2Int(0, -1);
    private static readonly Vector2Int NORMAL_LEFT = new Vector2Int(-1, 0);
    private static readonly Vector2Int NORMAL_LEFTUP = new Vector2Int(0, 1);
    private static readonly Vector2Int NORMAL_LEFTDOWN = new Vector2Int(-1, -1);

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
    /// 方向の左右を取得
    /// </summary>
    /// <param name="_normal"></param>
    /// <returns></returns>
    public static List<Vector2Int> GetLRWingLocations(Vector2Int _normal)
    {
        var ret = new List<Vector2Int>();

        if (_normal.Equals(NORMAL_RIGHT))
        {
            ret.Add(NORMAL_RIGHTUP);
            ret.Add(NORMAL_RIGHTDOWN);
        }
        else if (_normal.Equals(NORMAL_RIGHTDOWN))
        {
            ret.Add(NORMAL_RIGHT);
            ret.Add(NORMAL_LEFTDOWN);
        }
        else if (_normal.Equals(NORMAL_LEFTDOWN))
        {
            ret.Add(NORMAL_RIGHTDOWN);
            ret.Add(NORMAL_LEFT);
        }
        else if (_normal.Equals(NORMAL_LEFT))
        {
            ret.Add(NORMAL_LEFTDOWN);
            ret.Add(NORMAL_LEFTUP);
        }
        else if (_normal.Equals(NORMAL_LEFTUP))
        {
            ret.Add(NORMAL_LEFT);
            ret.Add(NORMAL_RIGHTUP);
        }
        else if (_normal.Equals(NORMAL_RIGHTUP))
        {
            ret.Add(NORMAL_LEFTUP);
            ret.Add(NORMAL_RIGHT);
        }

        return ret;
    }
}
