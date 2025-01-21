using System.Collections;
using UnityEngine;

/// <summary>
/// ゲーム内メニュー
/// </summary>
public class UIInGameMenu : MonoBehaviour
{
    #region メンバー

    public GameObject blackBg;
    public UIMainCommand mainCommand;

    /// <summary>実行中</summary>
    public bool isActive { get; private set; }

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        StartCoroutine(mainCommand.Close(true));
        blackBg.SetActive(false);
        isActive = false;
    }

    /// <summary>
    /// 開く
    /// </summary>
    public void Open()
    {
        isActive = true;
        // ゲーム停止
        var manager = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();
        manager.inGameTimeSpeed = 0f;

        blackBg.SetActive(true);
        StartCoroutine(MenuCoroutine());
    }

    /// <summary>
    /// メニュー動作
    /// </summary>
    /// <returns></returns>
    private IEnumerator MenuCoroutine()
    {
        // メインコマンドの処理に任せる
        yield return mainCommand.Open();
        yield return new WaitWhile(() => mainCommand.isActive);

        isActive = false;
        // 閉じる
        blackBg.SetActive(false);

        // ゲーム再起動
        var manager = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();
        manager.inGameTimeSpeed = 1f;
    }
}
