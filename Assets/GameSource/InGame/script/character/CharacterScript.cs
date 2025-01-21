using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�x�[�X
/// </summary>
public class CharacterScript : MonoBehaviour
{
    #region �萔

    /// <summary>�W�����v����</summary>
    protected const float JUMP_V0 = 15f;
    /// <summary>���������x</summary>
    protected const float FALL_G = 25.6f;
    /// <summary>�������x�ő�</summary>
    protected const float FALL_MAX = -50f;
    /// <summary>�ڒn���苗��</summary>
    protected const float STAND_DISTANCE = 0.05f;

    /// <summary>�����U������</summary>
    protected const float DAMAGE_INTERVAL = 0.25f;

    #endregion

    #region �����o�[



    #endregion

    #region �ϐ�

    /// <summary>����</summary>
    protected Rigidbody rigid;
    /// <summary>���f���A�j���[�V����</summary>
    protected Animator anim;

    /// <summary>Start�������I���܂�Update�͌Ă΂Ȃ�</summary>
    private bool started = false;

    /// <summary>�U���󂯂��q�X�g���[</summary>
    protected class AttackHistory
    {
        public AttackParameter atk;
        public float intTime;

        public AttackHistory(AttackParameter _atk, float _intTime) { atk = _atk; intTime = _intTime; }
    }
    /// <summary>�_���[�W�Ǘ�</summary>
    protected List<AttackHistory> atkHistories;

    /// <summary>�ő�HP</summary>
    protected int hp_max;
    /// <summary>HP</summary>
    protected int hp;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    private IEnumerator Start()
    {
        // �������ϐ��Ȃ�
        atkHistories = new List<AttackHistory>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        yield return new WaitWhile(() => ManagerSceneScript.GetInstance() == null);

        yield return InitCharacter();
        started = true;
    }

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        if (started)
        {
            DamageControl();
            UpdateCharacter();
        }
    }

    /// <summary>
    /// Update����Ƀt���[������1��
    /// </summary>
    private void LateUpdate()
    {
        if (started)
        {
            UpdateCharacter2();
        }
    }

    /// <summary>�h������������</summary>
    protected virtual IEnumerator InitCharacter() { yield break; }
    /// <summary>�h���X�V����</summary>
    protected virtual void UpdateCharacter() { }
    /// <summary>�h���X�V�㏈��</summary>
    protected virtual void UpdateCharacter2() { }

    #endregion

    #region �_���[�W�Ǘ�

    /// <summary>
    /// �_���[�W�q�b�g
    /// </summary>
    /// <param name="param"></param>
    public void AttackTrigger(AttackParameter param)
    {
        if (!atkHistories.Any(h => h.atk == param))
        {
            // �����ɂȂ���΃_���[�W�󂯂�
            atkHistories.Add(new AttackHistory(param, DAMAGE_INTERVAL));

            hp -= param.GetDamage();
            DamageHit();
            if (hp <= 0)
            {
                DamageDeath();
            }
        }
    }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    private void DamageControl()
    {
        var manager = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();

        foreach (var h in atkHistories)
        {
            h.intTime -= manager.inGameDeltaTime;
        }

        // ���Ȃ��Ȃ��Ă�����폜
        atkHistories.RemoveAll(h => h.intTime <= 0f);
    }

    /// <summary>
    /// �_���[�W�������C�x���g
    /// </summary>
    protected virtual void DamageHit() { }

    /// <summary>
    /// �_���[�W�̂��ߎ��S
    /// </summary>
    protected virtual void DamageDeath()
    {
        atkHistories.Clear();
    }

    #endregion

    #region ���[�V��������̃C�x���g

    /// <summary>
    /// �����H
    /// </summary>
    /// <param name="animationEvent"></param>
    virtual protected void OnFootstep(AnimationEvent animationEvent)
    {
        //if (animationEvent.animatorClipInfo.weight > 0.5f)
        //{
        //    if (FootstepAudioClips.Length > 0)
        //    {
        //        var index = Random.Range(0, FootstepAudioClips.Length);
        //        AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
        //    }
        //}
    }

    /// <summary>
    /// ���n�H
    /// </summary>
    /// <param name="animationEvent"></param>
    virtual protected void OnLand(AnimationEvent animationEvent)
    {
    }

    #endregion
}
