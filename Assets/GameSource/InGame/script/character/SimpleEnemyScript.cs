using UnityEngine;

/// <summary>
/// 単純に近づいてくるだけの敵
/// </summary>
public class SimpleEnemyScript : CharacterScript
{
    #region 定数

    /// <summary>処理負荷軽減のため移動チェックはｘ秒ごと</summary>
    private const float MOVE_CHECK_INTERVAL = 1f;

    #endregion

    #region メンバー

    public float float_height = 0.6f;
    public float move_speed = 5f;

    /// <summary>移動方向判定タイマー</summary>
    private float checkTimer = -1f;
    private Vector3 moveVector = Vector3.zero;

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateCharacter()
    {
        var main = GameMainSystem.Instance;

        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        if (checkTimer < 0f)
        {
            checkTimer = MOVE_CHECK_INTERVAL;
            // rigidBody使わない

            // プレイヤー位置
            var pPos = main.playerScript.transform.position;
            var dist = pPos - transform.position;
            dist.y = 0;
            if (dist.sqrMagnitude < 0.1f) return;   // 充分近い時は何もしない

            // 単位ベクトル
            dist = dist.normalized;

            // 移動ベクトル設定
            moveVector = dist * move_speed;

            // モデル向き設定
            transform.rotation = Quaternion.LookRotation(dist, new Vector3(0, 1, 0));
        }
        else
        {
            checkTimer -= Time.deltaTime;
        }


        // 移動
        transform.position += moveVector * Time.deltaTime;

    }
}
