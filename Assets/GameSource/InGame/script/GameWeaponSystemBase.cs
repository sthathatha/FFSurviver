using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 武器システム
/// </summary>
public class GameWeaponSystemBase : SubScriptBase
{
    #region 共通パラメータ

    /// <summary>攻撃力アップ</summary>
    public float Prm_attackRate { get; set; } = 1f;

    /// <summary>攻撃サイズ</summary>
    public float Prm_attackSize { get; set; } = 1f;

    /// <summary>攻撃の基準になる位置</summary>
    protected List<Vector3> playerPos;

    #endregion

    #region Unity基底

    /// <summary>
    /// 開始
    /// </summary>
    protected override IEnumerator InitStart()
    {
        yield return base.InitStart();

        // 先頭はプレイヤー自身
        playerPos = new List<Vector3>
        {
            Vector3.zero
        };
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateSub()
    {
        base.UpdateSub();

        var main = GameMainSystem.Instance;
        // 距離が離れるとなにかの処理がかかりそうなので常について動く
        var plr = main.GetPlayerCenter();
        transform.position = plr;

        // 位置更新
        playerPos[0] = plr;
        // オプション処理
        if (main.weaponManager.HaveWeapon(WeaponManager.ID.ChildOption))
        {
            var opt = main.weaponManager.GetWeaponSlot(WeaponManager.ID.ChildOption).AsChildren();
            while (playerPos.Count < opt.GetOptionCount() + 1)
                playerPos.Add(Vector3.zero);
            if (playerPos.Count > opt.GetOptionCount() + 1)
                playerPos = playerPos.Take(opt.GetOptionCount() + 1).ToList();

            for (var i = 0; i < opt.GetOptionCount(); ++i)
            {
                var idx = i + 1;
                playerPos[idx] = opt.GetOption(i).GetCenter();
            }
        }
        else
        {
            playerPos = playerPos.Take(1).ToList();
        }

        ExecUpdate();
    }

    #endregion

    #region 派生機能

    /// <summary>
    /// システムに登録
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCoroutine(List<int> paramList)
    {
        yield return base.InitCoroutine(paramList);

        GameMainSystem.Instance.weaponManager.AddWeapon((WeaponManager.ID)paramList[0], this);
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    protected virtual void ExecUpdate()
    {
    }

    /// <summary>
    /// 攻撃パラメータ設定
    /// </summary>
    /// <param name="prm">攻撃クラス</param>
    /// <param name="power"></param>
    protected void SetAttackParam(AttackParameter prm, int power)
    {
        prm.SetAttackRate(power * Prm_attackRate);
        prm.scaleRate = Prm_attackSize;
        if (prm is not ExpandAttack)
        {
            prm.transform.localScale = new Vector3(Prm_attackSize, Prm_attackSize, Prm_attackSize);
        }
    }

    #endregion
}
