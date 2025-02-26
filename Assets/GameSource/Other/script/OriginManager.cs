using System.Collections;
using UnityEngine;

/// <summary>
/// 独自マネージャー
/// </summary>
public class OriginManager : MonoBehaviour
{
    /// <summary>
    /// InstanceはManagerSceneから取得する
    /// </summary>
    public static OriginManager Instance
    {
        get
        {
            return ManagerSceneScript.GetInstance().GetComponent<OriginManager>();
        }
    }

    #region メンバー

    /// <summary>オプション</summary>
    public OptionUI optionUI;

    /// <summary>ゲーム内有効時間</summary>
    public float inGameDeltaTime { get; private set; } = 0f;
    /// <summary>ゲーム内時間速度</summary>
    public float inGameTimeSpeed = 1f;

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        optionUI.Close(true);
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        var manager = GetComponent<ManagerSceneScript>();

        if (inGameTimeSpeed > 0f) inGameDeltaTime = inGameTimeSpeed * manager.validDeltaTime;
        else inGameDeltaTime = 0f;
    }

    /// <summary>
    /// yield returnに使うゲーム内用の待機
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator WaitIngameTime(float time)
    {
        var t = time;
        while (t > 0f)
        {
            yield return null;
            t -= inGameDeltaTime;
        }
    }
}
