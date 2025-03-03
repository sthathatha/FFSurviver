using System.Collections;
using UnityEngine;

/// <summary>
/// 月の怪物（手が本体）
/// </summary>
public class BossMoonScript : BossScriptBase
{
    /// <summary>攻撃小インターバル</summary>
    private const float ATK_INTERVAL1 = 0.2f;
    /// <summary>攻撃かたまりのインターバル</summary>
    private const float ATK_INTERVAL2 = 10f;

    /// <summary>死ぬときに消えるボスモデル</summary>
    public GameObject handModel;

    public MoonObject moon1;
    public MoonObject moon2;
    public MoonObject moonBlack;

    /// <summary>攻撃オブジェクト</summary>
    public BossMoonAttack ballAttack;

    /// <summary>予告テンプレート</summary>
    public BossMoonAttackYokoku yokoku;
    /// <summary>予告表示</summary>
    private BossMoonAttackYokoku yoyakuDisp = null;

    /// <summary>時間</summary>
    private float actionTimer = 0f;
    /// <summary>攻撃用時間</summary>
    private float atkTimer = 0f;

    /// <summary>攻撃位置ランダム</summary>
    private int atkOffset = 0;
    /// <summary>攻撃回数</summary>
    private int atkCount = 0;
    /// <summary>攻撃フェーズ</summary>
    private int atkPhase = 0;
    /// <summary>攻撃する位置</summary>
    private Vector3 targetPos;
    /// <summary>Y座標</summary>
    private DeltaFloat atkY;

    #region 初期化・死亡演出

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();
        atkY = new DeltaFloat();
        EnableCollider(false);

        var game = GameMainSystem.Instance;
        // 一旦隠れる
        var p = game.GetPlayerCenter();
        p.y = -10f;
        transform.position = p;

        // 念の為表示
        moon1.Fade(true);
        moon2.Fade(true);

        // 見えてる方の目を動かす
        var move1 = false;
        while (true)
        {
            if (moon1.IsInView())
            {
                move1 = true;
                break;
            }
            else if (moon2.IsInView())
            {
                move1 = false;
                break;
            }
        }
        if (move1)
        {
            moon1.MoveToEye();
            yield return new WaitWhile(() => moon1.IsMoving());
        }
        else
        {
            moon2.MoveToEye();
            yield return new WaitWhile(() => moon2.IsMoving());
        }
        // 黒表示
        moonBlack.SetEyeBack(move1);
        moonBlack.Fade(true);
        yield return new WaitWhile(() => moonBlack.IsMoving());

        // 自分(手)の回転は黒の逆向き
        var ownDir = -moonBlack.transform.localPosition;
        ownDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(ownDir, new Vector3(0, 1, 0));

        // 目が下に向く
        yield return new WaitForSeconds(1f);
        moon1.DownEye();
        moon2.DownEye();
        yield return new WaitWhile(() => moon1.IsMoving());
    }

    /// <summary>
    /// 死亡演出
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator DeathAnim()
    {
        yield return base.DeathAnim();
        GameMainSystem.Instance.prm_Game.Defeated_Boss4 = true;

        // 予約があったら消す
        if (yoyakuDisp != null) Destroy(yoyakuDisp.gameObject);
        // 手を消す
        handModel.gameObject.SetActive(false);
        var p = transform.position;
        p.y = -25f;
        transform.position = p;

        // 目をフェードアウト
        moon1.Fade(false);
        moon2.Fade(false);
        yield return new WaitWhile(() => moon2.IsMoving());
        // 本体フェードアウト
        moonBlack.Fade(false);
        yield return new WaitWhile(() => moonBlack.IsMoving());
    }

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
        // 死んでたらもう動作しない
        if (hp <= 0) return;

        var dt = OriginManager.Instance.inGameDeltaTime;

        UpdateAttack(dt);

        actionTimer -= dt;
        if (actionTimer > 0) return;

        var posIdx = (atkOffset + atkCount) % 4;
        if (atkCount == 7)
        {
            // 最後の攻撃をする
            atkPhase = 1;
            atkTimer = 0f;
            targetPos = CreateRandomPos(posIdx);

            atkOffset = Util.RandomInt(0, 3);
            atkCount = 0;
            actionTimer = ATK_INTERVAL2;
        }
        else
        {
            // 球を落とす
            ballAttack.CreateNew(CreateRandomPos(posIdx), strength_rate);
            atkCount++;
            actionTimer = ATK_INTERVAL1;
        }

    }

    /// <summary>
    /// ランダム位置を作る 0～4で傾向
    /// </summary>
    /// <returns></returns>
    private Vector3 CreateRandomPos(int cnt)
    {
        const float RANDOM_RANGE = 30f;

        var basePos = cnt switch
        {
            0 => new Vector3(-RANDOM_RANGE, 0f, -RANDOM_RANGE),
            1 => new Vector3(0, 0, -RANDOM_RANGE),
            2 => new Vector3(-RANDOM_RANGE, 0, 0),
            _ => new Vector3(0, 0, 0),
        };

        basePos.x += Util.RandomFloat(0, RANDOM_RANGE);
        basePos.z += Util.RandomFloat(0, RANDOM_RANGE);

        return GameMainSystem.Instance.GetPlayerCenter() + basePos;
    }

    /// <summary>
    /// 攻撃の動き
    /// </summary>
    private void UpdateAttack(float delta)
    {
        if (atkPhase == 0) return;

        atkTimer -= delta;
        if (atkTimer > 0f) return;

        if (atkPhase == 1)
        {
            // 予告を出す
            yoyakuDisp = yokoku.CreateNew(targetPos);
            atkPhase++;
            atkTimer = 0.5f;
        }
        else if (atkPhase == 2)
        {
            // 落ち始め設定
            atkY.Set(targetPos.y + 50f);
            atkY.MoveTo(0.6f, 0.5f, DeltaFloat.MoveType.LINE);
            atkPhase++;
            EnableCollider(true);
        }
        else if (atkPhase == 3)
        {
            // 落ちる
            atkY.Update(delta);
            transform.position = new Vector3(targetPos.x, atkY.Get(), targetPos.z);
            if (!atkY.IsActive())
            {
                atkTimer = 3f;
                atkPhase++;

                // 落ち終わり
                Destroy(yoyakuDisp.gameObject);
                ManagerSceneScript.GetInstance().GetCamera3D().PlayShakeOne(Shaker.ShakeSize.Middle);
            }
        }
        else if (atkPhase == 4)
        {
            // 消える
            atkY.MoveTo(-25f, 0.3f, DeltaFloat.MoveType.ACCEL);
            atkPhase++;
        }
        else if (atkPhase == 5)
        {
            atkY.Update(delta);
            transform.position = new Vector3(targetPos.x, atkY.Get(), targetPos.z);
            if (!atkY.IsActive())
            {
                // 消え終わり
                atkPhase = 0;
                EnableCollider(false);
            }
        }
    }

    /// <summary>
    /// 当たり判定有効化
    /// </summary>
    /// <param name="e"></param>
    private void EnableCollider(bool e)
    {
        GetComponent<Collider>().enabled = e;
    }
}
