using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.UI.Image;

/// <summary>
/// つくよみちゃん
/// </summary>
public class BossTukuyomiScript : BossScriptBase
{
    #region 定数

    /// <summary>
    /// アニメーション
    /// </summary>
    private enum AnimType
    {
        Stand = 0,
        Run,
        Jump,
        Fall,
        Grounded,
    }

    #endregion

    #region メンバー

    /// <summary>モデル</summary>
    public GameObject model;

    /// <summary>駒</summary>
    public SimpleAttack atkTemplate;

    #endregion

    #region 変数

    /// <summary>コルーチン制御</summary>
    private IEnumerator updateCor = null;

    #endregion

    #region 登場退場演出

    /// <summary>
    /// 登場時
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();
        var game = GameMainSystem.Instance;
        var origin = OriginManager.Instance;
        anim = model.GetComponent<Animator>();

        ShowModel(false);

        //todo:登場セリフ表示

        // 空中から降りてくる
        var py = new DeltaFloat();
        py.Set(50f);
        py.MoveTo(0f, 5f, DeltaFloat.MoveType.DECEL);
        // カメラ方向に少し前
        var p = game.GetPlayerCenter();
        var camDist = p - ManagerSceneScript.GetInstance().GetCamera3D().gameObject.transform.position;
        camDist.y = 0f;
        p += camDist.normalized * 20f;
        p.y = py.Get();
        transform.position = p;
        // 向き
        transform.rotation = Quaternion.LookRotation(-camDist, new Vector3(0, 1, 0));
        // アニメーション
        ShowModel(true);
        SetAnimation(AnimType.Fall);

        // 降りてくる
        while (py.IsActive())
        {
            yield return null;
            py.Update(origin.inGameDeltaTime);
            p.y = py.Get();
            transform.position = p;
        }
        // 降りてくる途中で死ぬかも
        if (hp <= 0) yield break;

        SetAnimation(AnimType.Grounded);

        // 動作作成
        updateCor = UpdateCoroutine();
        StartCoroutine(updateCor);
    }

    /// <summary>
    /// 死亡時
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator DeathAnim()
    {
        // 更新コルーチン削除
        if (updateCor != null)
        {
            StopCoroutine(updateCor);
            updateCor = null;
        }

        yield return base.DeathAnim();
        ShowModel(false);

        //todo:退場セリフ

    }

    #endregion

    #region メイン処理

    /// <summary>
    /// コルーチンで動く
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateCoroutine()
    {
        var origin = OriginManager.Instance;

        while (true)
        {
            yield return origin.WaitIngameTime(1f);

            // 適当な位置に移動する
            yield return RandomMoveCoroutine();
            yield return origin.WaitIngameTime(1f);

            var atkCheck = Util.RandomInt(0, 2);
            if (atkCheck == 0)
            {
                // 攻撃１
                yield return AttackCoroutine1();
            }
            else if (atkCheck == 1)
            {
                // 攻撃２
                yield return AttackCoroutine2();
            }
            else if (atkCheck == 2)
            {
                // 攻撃３
                yield return AttackCoroutine3();
            }
        }
    }

    #endregion

    #region 汎用コルーチン

    /// <summary>
    /// 適当な位置に移動する
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomMoveCoroutine()
    {
        var origin = OriginManager.Instance;
        var p = new DeltaVector3();
        p.Set(transform.position);
        var tgt = GameMainSystem.Instance.GetPlayerCenter();
        tgt += Quaternion.Euler(0, Util.RandomFloat(0, 359f), 0) * new Vector3(0, 0, Util.RandomFloat(10f, 20f));
        tgt.y = 0f;
        var dist = tgt - transform.position;
        var tim = dist.magnitude / 20f;
        p.MoveTo(tgt, tim, DeltaFloat.MoveType.LINE);

        // 向きとアニメーション
        transform.rotation = Quaternion.LookRotation(dist, new Vector3(0, 1, 0));
        SetAnimation(AnimType.Run);

        while (p.IsActive())
        {
            yield return null;
            p.Update(origin.inGameDeltaTime);
            transform.position = p.Get();
        }

        // プレイヤーの位置に向く
        dist = GameMainSystem.Instance.GetPlayerCenter() - transform.position;
        dist.y = 0f;
        transform.rotation = Quaternion.LookRotation(dist, new Vector3(0, 1, 0));
        SetAnimation(AnimType.Stand);
    }

    /// <summary>
    /// 回転させる弾幕
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCoroutine1()
    {
        var origin = OriginManager.Instance;
        var cnt = CalcAttackCount();

        for (var i = 0; i < 50; ++i)
        {
            yield return origin.WaitIngameTime(0.1f);
            CreateAttack(cnt, i * 4f);
        }
    }

    /// <summary>
    /// 市松模様
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCoroutine2()
    {
        var origin = OriginManager.Instance;
        var cnt = CalcAttackCount();

        for (var i = 0; i < 70; ++i)
        {
            yield return origin.WaitIngameTime(0.08f);
            if (i % 2 == 0)
                CreateAttack(cnt, i * 4f);
            else
                CreateAttack(cnt, i * -4f);
        }
    }

    /// <summary>
    /// 全方位波
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCoroutine3()
    {
        var origin = OriginManager.Instance;

        for (var i = 0; i < 4; ++i)
        {
            CreateAttack(18, 0f);
            yield return origin.WaitIngameTime(0.3f);
            CreateAttack(18, 10f);
            yield return origin.WaitIngameTime(1.5f);
        }
    }

    #endregion

    #region 汎用メソッド

    /// <summary>
    /// HPが減るほど攻撃方向が増える
    /// </summary>
    /// <returns></returns>
    private int CalcAttackCount()
    {
        var hpRate = (float)hp / hp_max;
        return hpRate switch
        {
            < 0.2f => 6,
            < 0.5f => 5,
            < 0.8f => 4,
            _ => 3,
        };
    }

    /// <summary>
    /// 表示制御
    /// </summary>
    /// <param name="_show"></param>
    private void ShowModel(bool _show)
    {
        var col = GetComponent<Collider>();
        // 表示
        model.SetActive(_show);
        col.enabled = _show;
    }

    /// <summary>
    /// アニメーション設定
    /// </summary>
    private void SetAnimation(AnimType _type)
    {
        if (_type == AnimType.Stand)
        {
            anim?.SetFloat("Speed", 0f);
            anim?.SetFloat("MotionSpeed", 0f);
        }
        else if (_type == AnimType.Run)
        {
            anim?.SetFloat("Speed", 5f);
            anim?.SetFloat("MotionSpeed", 5f);
        }
        else if (_type == AnimType.Jump)
        {
            anim?.SetBool("Grounded", false);
            anim?.SetBool("FreeFall", false);
            anim?.SetBool("Jump", true);
        }
        else if (_type == AnimType.Fall)
        {
            anim?.SetBool("Jump", false);
            anim?.SetBool("Grounded", false);
            anim?.SetBool("FreeFall", true);
        }
        else
        {
            anim?.SetBool("Jump", false);
            anim?.SetBool("FreeFall", false);
            anim?.SetBool("Grounded", true);
        }
    }

    /// <summary>
    /// 自分の中心
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCenterPos()
    {
        return transform.position + new Vector3(0, 0.55f, 0);
    }

    /// <summary>
    /// まわりに攻撃作成
    /// </summary>
    /// <param name="cnt">個数</param>
    /// <param name="startRot">基準角度 0度＝前方</param>
    /// <param name="heightRot">高さ方向角度</param>
    private void CreateAttack(int cnt, float startRot = 0f, float heightRot = 0f)
    {
        var game = GameMainSystem.Instance;
        var center = GetCenterPos();
        var dir = Quaternion.Euler(0, startRot, 0) *
            transform.rotation *
            Quaternion.Euler(heightRot, 0, 0) *
            new Vector3(0, 0, 1);

        var addQ = Quaternion.Euler(0, 360f / cnt, 0);
        for (var i = 0; i < cnt; i++)
        {
            var atk = Instantiate(atkTemplate, game.attackParent);
            atk.gameObject.SetActive(false);
            atk.SetAttackRate(strength_rate);
            atk.Shoot(center, dir);

            dir = addQ * dir;
        }
    }

    #endregion
}
