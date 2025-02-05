using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// オプション
/// </summary>
public class ChildOptionSystem : GameWeaponSystemBase
{
    /// <summary>コピー元</summary>
    public ChildOption template;

    /// <summary>子供</summary>
    protected List<ChildOption> children;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitStart()
    {
        yield return base.InitStart();
        children = new List<ChildOption>();

        AddChild();
    }

    /// <summary>
    /// 子供追加
    /// </summary>
    public void AddChild()
    {
        var child = Instantiate(template, GameMainSystem.Instance.attackParent);
        if (children.Count > 0)
        {
            child.InitOption(children.Last().transform);
        }
        else
        {
            child.InitOption(GameMainSystem.Instance.playerScript.transform);
        }

        children.Add(child);
    }

    /// <summary>
    /// オプション取得
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public ChildOption GetOption(int idx)
    {
        return children[idx];
    }

    /// <summary>
    /// 数取得
    /// </summary>
    /// <returns></returns>
    public int GetOptionCount() { return children.Count; }
}
