using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�x�[�X
/// </summary>
public class CharacterScript : MonoBehaviour
{
    #region �����o�[



    #endregion

    #region �ϐ�

    /// <summary>����</summary>
    protected Rigidbody rigid;
    /// <summary>���f���A�j���[�V����</summary>
    protected Animator anim;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    private IEnumerator Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        yield return new WaitWhile(() => ManagerSceneScript.GetInstance() == null);

        yield return InitCharacter();
    }

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        UpdateCharacter();
    }

    /// <summary>
    /// Update����Ƀt���[������1��
    /// </summary>
    private void LateUpdate()
    {
        UpdateCharacter2();
    }

    /// <summary>�h������������</summary>
    protected virtual IEnumerator InitCharacter() { yield break; }
    /// <summary>�h���X�V����</summary>
    protected virtual void UpdateCharacter() { }
    /// <summary>�h���X�V�㏈��</summary>
    protected virtual void UpdateCharacter2() { }

    #endregion

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
}
