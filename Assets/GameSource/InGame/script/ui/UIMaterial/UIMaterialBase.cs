using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// UI�A�C�e���P�Ԃ�̋���
/// </summary>
public class UIMaterialBase : MonoBehaviour
{
    public Animator cursor;

    /// <summary>
    /// �J�[�\����\��
    /// </summary>
    public void Cursor_Hide()
    {
        if (cursor == null) return;

        cursor.gameObject.SetActive(false);
    }

    /// <summary>
    /// �J�[�\���ʏ�
    /// </summary>
    public void Cursor_Idle()
    {
        if (cursor == null) return;

        cursor.gameObject.SetActive(true);
        cursor.SetInteger("state", 0);
    }

    /// <summary>
    /// �J�[�\����~�\��
    /// </summary>
    public void Cursor_Stop()
    {
        if (cursor == null) return;

        cursor.gameObject.SetActive(true);
        cursor.SetInteger("state", 1);
    }
}
