using System.Collections;
using UnityEngine;

/// <summary>
/// 鏡の怪物
/// </summary>
public class BossMirrorScript : BossScriptBase
{
    /// <summary>移動速度</summary>
    const float MOVE_SPEED = 1f;

    /// <summary>
    /// キャラクター初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();
        anim.SetBool("boss", true);

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
    }

    /// <summary>
    /// 死亡演出
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator DeathAnim()
    {
        yield return base.DeathAnim();
        GameMainSystem.Instance.prm_Game.Defeated_Boss1 = true;

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
}
