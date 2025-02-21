using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 水の怪物　本体
/// </summary>
public class BossWaterScript : BossScriptBase
{
    #region 定数

    /// <summary>連続発射</summary>
    const int ATK_REPEAT = 3;

    /// <summary>攻撃間隔短</summary>
    const float ATK_INTERVAL1 = 6f;
    /// <summary>連続攻撃の次</summary>
    const float ATK_INTERVAL2 = 6f;

    /// <summary>消える時間</summary>
    const float DOWN_TIME = 0.3f;
    /// <summary>出てから降りる時間</summary>
    const float ACTIVE_TIME = 40f;
    /// <summary>出てくる時間</summary>
    const float UP_TIME = 0.3f;

    /// <summary>下のY座標</summary>
    const float DOWN_Y = -8f;
    /// <summary>上のY座標</summary>
    const float UP_Y = 0f;

    #endregion

    #region パラメータ

    /// <summary>ダミー敵</summary>
    public AttackParameter dummyTemplate;

    /// <summary>攻撃</summary>
    public BossWaveAttack atkTemplate;

    /// <summary>ダミー</summary>
    private List<AttackParameter> dummies;

    /// <summary>連続攻撃回数</summary>
    private int attackCount = 0;
    /// <summary>攻撃待ち時間</summary>
    private float attackTimer = 0f;

    /// <summary>現在の中心に向けて攻撃</summary>
    private Vector3 centerPos;

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
    private float timer = 0f;

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
        attackCount = 0;
        attackTimer = ATK_INTERVAL2;
        centerPos = Vector3.zero;
        dummies = new List<AttackParameter>();

        //初期設定
        CalcAppearPos();
        timer = UP_TIME;
        phase = Phase.Upping;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
        var system = GameMainSystem.Instance;
        var origin = OriginManager.Instance;

        timer -= origin.inGameDeltaTime;

        if (phase == Phase.Hide)
        {
            if (timer < 0f)
            {
                // 現れる
                CalcAppearPos();
                timer = UP_TIME;
                phase = Phase.Upping;
            }
        }
        else if (phase == Phase.Active)
        {
            // 攻撃処理
            AttackControl();

            if (timer < 0f)
            {
                // 消える
                timer = DOWN_TIME;
                phase = Phase.Downing;
            }
        }
        else if (phase == Phase.Upping)
        {
            if (timer < 0f)
            {
                // 現れ終わり
                phase = Phase.Active;
                timer = ACTIVE_TIME;
            }
            else
            {
                // 出てくるY座標の計算と設定
                var y = Util.CalcBetweenFloat((UP_TIME - timer) / UP_TIME, DOWN_Y, UP_Y);
                SetAllY(y);
            }
        }
        else
        {
            if (timer < 0f)
            {
                // 消え終わり
                phase = Phase.Hide;
                timer = DOWN_TIME;
            }
            else
            {
                // 消えるY座標の計算と設定
                var y = Util.CalcBetweenFloat(timer / UP_TIME, DOWN_Y, UP_Y);
                SetAllY(y);
            }
        }
    }

    /// <summary>
    /// 死亡演出
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator DeathAnim()
    {
        yield return base.DeathAnim();
        GameMainSystem.Instance.prm_Game.Defeated_Boss3 = true;

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

    #region メソッド

    /// <summary>
    /// 現れる場所を設定
    /// </summary>
    private void CalcAppearPos()
    {
        DeleteAllDummy();

        // プレイヤー位置をターゲット中心
        centerPos = GameMainSystem.Instance.GetPlayerCenter();
        centerPos.y = 0f;

        // 自分の位置を計算
        transform.position = GetAppearPoint();

        // ダミーを作る、距離を回転してプレイヤーを囲むように
        var dmyCount = ((float)hp / hp_max) switch
        {
            < 0.4f => 7,
            < 0.6f => 5,
            < 0.8f => 3,
            _ => 1,
        };
        var dist = transform.position - centerPos;
        var rot = Quaternion.Euler(0, 360f / (dmyCount + 1), 0);
        for (var i = 0; i < dmyCount; i++)
        {
            dist = rot * dist;
            // ダミー
            var dmy = Instantiate(dummyTemplate, GameMainSystem.Instance.attackParent);
            dmy.transform.position = centerPos + dist;
            dmy.gameObject.SetActive(true);
            dummies.Add(dmy);
        }

        // Y座標
        SetAllY(DOWN_Y);
    }

    /// <summary>
    /// ダミーを消す
    /// </summary>
    private void DeleteAllDummy()
    {
        foreach (var d in dummies)
        {
            Destroy(d.gameObject);
        }
        dummies.Clear();
    }

    /// <summary>
    /// 自分とダミー全部のY座標設定
    /// </summary>
    /// <param name="y"></param>
    private void SetAllY(float y)
    {
        var p = transform.position;
        p.y = y;
        transform.position = p;

        foreach (var d in dummies)
        {
            p = d.transform.position;
            p.y = y;
            d.transform.position = p;
        }
    }

    /// <summary>
    /// 波攻撃1個作成
    /// </summary>
    private void AttackControl()
    {
        var dt = OriginManager.Instance.inGameDeltaTime;

        attackTimer -= dt;
        if (attackTimer > 0) return;

        // 攻撃
        var wave = Instantiate(atkTemplate, GameMainSystem.Instance.attackParent);
        wave.Shoot(centerPos);

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

    #endregion
}
