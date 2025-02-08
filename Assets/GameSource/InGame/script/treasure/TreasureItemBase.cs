using UnityEngine;

/// <summary>
/// アイテム機能
/// </summary>
public abstract class TreasureItemBase
{
    /// <summary>名前</summary>
    public string name = "";
    /// <summary>説明文</summary>
    public string description = "";
    /// <summary>レアリティ</summary>
    protected int rarelity = 100;

    /// <summary>出現するかどうか</summary>
    public abstract bool CanGet();
    /// <summary>レアリティ取得 固定ならRarelity設定すればよい</summary>
    public virtual int GetRarelity() { return rarelity; }
    /// <summary>取得時の結果反映</summary>
    public abstract void ExecGetItem();
}
