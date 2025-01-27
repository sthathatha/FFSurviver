using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ‚¨q‚ËÒ€–Ú
/// </summary>
public class UIWantedMaterial : UIMaterialBase
{
    public Image icon;
    public Image defeated;

    /// <summary>
    /// ‚¨q‚ËÒ•\¦
    /// </summary>
    /// <param name="_defeated">“¢”°Ï‚İ</param>
    public void ShowWanted(bool _defeated)
    {
        defeated.gameObject.SetActive(_defeated);
    }
}
