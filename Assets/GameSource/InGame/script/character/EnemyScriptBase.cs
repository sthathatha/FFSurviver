using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �G�X�N���v�g����
/// </summary>
public class EnemyScriptBase : CharacterScript
{
    #region �����o�[

    /// <summary>��{�̗�</summary>
    public int hp_base = 1;

    /// <summary>�ő�HP</summary>
    protected int hp_max;

    /// <summary>�o���l��{</summary>
    public int exp_base = 1;

    /// <summary>�������[�g</summary>
    protected float strength_rate = 1f;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();

        // �Q�[���V�X�e������X�e�[�^�X�{�����擾
        strength_rate = IsBoss() ? GameMainSystem.Instance.GetBossRate() : GameMainSystem.Instance.GetEnemyRate();
        hp_max = Mathf.RoundToInt(hp_base * strength_rate);
        hp = hp_max;

        var atk = GetComponent<AttackParameter>();
        atk.SetAttackRate(strength_rate);
    }

    /// <summary>
    /// �X�V
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        // ���ꂷ������
        var pCenter = GameMainSystem.Instance.GetPlayerCenter();
        var distance = pCenter - transform.position;
        if (distance.sqrMagnitude > 10000f) TooFarPlayer(pCenter);
    }

    /// <summary>
    /// �v���C���[���痣�ꂷ������
    /// </summary>
    /// <param name="playerCenter"></param>
    protected virtual void TooFarPlayer(Vector3 playerCenter)
    {
    }

    /// <summary>
    /// ���S������
    /// </summary>
    protected override void DamageDeath()
    {
        base.DamageDeath();

        // �o���l���擾
        //todo:�o���l�v�Z
        var exp = Mathf.CeilToInt(Mathf.Pow(strength_rate, 0.8f) * exp_base);
        GameMainSystem.Instance.AddExp(exp);
    }

    #endregion

    #region ��������p

    /// <summary>
    /// �{�X
    /// </summary>
    /// <returns></returns>
    virtual protected bool IsBoss() { return false; }

    #endregion
}
