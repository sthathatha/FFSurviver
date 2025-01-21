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

        // ��Active����Active�ɂ���Ə���������Ă邩��x�����o��
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
    /// �J�[�\����~�\��
    /// </summary>
    public void Cursor_Stop()
    {
        if (cursor == null) return;

        cursor.SetInteger("state", 1);
    }
}
