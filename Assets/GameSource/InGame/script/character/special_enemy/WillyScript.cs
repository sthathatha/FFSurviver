using System.Collections;
using UnityEngine;

/// <summary>
/// ウイリー
/// </summary>
public class WillyScript : EnemyScriptBase
{
    #region 定数

    /// <summary>処理負荷軽減のため移動チェックはｘ秒ごと</summary>
    private const float MOVE_CHECK_INTERVAL = 1f;
    /// <summary>加速度</summary>
    private const float MOVE_ACCEL = 15f;
    /// <summary>最高速度</summary>
    private const float MOVE_MAX_SPEED = 25f;
    /// <summary>転回速度</summary>
    private const float ROT_SPEED = 100f;

    #endregion

    #region メンバー

    /// <summary>移動フェーズ</summary>
    private enum MovePhase
    {
        /// <summary>突っ込む</summary>
        Dash,
        /// <summary>転回する前</summary>
        Dash2,
        /// <summary>減速＆転回</summary>
        Brake,
    }
    private MovePhase phase;

    /// <summary>移動方向判定タイマー</summary>
    private float checkTimer = -1f;

    /// <summary>移動ベクトル</summary>
    private Vector3 moveVector = Vector3.zero;

    /// <summary>転回時の回転方向</summary>
    private float brakeRot = 1f;

    #endregion

    #region 初期化

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();

        // 初期設定はプレイヤー位置に突進
        phase = MovePhase.Dash;
        checkTimer = MOVE_CHECK_INTERVAL;

        // 転回中はプレイヤー位置まで回転する
        var main = GameMainSystem.Instance;
        var pPos = main.GetPlayerCenter();
        var dist = pPos - transform.position;
        dist.y = 0f;

        moveVector = dist.normalized * MOVE_MAX_SPEED / 2f;

        // モデルの向き
        transform.rotation = Quaternion.LookRotation(moveVector, new Vector3(0, 1, 0));
    }

    #endregion

    #region 基底

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateCharacter()
    {
        var manager = ManagerSceneScript.GetInstance();
        var dt = manager.validDeltaTime;
        var main = GameMainSystem.Instance;

        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        // ｎ秒に１回位置更新
        if (checkTimer < 0f)
        {
            checkTimer = MOVE_CHECK_INTERVAL;

            if (phase == MovePhase.Dash)
            {
                // プレイヤー位置
                var pPos = main.GetPlayerCenter();
                var dist = pPos - transform.position;
                dist.y = 0;

                // ダッシュ中は通り過ぎるまで加速のみ
                if (Vector3.Dot(dist, moveVector) >= 0f) return;

                // 通り過ぎていたら転回待ちに移行
                phase = MovePhase.Dash2;

            }
            else if (phase == MovePhase.Dash2)
            {
                var pPos = main.GetPlayerCenter();
                var dist = pPos - transform.position;
                dist.y = 0;

                // 回転方向を今近い方に決定
                brakeRot = FieldUtil.CalcNearRotation(moveVector, dist) * ROT_SPEED;
                //
                phase = MovePhase.Brake;
            }
        }
        else
        {

            if (phase == MovePhase.Dash || phase == MovePhase.Dash2)
            {
                checkTimer -= dt;
                // ひたすら直進
                // ある程度まで加速
                var spd = moveVector.magnitude;

                if (spd < MOVE_MAX_SPEED)
                {
                    var newSpd = spd + MOVE_ACCEL * dt;
                    moveVector *= newSpd / spd;
                }
            }
            else
            {
                // 転回中はプレイヤー位置まで回転する
                var pPos = main.GetPlayerCenter();
                var dist = pPos - transform.position;

                // 間の角度が小さければ突進モードになる
                var rotDelta = brakeRot * dt;
                var rotQuat = Quaternion.Euler(0, rotDelta, 0);
                var nextVector = rotQuat * moveVector;
                if (FieldUtil.CalcNearRotation(moveVector, dist) != FieldUtil.CalcNearRotation(nextVector, dist))
                {
                    phase = MovePhase.Dash;
                    return;
                }

                // 減速しながら回転
                var spd = nextVector.magnitude;
                if (spd > MOVE_MAX_SPEED / 5f)
                {
                    var newSpd = spd - MOVE_ACCEL * dt;
                    nextVector *= newSpd / spd;
                }

                moveVector = nextVector;

                // モデルの向き
                transform.rotation = Quaternion.LookRotation(moveVector, new Vector3(0, 1, 0));
            }
        }

        // 移動
        transform.position += moveVector * dt;
    }

    /// <summary>
    /// 死亡時
    /// </summary>
    protected override void DamageDeath()
    {
        base.DamageDeath();
        Destroy(gameObject);
    }

    #endregion
}
