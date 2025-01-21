using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// UIアイテム１個ぶんの共通
/// </summary>
public class UIMaterialBase : MonoBehaviour
{
    public Animator cursor;

    /// <summary>
    /// カーソル非表示
    /// </summary>
    public void Cursor_Hide()
    {
        if (cursor == null) return;

        cursor.gameObject.SetActive(false);
    }

    /// <summary>
    /// カーソル通常
    /// </summary>
    public void Cursor_Idle()
    {
        if (cursor == null) return;

        // 非ActiveからActiveにすると初期化されてるから警告が出る
        if (cursor.gameObject.activeInHierarchy)
        {
            cursor.SetInteger("state", 0);
        }
        else
        {
            cursor.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// カーソル停止表示
    /// </summary>
    public void Cursor_Stop()
    {
        if (cursor == null) return;

        cursor.SetInteger("state", 1);
    }
}
