using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// クソ花のタネ攻撃
/// </summary>
public class BossSeedAttack : AttackParameter
{
    /// <summary>最初の距離</summary>
    public const float START_DISTANCE = 40f;
    /// <summary>速度</summary>
    private const float SPEED = START_DISTANCE / 2.5f;
    /// <summary>消える時間</summary>
    private const float END_TIME = 5f;

    /// <summary>飛ぶ方向</summary>
    private Vector3 direction;

    /// <summary>待ち時間</summary>
    private float timer;

    /// <summary>回転要素</summary>
    private Vector3 rotLine;

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <param name="waitTime"></param>
    public void Shoot(Vector3 pos, Vector3 dir, float waitTime)
    {
        transform.position = pos;
        direction = dir.normalized * SPEED;
        timer = waitTime;

        // 回転ランダム
        var randRot = Quaternion.Euler(Util.RandomFloat(0, 359f), Util.RandomFloat(0, 359f), Util.RandomFloat(0, 359f));
        transform.rotation = randRot;
        // Z軸を回転軸として回る
        rotLine = randRot * new Vector3(0, 0, 1);

        gameObject.SetActive(true);
        StartCoroutine(UpdateCoroutine());
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateCoroutine()
    {
        var origin = OriginManager.Instance;
        // 待機
        while (timer > 0)
        {
            timer -= origin.inGameDeltaTime;
            yield return null;
        }

        timer = END_TIME;
        // 発射
        while (timer > 0)
        {
            timer -= origin.inGameDeltaTime;
            transform.position = transform.position + direction * origin.inGameDeltaTime;
            yield return null;
        }

        // 消える
        Destroy(gameObject);
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        var dt = OriginManager.Instance.inGameDeltaTime;
        // 常に回転している
        var q = Quaternion.AngleAxis(180f * dt, rotLine);
        transform.rotation = q * transform.rotation;
    }
}
