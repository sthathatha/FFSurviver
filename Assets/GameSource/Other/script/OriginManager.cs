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

    #endregion

    /// <summary>
    /// ������
    /// </summary>
    private void Start()
    {
        optionUI.Close(true);
    }
}
