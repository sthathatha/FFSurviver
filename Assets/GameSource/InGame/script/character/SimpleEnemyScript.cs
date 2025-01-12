using UnityEngine;

/// <summary>
/// 単純に近づいてくるだけの敵
/// </summary>
public class SimpleEnemyScript : EnemyScriptBase
{
    #region 定数

    /// <summary>処理負荷軽減のため移動チェックはｘ秒ごと</summary>
    private const float MOVE_CHECK_INTERVAL = 1f;

    #endregion

    #region メンバー

    public float float_height = 0.6f;
    public float move_speed = 5f;
    public float check_interval = MOVE_CHECK_INTERVAL;

    /// <summary>移動方向判定タイマー</summary>
    private float checkTimer = -1f;
    private Vector3 moveVector = Vector3.zero;

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();

        var manager = ManagerSceneScript.GetInstance();
        var dt = manager.validDeltaTime;
        var main = GameMainSystem.Instance;

        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        if (checkTimer < 0f)
        {
            checkTimer = check_interval;

            // プレイヤー位置
            var pPos = main.GetPlayerCenter();
            var dist = pPos - transform.position;
            dist.y = 0;
            if (dist.sqrMagnitude < 1f)
            {
                // 充分近い時は何もしない
                moveVector = Vector3.zero;
                return;
            }

            // ランダム回転幅±30度
            var randRot = Quaternion.Euler(0, Util.RandomFloat(-30f, 30f), 0);
            dist = randRot * dist;

            // 単位ベクトル
            dist = dist.normalized;

            // 移動ベクトル設定
            moveVector = dist * move_speed;

            // モデル向き設定
            transform.rotation = Quaternion.LookRotation(dist, new Vector3(0, 1, 0));
        }
        else
        {
            checkTimer -= dt;
        }


        // 移動
        transform.position += moveVector * dt;
    }

    /// <summary>
    /// プレイヤーから離れすぎた時
    /// </summary>
    /// <param name="playerCenter"></param>
    protected override void TooFarPlayer(Vector3 playerCenter)
    {
        base.TooFarPlayer(playerCenter);

        // 消える
        Destroy(gameObject);
    }

    /// <summary>
    /// 死亡時
    /// </summary>
    protected override void DamageDeath()
    {
        base.DamageDeath();
        Destroy(gameObject);
    }
}
