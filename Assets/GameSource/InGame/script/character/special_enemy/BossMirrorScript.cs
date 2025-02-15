using System.Collections;
using UnityEngine;

/// <summary>
/// 鏡の怪物
/// </summary>
public class BossMirrorScript : BossScriptBase
{
    /// <summary>移動速度</summary>
    const float MOVE_SPEED = 1f;

    /// <summary>連続発射</summary>
    const int ATK_REPEAT = 5;

    /// <summary>攻撃間隔短</summary>
    const float ATK_INTERVAL1 = 0.3f;
    /// <summary>連続攻撃の次</summary>
    const float ATK_INTERVAL2 = 4f;

    /// <summary>武器制御クラス</summary>
    private BossMirrorAttack attackSystem;

    /// <summary>連続攻撃回数</summary>
    private int attackCount = 0;
    /// <summary>攻撃待ち時間</summary>
    private float attackTimer = 0f;

    /// <summary>
    /// キャラクター初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();
        anim.SetBool("boss", true);

        // パラメータ設定
        attackCount = 0;
        attackTimer = ATK_INTERVAL2;

        // 武器読み込み
        var manager = ManagerSceneScript.GetInstance();
        manager.LoadSubScene("GameWeaponBossMirror");
        yield return new WaitWhile(() => manager.IsLoadingSubScene());
        attackSystem = manager.GetSubSceneList().Find(sub => sub is BossMirrorAttack) as BossMirrorAttack;

        yield return TooFarPlayer(GameMainSystem.Instance.GetPlayerCenter());
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
        var system = GameMainSystem.Instance;
        var origin = OriginManager.Instance;
        const float rotSpeed = Mathf.PI * 0.2f;

        // 常にプレイヤーを探す
        var deltaRot = rotSpeed * origin.inGameDeltaTime;
        var dist = system.GetPlayerCenter() - transform.position;
        dist.y = 0f;
        dist = dist.normalized;
        var sign = FieldUtil.CalcNearRotation(forwardVec, dist);

        // 十分近い場合は終わり
        if (Mathf.Acos(Vector3.Dot(dist, forwardVec)) <= deltaRot)
        {
            forwardVec = dist;
        }
        else
        {
            // 今回回転角度
            var dt = deltaRot * Mathf.Rad2Deg * sign;
            forwardVec = Quaternion.Euler(0, dt, 0) * forwardVec;
        }
        transform.rotation = Quaternion.LookRotation(forwardVec, new Vector3(0, 1, 0));

        // 移動
        transform.position += forwardVec * MOVE_SPEED * origin.inGameDeltaTime;

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
        GameMainSystem.Instance.prm_Game.Defeated_Boss1 = true;

        // 武器クラス削除
        ManagerSceneScript.GetInstance().DeleteSubScene(attackSystem);
        attackSystem = null;

        //todo:消える演出
    }

    /// <summary>
    /// プレイヤーから離れすぎた時
    /// </summary>
    /// <param name="playerCenter"></param>
    protected override IEnumerator TooFarPlayer(Vector3 playerCenter)
    {
        base.TooFarPlayer(playerCenter);

        // 適当な位置に出現
        var app = GetAppearPoint();
        app.y = transform.position.y;
        transform.position = app;

        RotToPlayer();
        yield break;
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void AttackControl()
    {
        var dt = OriginManager.Instance.inGameDeltaTime;

        attackTimer -= dt;
        if (attackTimer > 0) return;

        // 攻撃
        var pCenter = GameMainSystem.Instance.GetPlayerCenter();
        attackSystem.Shoot(transform.position, pCenter, strength_rate);

        attackCount++;
        if (attackCount >= ATK_REPEAT)
        {
            attackCount = 0;
            attackTimer = ATK_INTERVAL2;
        }
        else
        {
            attackTimer = ATK_INTERVAL1;
        }
    }
}
