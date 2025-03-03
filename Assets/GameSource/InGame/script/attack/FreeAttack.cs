using UnityEngine;

/// <summary>
/// 外から自由に操作する（自分で動かない）攻撃
/// </summary>
public class FreeAttack : AttackParameter
{
    /// <summary>エフェクト管理</summary>
    public GameObject effectObject;

    /// <summary>エフェクト表示中か</summary>
    public bool IsShowEffect { get; private set; } = true;

    #region 操作

    /// <summary>
    /// エフェクト表示オンオフ
    /// </summary>
    /// <param name="_show"></param>
    public void ShowEffect(bool _show)
    {
        effectObject.SetActive(_show);
        IsShowEffect = _show;
    }

    #endregion
}
