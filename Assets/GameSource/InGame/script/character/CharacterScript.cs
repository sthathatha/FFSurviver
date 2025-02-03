using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;

/// <summary>
/// キャラクターベース
/// </summary>
public class CharacterScript : MonoBehaviour
{
    #region 定数

    /// <summary>ジャンプ初速</summary>
    protected const float JUMP_V0 = 15f;
    /// <summary>落下加速度</summary>
    protected const float FALL_G = 25.6f;
    /// <summary>落下速度最大</summary>
    protected const float FALL_MAX = -50f;
    /// <summary>接地判定距離</summary>
    protected const float STAND_DISTANCE = 0.05f;

    /// <summary>同じ攻撃から</summary>
    protected const float DAMAGE_INTERVAL = 0.25f;

    #endregion

    #region メンバー



    #endregion

    #region 変数

    /// <summary>物理</summary>
    protected Rigidbody rigid;
    /// <summary>モデルアニメーション</summary>
    protected Animator anim;

    /// <summary>Start処理が終わるまでUpdateは呼ばない</summary>
    private bool started = false;

    /// <summary>攻撃受けたヒストリー</summary>
    protected class AttackHistory
    {
        public AttackParameter atk;
        public float intTime;

        public AttackHistory(AttackParameter _atk, float _intTime) { atk = _atk; intTime = _intTime; }
    }
    /// <summary>ダメージ管理</summary>
    protected List<AttackHistory> atkHistories;

    /// <summary>HP</summary>
    protected int hp;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    private IEnumerator Start()
    {
        // 初期化変数など
        atkHistories = new List<AttackHistory>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        yield return new WaitWhile(() => ManagerSceneScript.GetInstance() == null);

        yield return InitCharacter();
        started = true;
    }

    /// <summary>
    /// 更新
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
    /// Updateより後にフレームごと1回
    /// </summary>
    private void LateUpdate()
    {
        if (started)
        {
            UpdateCharacter2();
        }
    }

    /// <summary>派生初期化処理</summary>
    protected virtual IEnumerator InitCharacter() { yield break; }
    /// <summary>派生更新処理</summary>
    protected virtual void UpdateCharacter() { }
    /// <summary>派生更新後処理</summary>
    protected virtual void UpdateCharacter2() { }

    #endregion

    #region ダメージ管理

    /// <summary>
    /// ダメージヒット
    /// </summary>
    /// <param name="param"></param>
    public void AttackTrigger(AttackParameter param)
    {
        if (!atkHistories.Any(h => h.atk == param))
        {
            // 履歴になければダメージ受ける
            atkHistories.Add(new AttackHistory(param, DAMAGE_INTERVAL * param.intervalRate));

            hp -= param.GetDamage();
            DamageHit();
            if (hp <= 0)
            {
                DamageDeath();
            }
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    private void DamageControl()
    {
        var manager = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();

        foreach (var h in atkHistories)
        {
            h.intTime -= manager.inGameDeltaTime;
        }

        // 居なくなっていたら削除
        atkHistories.RemoveAll(h => h.intTime <= 0f);
    }

    /// <summary>
    /// ダメージうけたイベント
    /// </summary>
    protected virtual void DamageHit() { }

    /// <summary>
    /// ダメージのため死亡
    /// </summary>
    protected virtual void DamageDeath()
    {
        atkHistories.Clear();
    }

    #endregion

    #region モーションからのイベント

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

    /// <summary>
    /// 着地？
    /// </summary>
    /// <param name="animationEvent"></param>
    virtual protected void OnLand(AnimationEvent animationEvent)
    {
    }

    #endregion
}
