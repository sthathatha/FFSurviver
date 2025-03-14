﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回転システム
/// </summary>
public class RollOptionSystem : GameWeaponSystemBase
{
    /// <summary>回転速度</summary>
    public float rotSpeed = 0.25f;

    /// <summary>プレイヤーからの距離</summary>
    public float distance = 8f;

    /// <summary>最初の数</summary>
    public int initCount = 1;

    /// <summary>コピー元</summary>
    public FreeAttack template;

    /// <summary>攻撃</summary>
    private List<FreeAttack> attacks;

    /// <summary>回転方向</summary>
    private float baseRot = 0f;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitStart()
    {
        yield return base.InitStart();

        attacks = new List<FreeAttack>();
        // 作成
        Create(initCount);
    }

    /// <summary>
    /// 作成
    /// </summary>
    public void Create(int optionCount)
    {
        // もうできてたら無視
        if (attacks.Count >= optionCount) return;

        var plr = GameMainSystem.Instance.playerScript;
        var pprm = GameMainSystem.Instance.prm_Player;

        for (var i = attacks.Count; i < optionCount; i++)
        {
            // 作成
            var atk = Instantiate(template, plr.body_parent, false);
            SetAttackParam(atk);
            atk.gameObject.SetActive(true);

            attacks.Add(atk);
        }

        UpdateAttackPos();
    }

    /// <summary>
    /// オプション追加
    /// </summary>
    public void AddOption()
    {
        Create(attacks.Count + 1);
    }

    /// <summary>
    /// 現在の数
    /// </summary>
    /// <returns></returns>
    public int GetOptionCount() { return attacks.Count; }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void ExecUpdate()
    {
        base.ExecUpdate();

        var org = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();

        // 更新
        baseRot += rotSpeed * Mathf.PI * 2f * org.inGameDeltaTime;
        baseRot = Util.GetNormalRadian(baseRot);

        UpdateAttackPos();
    }

    /// <summary>
    /// 攻撃位置更新
    /// </summary>
    private void UpdateAttackPos()
    {
        if (attacks == null || attacks.Count == 0) return;

        // カメラ方向
        var camR = ManagerSceneScript.GetInstance().GetCamera3D().transform.rotation;

        // 個数によって差分決定
        var theta = Mathf.PI * 2f / attacks.Count;

        var basePos = new Vector3(0, 0, distance);
        var r = baseRot;
        // １個ずつ場所決定
        foreach (var atk in attacks)
        {
            var rq = Quaternion.Euler(0, -r * Mathf.Rad2Deg, 0);
            var p = rq * basePos;
            atk.transform.localPosition = p;

            r += theta;

            // 手前のやつ非表示
            var camRotSub = Quaternion.Angle(camR, rq);
            if (camRotSub > 100f)
            {
                if (atk.IsShowEffect) atk.ShowEffect(false);
            }
            else
            {
                if (!atk.IsShowEffect) atk.ShowEffect(true);
            }
        }
    }

    /// <summary>
    /// サイズ変更を反映
    /// </summary>
    public void UpdateSize()
    {
        foreach (var atk in attacks)
        {
            atk.transform.localScale = new Vector3(Prm_attackSize, Prm_attackSize, Prm_attackSize);
        }
    }

    /// <summary>
    /// 攻撃力を反映
    /// </summary>
    public void UpdateAttackParam()
    {
        var plr = GameMainSystem.Instance.playerScript;
        var pprm = GameMainSystem.Instance.prm_Player;

        foreach (var atk in attacks)
        {
            SetAttackParam(atk);
        }
    }
}
