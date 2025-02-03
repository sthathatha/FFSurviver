using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 時間ごとに発生する攻撃
/// </summary>
public class BaseTimerWeapon : GameWeaponSystemBase
{
    #region パラメータ

    /// <summary>インターバル秒</summary>
    public float Prm_interval = 4f;

    /// <summary>制御時間</summary>
    private float time = 0f;

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        ExecUpdate();
        var origin = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();

        time -= origin.inGameDeltaTime;
        if (time <= 0f)
        {
            ExecAttack();
            time = Prm_interval;
        }
    }

    /// <summary>
    /// 攻撃呼び出し
    /// </summary>
    /// <param name="targets"></param>
    protected virtual void ExecAttack()
    {
    }
}
