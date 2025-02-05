using UnityEngine;
using static PlayerScript;

/// <summary>
/// オプション
/// </summary>
public class ChildOption : MonoBehaviour
{
    #region 定数

    /// <summary>CharacterScriptから移植　落下加速度</summary>
    protected const float FALL_G = 25.6f;

    /// <summary>最大距離</summary>
    private const float MAX_DIST = 5f;
    /// <summary>最大距離2乗</summary>
    private const float MAX_DISTSQR = MAX_DIST * MAX_DIST;

    /// <summary>高さ基準</summary>
    private const float FLOAT_HEIGHT = 0.6f;

    #endregion

    #region メンバー

    /// <summary>追尾する親</summary>
    private Transform parentOption;

    /// <summary>Y以外の速度</summary>
    private Vector3 velocity;

    /// <summary>接地判定用Ray</summary>
    private Ray ground_ray;

    /// <summary>空中にいる</summary>
    private bool isFloating = false;

    #endregion

    #region 初期化

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="_parent"></param>
    public void InitOption(Transform _parent)
    {
        ground_ray = new Ray();
        ground_ray.direction = new Vector3(0, -1f, 0);

        parentOption = _parent;
        velocity = Vector3.zero;
        var initPos = _parent.transform.position;
        initPos.y = 0f;
        initPos.x += Util.RandomFloatSin(-MAX_DIST / 2f, MAX_DIST / 2f);
        initPos.z += Util.RandomFloatSin(-MAX_DIST / 2f, MAX_DIST / 2f);

        transform.position = initPos;
        isFloating = false;

        gameObject.SetActive(true);
    }

    #endregion

    #region 更新処理

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        var maxV = GameMainSystem.Instance.prm_Player.GetSpeedVelocity();

        var dist = parentOption.position - transform.position;
        if (dist.sqrMagnitude > MAX_DISTSQR)
        {
            // 離れていたら補正
            var normalDist = dist.normalized;
            var limitDist = normalDist * MAX_DIST;
            transform.position = parentOption.position - limitDist;

            // 速度に加算
            var addV = normalDist;
            velocity += addV;
            if (velocity.magnitude > maxV)
            {
                velocity = velocity.normalized * maxV;
            }
        }

        MoveControl();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void MoveControl()
    {
        var dt = OriginManager.Instance.inGameDeltaTime;
        var old_y = transform.position.y;
        var newPos = transform.position;

        // 垂直移動処理
        if (isFloating)
        {
            // 下に加速
            velocity.y -= FALL_G * dt;

            newPos += velocity * dt;
            // 落ち過ぎたら上に出る
            if (newPos.y < -20f) newPos.y = 20f;

            // 地面判定
            var groundHit = GroundSearch(newPos, old_y - newPos.y, ground_ray, out GameGround ground, out RaycastHit hitInfo);

            // 物を踏んだら着地
            if (groundHit && (old_y - newPos.y) > 0f)
            {
                velocity.y = 0f;
                isFloating = false;

                newPos = hitInfo.point;
            }
            transform.position = newPos;
        }
        else
        {
            newPos += velocity * dt;
            var groundHit = GroundSearch(newPos, 0f, ground_ray, out GameGround ground, out RaycastHit hitInfo);

            // 地面が無くなったら落下
            if (!groundHit)
            {
                isFloating = true;
            }

            transform.position = newPos;
        }

        // 落ちすぎない
        if (transform.position.y < FLOAT_HEIGHT)
        {
            transform.position = new Vector3(transform.position.x, FLOAT_HEIGHT, transform.position.z);
        }
    }

    #endregion

    #region 取得

    /// <summary>
    /// 基準位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCenter()
    {
        return transform.position + new Vector3(0, FLOAT_HEIGHT, 0);
    }

    #endregion
}
