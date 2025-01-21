using System.Collections;
using UnityEngine;

/// <summary>
/// �Ǝ��}�l�[�W���[
/// </summary>
public class OriginManager : MonoBehaviour
{
    #region �����o�[

    /// <summary>�I�v�V����</summary>
    public OptionUI optionUI;

    /// <summary>�Q�[�����L������</summary>
    public float inGameDeltaTime { get; private set; } = 0f;
    /// <summary>�Q�[�������ԑ��x</summary>
    public float inGameTimeSpeed = 1f;

    #endregion

    /// <summary>
    /// ������
    /// </summary>
    private void Start()
    {
        optionUI.Close(true);
    }

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        var manager = GetComponent<ManagerSceneScript>();

        if (inGameTimeSpeed > 0f) inGameDeltaTime = inGameTimeSpeed * manager.validDeltaTime;
        else inGameDeltaTime = 0f;
    }
}
