using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ねずみ花火
/// </summary>
public class FireworksAttack : AttackParameter
{
    #region メンバー

    /// <summary>基本座標</summary>
    private Vector3 basePos;
    /// <summary>移動速度</summary>
    private Vector3 velocity;

    /// <summary>回転</summary>
    private float rot;
    /// <summary>回転速度</summary>
    private const float rotSpeed = Mathf.PI * 6f;

    /// <summary>距離</summary>
    private float dist;

    /// <summary>消える時間</summary>
    private float valid_time;

    /// <summary>基本移動速度</summary>
    private const float moveSpeed = 15f;

    #endregion

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="_initPos"></param>
    /// <param name="_rot"></param>
    public void Shoot(Vector3 _initPos, float _rot)
    {
        basePos = _initPos;
        transform.position = _initPos;
        velocity = new Vector3(Mathf.Cos(_rot) * moveSpeed * scaleRate, 0, Mathf.Sin(_rot) * moveSpeed * scaleRate);

        rot = Util.RandomFloat(0f, Mathf.PI * 2f);
        dist = 1.5f * scaleRate;
        valid_time = 5f;

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        var dt = OriginManager.Instance.inGameDeltaTime;
        valid_time -= dt;
        if (valid_time <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        // 移動計算
        basePos += velocity * dt;

        rot -= rotSpeed * dt;
        rot = Util.GetNormalRadian(rot);
        var distPos = new Vector3(Mathf.Cos(rot) * dist, 0, Mathf.Sin(rot) * dist);

        transform.position = basePos + distPos;
    }
}
