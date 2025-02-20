using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 鏡の怪物の攻撃
/// </summary>
public class BossMirrorAttack : SubScriptBase
{
    /// <summary>ボスオブジェクト用</summary>
    public Transform tmpParent;

    /// <summary>爆弾</summary>
    public EffectOneShotAttack template;

    /// <summary>赤レーザー</summary>
    public GeneralLaser laserRed;
    /// <summary>白レーザー</summary>
    public GeneralLaser laserWhite;

    /// <summary>削除待ち</summary>
    private bool destroyWait = false;

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    /// <param name="rate">攻撃力倍率</param>
    public void Shoot(Vector3 self, Vector3 target, float rate)
    {
        StartCoroutine(AttackCoroutine(self, target, rate));
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    /// <param name="rate"></param>
    /// <returns></returns>
    private IEnumerator AttackCoroutine(Vector3 self, Vector3 target, float rate)
    {
        var origin = OriginManager.Instance;
        var game = GameMainSystem.Instance;

        // 赤レーザー表示
        var red = Instantiate(laserRed, tmpParent);
        red.SetLine(self, target);
        red.gameObject.SetActive(true);

        var time = 1f;
        while (time > 0f && !destroyWait)
        {
            time -= origin.inGameDeltaTime;
            yield return null;
        }
        Destroy(red.gameObject);
        if (destroyWait) yield break;

        // 白レーザー表示
        var whiteRay = new Ray(self, target - self);
        var rayRange = (target - self).magnitude;
        var pMask = LayerMask.GetMask(new string[] { "Player" });

        var white = Instantiate(laserWhite, tmpParent);
        white.SetLine(self, target);
        white.gameObject.SetActive(true);
        time = 0.5f;
        while (time > 0f && !destroyWait)
        {
            // 白表示中にプレイヤーに当たったら直撃
            if (Physics.Raycast(whiteRay, out RaycastHit rayHit, rayRange, pMask))
            {
                target = rayHit.point;
                break;
            }

            time -= origin.inGameDeltaTime;
            yield return null;
        }
        Destroy(white.gameObject);
        if (destroyWait) yield break;

        // 爆発作成
        var bomb = Instantiate(template, game.attackParent);
        bomb.SetAttackRate(rate);
        bomb.Shoot(target);
    }

    /// <summary>
    /// 消え終わったらDestroy
    /// </summary>
    public void WaitDestroy()
    {
        destroyWait = true;
        StartCoroutine(WaitDestroyCoroutine());
    }

    /// <summary>
    /// 消えるの待ち
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitDestroyCoroutine()
    {
        // Coroutineはフラグ立てたら2～3フレぐらいで充分
        yield return null;
        yield return null;
        yield return null;
        DeleteScene();
    }
}
