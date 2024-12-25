using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターベース
/// </summary>
public class CharacterScript : MonoBehaviour
{
    #region メンバー



    #endregion

    #region 変数

    /// <summary>物理</summary>
    protected Rigidbody rigid;
    /// <summary>モデルアニメーション</summary>
    protected Animator anim;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    private IEnumerator Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        yield return new WaitWhile(() => ManagerSceneScript.GetInstance() == null);

        yield return InitCharacter();
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        UpdateCharacter();
    }

    /// <summary>
    /// Updateより後にフレームごと1回
    /// </summary>
    private void LateUpdate()
    {
        UpdateCharacter2();
    }

    /// <summary>派生初期化処理</summary>
    protected virtual IEnumerator InitCharacter() { yield break; }
    /// <summary>派生更新処理</summary>
    protected virtual void UpdateCharacter() { }
    /// <summary>派生更新後処理</summary>
    protected virtual void UpdateCharacter2() { }

    #endregion

    /// <summary>
    /// 足音？
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
