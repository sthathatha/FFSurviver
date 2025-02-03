using System.Collections;
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

        initCount = 3;

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
            SetAttackParam(atk, pprm.stat_magic.value);
            atk.gameObject.SetActive(true);

            attacks.Add(atk);
        }

        UpdateAttackPos();
    }

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

        // 個数によって差分決定
        var theta = Mathf.PI * 2f / attacks.Count;

        var basePos = new Vector3(0, 0, distance);
        var r = baseRot;
        // １個ずつ場所決定
        foreach (var atk in attacks)
        {
            var p = Quaternion.Euler(0, -r * Mathf.Rad2Deg, 0) * basePos;
            atk.transform.localPosition = p;

            r += theta;
        }
    }
}
