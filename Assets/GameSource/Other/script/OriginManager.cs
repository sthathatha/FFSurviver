using System.Collections;
using UnityEngine;

/// <summary>
/// 独自マネージャー
/// </summary>
public class OriginManager : MonoBehaviour
{
    #region メンバー

    /// <summary>オプション</summary>
    public OptionUI optionUI;

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        optionUI.Close(true);
    }
}
