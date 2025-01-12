using UnityEngine;

/// <summary>
/// 攻撃パラメータ
/// </summary>
public class AttackParameter : MonoBehaviour
{
    #region パラメータ

    public bool atkToEnemy = false;
    public bool atkToPlayer = false;

    public int attackPower = 1;
    private float attackRate = 1f;

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
                enm.AttackTrigger(this);
            }
        }

        if (atkToPlayer)
        {
            var plr = other.gameObject.GetComponent<PlayerScript>();
            if (plr != null)
            {

            }
        }
    }
}
