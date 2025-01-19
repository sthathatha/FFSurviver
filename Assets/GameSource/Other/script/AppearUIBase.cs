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
    public float show_time = 0.3f;

    /// <summary>移動座標</summary>
    private DeltaFloat moveX;

    /// <summary>瞬時表示</summary>
    protected bool isImmediate;

    #endregion

    #region 基底

    /// <summary>
    /// 表示
    /// </summary>
    private void Start()
    {
        moveX = new DeltaFloat();
    }

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
        var basePos = transform.localPosition;

        if (_immediate)
        {
            // すぐ出てくる
            basePos.x = show_X;
            transform.localPosition = basePos;
            gameObject.SetActive(true);
        }
        else
        {
            // 移動
            isImmediate = _immediate;
            InitMenu();
            gameObject.SetActive(true);

            moveX.Set(hide_X);
            moveX.MoveTo(show_X, show_time, DeltaFloat.MoveType.LINE);
            while (moveX.IsActive())
            {
                moveX.Update(manager.validDeltaTime);
                basePos.x = moveX.Get();
                transform.localPosition = basePos;

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
        var basePos = transform.localPosition;

        if (_immediate)
        {
            // すぐ消える
            basePos.x = hide_X;
            transform.localPosition = basePos;
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
                transform.localPosition = basePos;

                yield return null;
            }
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 開く前の表示更新
    /// </summary>
    virtual protected void InitMenu() { }

    /// <summary>
    /// 開ききったあとの更新コルーチン
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator UpdateMenu() { yield break; }

    #endregion


}
