using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���q�ˎҍ���
/// </summary>
public class UIWantedMaterial : UIMaterialBase
{
    public Image icon;
    public Image defeated;

    /// <summary>
    /// ���q�ˎҕ\��
    /// </summary>
    /// <param name="_defeated">�����ς�</param>
    public void ShowWanted(bool _defeated)
    {
        defeated.gameObject.SetActive(_defeated);
    }
}
