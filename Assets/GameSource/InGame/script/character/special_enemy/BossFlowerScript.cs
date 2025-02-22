using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クソ花
/// </summary>
public class BossFlowerScript : BossScriptBase
{
    #region 定数

    /// <summary>攻撃間隔</summary>
    const float ATK_INTERVAL = 9f;
    /// <summary>タネの一周の数</summary>
    const int SEED_COUNT = 40;
    /// <summary>タネ1周作る時間</summary>
    const float SEED_CREATE_TIME = 1.5f;
    /// <summary>タネ生成してから動き出す時間</summary>
    const float SEED_WAIT_TIME = 1f;
    /// <summary>タネ1個の時間</summary>
    const float SEED_INTERVAL = SEED_CREATE_TIME / SEED_COUNT;
    /// <summary>タネ1個の角度</summary>
    const float SEED_INT_ROT = 360f / SEED_COUNT;

    /// <summary>消える時間</summary>
    const float DOWN_TIME = 0.3f;
    /// <summary>出てから降りる時間</summary>
    const float ACTIVE_TIME = 40f;
    /// <summary>出てくる時間</summary>
    const float UP_TIME = 0.3f;

    /// <summary>下のY座標</summary>
    const float DOWN_Y = -2.7f;
    /// <summary>上のY座標</summary>
    const float UP_Y = 0f;

    #endregion

    #region パラメータ

    /// <summary>攻撃</summary>
    public BossSeedAttack atkTemplate;

    /// <summary>攻撃待ち時間</summary>
    private float attackTimer = 0f;

    /// <summary>タネ作成数</summary>
    private int seedCount = 0;

    /// <summary>タネ動き出すまでの時間</summary>
    private float seedWait = 0f;

    /// <summary>タネ狙い位置</summary>
    private Vector3 seedTarget;
    /// <summary>タネ位置差分</summary>
    private Vector3 seedDir;

    /// <summary>
    /// 動き
    /// </summary>
    private enum Phase
    {
        Hide = 0,
        Upping,
        Active,
        Downing,
    }
    private Phase phase;
    private float moveTimer = 0f;

    #endregion

    #region 基底

    /// <summary>
    /// キャラクター初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();

        // パラメータ設定
        attackTimer = ATK_INTERVAL / 3f;
        seedCount = 0;

        //初期設定
        CalcAppearPos();
        moveTimer = UP_TIME;
        phase = Phase.Upping;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();

        // 移動処理
        MoveControl();

        // 攻撃処理
        AttackControl();
    }

    /// <summary>
    /// 死亡演出
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator DeathAnim()
    {
        yield return base.DeathAnim();
        GameMainSystem.Instance.prm_Game.Defeated_Boss2 = true;

        //todo:消える演出
    }

    /// <summary>
    /// プレイヤーから離れすぎた時
    /// </summary>
    /// <param name="playerCenter"></param>
    protected override IEnumerator TooFarPlayer(Vector3 playerCenter)
    {
        yield return base.TooFarPlayer(playerCenter);
        // 離れても特になにもしない
    }

    #endregion

    #region 移動制御

    /// <summary>
    /// 移動制御
    /// </summary>
    private void MoveControl()
    {
        var system = GameMainSystem.Instance;
        var origin = OriginManager.Instance;

        // 常にプレイヤー方向に向く
        var dist = system.GetPlayerCenter() - transform.position;
        dist.y = 0f;
        transform.rotation = Quaternion.LookRotation(dist, new Vector3(0, 1, 0));

        // 近くに居たらすぐ逃げる
        if (phase == Phase.Active && dist.sqrMagnitude < 200f)
            moveTimer -= origin.inGameDeltaTime * 5f;
        else
            moveTimer -= origin.inGameDeltaTime;

        if (phase == Phase.Hide)
        {
            if (moveTimer < 0f)
            {
                // 現れる
                CalcAppearPos();
                moveTimer = UP_TIME;
                phase = Phase.Upping;
            }
        }
        else if (phase == Phase.Active)
        {
            if (moveTimer < 0f)
            {
                // 消える
                moveTimer = DOWN_TIME;
                phase = Phase.Downing;
            }
        }
        else if (phase == Phase.Upping)
        {
            if (moveTimer < 0f)
            {
                // 現れ終わり
                phase = Phase.Active;
                moveTimer = ACTIVE_TIME;
            }
            else
            {
                // 出てくるY座標の計算と設定
                var y = Util.CalcBetweenFloat((UP_TIME - moveTimer) / UP_TIME, DOWN_Y, UP_Y);
                SetAllY(y);
            }
        }
        else
        {
            if (moveTimer < 0f)
            {
                // 消え終わり
                phase = Phase.Hide;
                moveTimer = DOWN_TIME;
            }
            else
            {
                // 消えるY座標の計算と設定
                var y = Util.CalcBetweenFloat(moveTimer / UP_TIME, DOWN_Y, UP_Y);
                SetAllY(y);
            }
        }
    }

    #endregion

    #region 攻撃制御

    /// <summary>
    /// 攻撃コントロール
    /// </summary>
    private void AttackControl()
    {
        var game = GameMainSystem.Instance;
        var dt = OriginManager.Instance.inGameDeltaTime;

        attackTimer -= dt;
        seedWait -= dt;
        if (attackTimer > 0) return;

        // 攻撃
        if (seedCount == 0)
        {
            // 初期値
            seedTarget = game.GetPlayerCenter();
            seedDir = seedTarget - transform.position;
            seedTarget.y = 1f;
            seedDir.y = 0f;
            seedDir = seedDir.normalized * BossSeedAttack.START_DISTANCE;
            seedWait = SEED_CREATE_TIME + SEED_WAIT_TIME;
        }
        else
        {
            // 位置回転と待ち時間更新
            seedDir = Quaternion.Euler(0f, SEED_INT_ROT, 0f) * seedDir;
        }

        // タネ作成
        var seed = Instantiate(atkTemplate, GameMainSystem.Instance.attackParent);
        seed.SetAttackRate(strength_rate);
        seed.Shoot(seedTarget - seedDir, seedDir, seedWait);

        // カウント
        seedCount++;
        if (seedCount >= SEED_COUNT)
        {
            seedCount = 0;
            attackTimer = ATK_INTERVAL;
        }
        else
        {
            attackTimer = SEED_INTERVAL;
        }
    }

    #endregion

    #region メソッド

    /// <summary>
    /// 現れる場所を設定
    /// </summary>
    private void CalcAppearPos()
    {
        // 自分の位置を計算
        transform.position = GetAppearPoint();

        // Y座標
        SetAllY(DOWN_Y);
    }

    /// <summary>
    /// Y座標設定
    /// </summary>
    /// <param name="y"></param>
    private void SetAllY(float y)
    {
        var p = transform.position;
        p.y = y;
        transform.position = p;
    }

    #endregion
}
