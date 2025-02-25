using System.Collections;
using UnityEngine;

/// <summary>
/// 月の怪物（手が本体）
/// </summary>
public class BossMoonScript : BossScriptBase
{
    /// <summary>死ぬときに消えるボスモデル</summary>
    public GameObject handModel;

    public MoonObject moon1;
    public MoonObject moon2;
    public MoonObject moonBlack;

    #region 初期化・死亡演出

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();

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
        return base.DeathAnim();
    }

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();


    }
}
