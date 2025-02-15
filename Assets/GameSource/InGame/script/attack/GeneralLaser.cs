using UnityEngine;

/// <summary>
/// レーザー表示物
/// </summary>
public class GeneralLaser : MonoBehaviour
{
    /// <summary>
    /// 表示
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 非表示
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 位置設定
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    public void SetLine(Vector3 v1, Vector3 v2)
    {
        // 位置
        transform.position = (v1 + v2) / 2f;

        // 長さ
        var dist = v2 - v1;
        var length = dist.magnitude;
        transform.localScale = new Vector3(1, 1, length);

        // 向き
        transform.rotation = Quaternion.LookRotation(dist, new Vector3(0, 1, 0));
    }
}
