using System.Collections;
using UnityEngine;

/// <summary>
/// ボス共通処理
/// </summary>
public class BossScriptBase : EnemyScriptBase
{
    #region 定数

    #endregion

    #region メンバー

    /// <summary>ボス</summary>
    public UIHpGauge uiHp;

    /// <summary>
    /// 状態
    /// </summary>
    protected enum State
    {
        Idle = 0,
        Death,
    }
    protected State state;

    /// <summary>向き</summary>
    protected Vector3 forwardVec;

    #endregion

    #region 初期化

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();
        state = State.Idle;
        forwardVec = new Vector3(0, 0, 1);

        uiHp.SetHP(hp, hp_max);
        uiHp.gameObject.SetActive(true);
    }

    #endregion

    /// <summary>
    /// ダメージ
    /// </summary>
    protected override void DamageHit()
    {
        base.DamageHit();
        uiHp.SetHP(hp, hp_max);
    }

    #region 死亡制御

    /// <summary>
    /// 死亡時
    /// </summary>
    protected override void DamageDeath()
    {
        base.DamageDeath();
        state = State.Death;

        StartCoroutine(DeathCoroutine());
    }

    /// <summary>
    /// 死亡処理コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathCoroutine()
    {
        // 消える
        yield return DeathAnim();

        uiHp.gameObject.SetActive(false);
        GameMainSystem.Instance.ActiveBossDefeat();
        Destroy(gameObject);
    }

    /// <summary>
    /// 派生・死亡演出
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DeathAnim() { yield break; }

    #endregion

    /// <summary>
    /// 離れすぎた時復活する位置を計算
    /// </summary>
    /// <returns></returns>
    public Vector3 GetAppearPoint()
    {
        var p = GameMainSystem.Instance.GetPlayerCenter();
        var dist = new Vector3(0, 0, FieldUtil.ENEMY_POP_DISTANCE);
        p += Quaternion.Euler(0, Util.RandomFloat(0, 359f), 0) * dist;

        return p;
    }

    /// <summary>
    /// プレイヤーの方を向く
    /// </summary>
    protected void RotToPlayer()
    {
        var dist = GameMainSystem.Instance.GetPlayerCenter() - transform.position;
        dist.y = 0;
        forwardVec = dist.normalized;
        transform.rotation = Quaternion.LookRotation(forwardVec, new Vector3(0, 1, 0));
    }
}
