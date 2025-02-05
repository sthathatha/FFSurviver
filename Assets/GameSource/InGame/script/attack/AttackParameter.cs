using UnityEngine;

/// <summary>
/// 攻撃パラメータ
/// </summary>
public class AttackParameter : MonoBehaviour
{
    #region パラメータ

    public bool atkToEnemy = false;
    public bool atkToPlayer = false;

    public bool isMagic = false;

    public int attackPower = 1;
    protected float attackRate = 1f;

    /// <summary>連続ヒット時間の長さに倍率</summary>
    public float intervalRate = 1f;

    /// <summary>サイズ倍率</summary>
    public float scaleRate { get; set; } = 1f;

    /// <summary>ヒット済み</summary>
    protected bool isHitted = false;

    #endregion

    /// <summary>
    /// 攻撃力
    /// </summary>
    /// <param name="rate"></param>
    public void SetAttackRate(float rate)
    {
        attackRate = rate;
    }

    /// <summary>
    /// ダメージ取得
    /// </summary>
    /// <returns></returns>
    public int GetDamage()
    {
        return Mathf.RoundToInt(attackPower * attackRate);
    }

    private void OnTriggerStay(Collider other)
    {
        if (atkToEnemy)
        {
            var enm = other.gameObject.GetComponent<EnemyScriptBase>();
            if (enm != null)
            {
                isHitted = true;
                enm.AttackTrigger(this);
            }
        }

        if (atkToPlayer)
        {
            var plr = other.gameObject.GetComponent<PlayerScript>();
            if (plr != null)
            {
                isHitted = true;
                plr.AttackTrigger(this);
            }
        }
    }
}
