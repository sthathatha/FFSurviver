using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�B�[���h�v�Z�n���\�b�h
/// </summary>
public class FieldUtil
{
    /// <summary>�t�B�[���h�̔��a�T�C�Y</summary>
    public const float FIELD_CELL_R = 5f;

    private static readonly Vector2Int NORMAL_RIGHT = new Vector2Int(1, 0);
    private static readonly Vector2Int NORMAL_RIGHTUP = new Vector2Int(1, 1);
    private static readonly Vector2Int NORMAL_RIGHTDOWN = new Vector2Int(0, -1);
    private static readonly Vector2Int NORMAL_LEFT = new Vector2Int(-1, 0);
    private static readonly Vector2Int NORMAL_LEFTUP = new Vector2Int(0, 1);
    private static readonly Vector2Int NORMAL_LEFTDOWN = new Vector2Int(-1, -1);

    /// <summary>
    /// �t�B�[���h���W���烏�[���h���W���v�Z
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
    /// ���[���h���W����t�B�[���h���W���v�Z
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
    /// �Q�n�_�̋������v�Z
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <returns></returns>
    public static int CalcLocationDistance(Vector2Int pos1, Vector2Int pos2)
    {
        Vector2Int dist = pos2 - pos1;
        int absX = Mathf.Abs(dist.x);
        int absY = Mathf.Abs(dist.y);

        // �������t�̏ꍇ�͐�Βl�̍��v
        if (dist.x > 0 && dist.y < 0 || dist.x < 0 && dist.y > 0)
        {
            return absX + absY;
        }

        // ����ȊO�̕����͑傫����
        return absX > absY ? absX : absY;
    }

    /// <summary>
    /// ���͂U�}�X���擾
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
    /// �x�N�g������]�����������̋߂���]�������v�Z
    /// Y���W��0�Ƃ���Y����]
    /// </summary>
    /// <param name="own"></param>
    /// <param name="target"></param>
    /// <returns>1:�����@-1:�E����</returns>
    public static int CalcNearRotation(Vector3 own, Vector3 target)
    {
        own.y = 0;
        target.y = 0;

        var vY = new Vector3(0, 1, 0);
        var ownRot = Quaternion.LookRotation(own, vY).eulerAngles.y;
        var targetRot = Quaternion.LookRotation(target, vY).eulerAngles.y;

        // �����̂ق����������p�x�ɂ���
        if (targetRot < ownRot) targetRot += 360f;

        var rot = targetRot - ownRot;
        // 180�x�ȉ��Ȃ獶����
        return rot <= 180f ? 1 : -1;
    }
}
