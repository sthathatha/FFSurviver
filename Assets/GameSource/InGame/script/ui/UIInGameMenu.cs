using System.Collections;
using UnityEngine;

/// <summary>
/// �Q�[�������j���[
/// </summary>
public class UIInGameMenu : MonoBehaviour
{
    #region �����o�[

    public GameObject blackBg;
    public UIMainCommand mainCommand;

    /// <summary>���s��</summary>
    public bool isActive { get; private set; }

    #endregion

    /// <summary>
    /// ������
    /// </summary>
    private void Start()
    {
        StartCoroutine(mainCommand.Close(true));
        blackBg.SetActive(false);
        isActive = false;
    }

    /// <summary>
    /// �J��
    /// </summary>
    public void Open()
    {
        isActive = true;
        // �Q�[����~
        var manager = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();
        manager.inGameTimeSpeed = 0f;

        blackBg.SetActive(true);
        StartCoroutine(MenuCoroutine());
    }

    /// <summary>
    /// ���j���[����
    /// </summary>
    /// <returns></returns>
    private IEnumerator MenuCoroutine()
    {
        // ���C���R�}���h�̏����ɔC����
        yield return mainCommand.Open();
        yield return new WaitWhile(() => mainCommand.isActive);

        isActive = false;
        // ����
        blackBg.SetActive(false);

        // �Q�[���ċN��
        var manager = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();
        manager.inGameTimeSpeed = 1f;
    }
}
