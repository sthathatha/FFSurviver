using System.Collections;
using UnityEngine;

/// <summary>
/// 横から出てくるUIの基本
/// </summary>
public class AppearUIBase : MonoBehaviour
{
    #region メンバー

    /// <summary>隠れる時のX座標</summary>
    public float hide_X = 0f;
    /// <summary>アクティブX座標</summary>
    public float show_X = 100f;
    /// <summary>表示時間</summary>
    public float show_time = 0.15f;

    /// <summary>移動座標</summary>
    private DeltaFloat moveX;

    /// <summary>瞬時表示</summary>
    protected bool isImmediate;

    /// <summary>初期化処理済み</summary>
    private bool isInitialized = false;

    /// <summary>稼働中</summary>
    public bool isActive { get; private set; } = false;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Initialize()
    {
        moveX = new DeltaFloat();
        InitStart();
        isInitialized = true;
    }

    /// <summary>
    /// 必ず１回やる初期化処理
    /// </summary>
    virtual protected void InitStart() { }

    #endregion

    #region 制御

    /// <summary>
    /// 開く
    /// </summary>
    /// <param name="_immediate">即開く</param>
    /// <returns></returns>
    public IEnumerator Open(bool _immediate = false)
    {
        var manager = ManagerSceneScript.GetInstance();
        var rect = GetComponent<RectTransform>();
        var basePos = rect.anchoredPosition;

        // 最初非表示の場合Startが走ってない場合があるので
        if (!isInitialized)
        {
            // Activeでないとできない初期化処理がありえるので、消えた場所に設定して起動状態にしておく
            basePos.x = hide_X;
            rect.anchoredPosition = basePos;
            gameObject.SetActive(true);

            Initialize();
            yield return null;
        }

        isActive = true;
        isImmediate = _immediate;

        if (_immediate)
        {
            // すぐ出てくる
            basePos.x = show_X;
            rect.anchoredPosition = basePos;
            InitOpen();
            gameObject.SetActive(true);
            yield return null;
        }
        else
        {
            // 移動
            InitOpen();
            gameObject.SetActive(true);

            moveX.Set(hide_X);
            moveX.MoveTo(show_X, show_time, DeltaFloat.MoveType.LINE);
            while (moveX.IsActive())
            {
                moveX.Update(manager.validDeltaTime);
                basePos.x = moveX.Get();
                rect.anchoredPosition = basePos;

                yield return null;
            }
        }

        StartCoroutine(UpdateMenu());
    }

    /// <summary>
    /// 右へ閉じる
    /// </summary>
    /// <param name="_immediate"></param>
    /// <returns></returns>
    public IEnumerator Close(bool _immediate = false)
    {
        var manager = ManagerSceneScript.GetInstance();
        var rect = GetComponent<RectTransform>();
        var basePos = rect.anchoredPosition;

        if (_immediate)
        {
            // すぐ消える
            basePos.x = hide_X;
            rect.anchoredPosition = basePos;
        }
        else
        {
            // 移動
            isImmediate = _immediate;

            moveX.Set(show_X);
            moveX.MoveTo(hide_X, show_time, DeltaFloat.MoveType.LINE);
            while (moveX.IsActive())
            {
                moveX.Update(manager.validDeltaTime);
                basePos.x = moveX.Get();
                rect.anchoredPosition = basePos;

                yield return null;
            }
        }

        isActive = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 開く前の表示更新
    /// </summary>
    virtual protected void InitOpen() { }

    /// <summary>
    /// 開ききったあとの更新コルーチン
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator UpdateMenu() { yield break; }

    #endregion


}
