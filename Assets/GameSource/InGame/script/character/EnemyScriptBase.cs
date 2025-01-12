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
        var rate = IsBoss() ? GameMainSystem.Instance.GetBossRate() : GameMainSystem.Instance.GetEnemyRate();
        hp_max = Mathf.RoundToInt(hp_base * rate);
        hp = hp_max;

        var atk = GetComponent<AttackParameter>();
        atk.SetAttackRate(rate);
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

    #endregion

    #region ��������p

    /// <summary>
    /// �{�X
    /// </summary>
    /// <returns></returns>
    virtual protected bool IsBoss() { return false; }

    #endregion
}
