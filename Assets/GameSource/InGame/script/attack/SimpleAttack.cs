using UnityEngine;

/// <summary>
/// シンプルに直線移動して消える攻撃
/// </summary>
public class SimpleAttack : AttackParameter
{
    #region メンバー

    /// <summary>あたったら消える</summary>
    public bool is_hit_disappear = false;
    /// <summary>消える時間</summary>
    public float disappear_time = 1f;
    /// <summary>飛ぶスピード</summary>
    public float speed = 1f;

    /// <summary>移動方向</summary>
    private Vector3 move_direction;
    /// <summary>生存時間</summary>
    private float valid_time = 0f;

    #endregion

    #region Unity既定

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        var dt = OriginManager.Instance.inGameDeltaTime;
        valid_time -= dt;
        if (valid_time <= 0f ||
            (is_hit_disappear && isHitted))
        {
            Destroy(gameObject);
            return;
        }

        transform.position += move_direction * dt;
    }

    #endregion

    #region 操作

    /// <summary>
    /// 開始
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="direction"></param>
    public void Shoot(Vector3 startPos, Vector3 direction)
    {
        valid_time = disappear_time;
        transform.position = startPos;
        move_direction = direction.normalized * speed;
        gameObject.SetActive(true);
    }

    #endregion
}
