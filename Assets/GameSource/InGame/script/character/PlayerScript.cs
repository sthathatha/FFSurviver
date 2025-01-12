using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー
/// </summary>
public class PlayerScript : CharacterScript
{
    #region メンバー

    #region パラメータ

    /// <summary>HP</summary>
    private UIHpGauge ui_hp;

    #endregion

    /// <summary>通常攻撃</summary>
    public SimpleAttack normal_attack;

    /// <summary>移動最高速</summary>
    public float run_speed = 10f;

    /// <summary>接地判定用Ray</summary>
    private Ray ground_ray;

    /// <summary>プレイヤー</summary>
    public enum PlayerState
    {
        /// <summary>立ち</summary>
        Stand = 0,
        /// <summary>ジャンプ</summary>
        Jump,
        /// <summary>ダメージ</summary>
        Damage,
    }
    public PlayerState state { get; private set; }

    /// <summary>空中上下速度</summary>
    private float y_speed = 0f;

    /// <summary>ジャンプ可能回数</summary>
    private int jump_count_max = 1;

    /// <summary>ジャンプ回数</summary>
    private int jump_count = 0;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();

        // 最初の取得など
        ui_hp = GameMainSystem.Instance.ui_hp;
        normal_attack.gameObject.SetActive(false);

        var cam = ManagerSceneScript.GetInstance().GetCamera3D();
        cam.SetCameraDist(10f);
        CameraControl(true);

        ground_ray = new Ray();
        ground_ray.direction = new Vector3(0, -1f, 0);
        state = PlayerState.Stand;

        // ゲームメインに自分を渡す
        GameMainSystem.Instance.playerScript = this;

        //todo:パラメータ初期化
        jump_count_max = 1;
        hp_max = 100;
        hp = hp_max;
        ui_hp.SetHP(hp, hp_max);
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        AttackControl();
        GroundControl();
        MoveControl();
        UpdateAnim();
    }

    /// <summary>
    /// 後で走る更新
    /// </summary>
    protected override void UpdateCharacter2()
    {
        base.UpdateCharacter2();
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;
        CameraControl();
    }

    #endregion

    #region 攻撃処理

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void AttackControl()
    {
        var main = GameMainSystem.Instance;

        // キャラの向き
        var direction = transform.rotation * new Vector3(0, 0, 1);

        // 通常攻撃
        if (GameInput.IsPress(GameInput.Buttons.NormalAttack)
            && (state != PlayerState.Damage))
        {
            var center = GetComponent<Collider>().bounds.center;
            var na = Instantiate(normal_attack, main.attackParent);
            na.gameObject.SetActive(false);
            na.Shoot(center, direction);
        }
    }

    #endregion

    #region 接地判定処理

    /// <summary>
    /// 地面検索
    /// </summary>
    /// <param name="hitGround"></param>
    /// <param name="hitInfo"></param>
    /// <returns></returns>
    private bool GroundSearch(out GameGround hitGround, out RaycastHit hitInfo)
    {
        hitGround = null;

        // Ray更新
        // 判定猶予
        var threathold = -y_speed * ManagerSceneScript.GetInstance().validDeltaTime;
        if (threathold < 0.2f) threathold = 0.2f;
        ground_ray.origin = transform.position + new Vector3(0, threathold, 0);

        // 検索
        var layer = LayerMask.GetMask(new string[] { "Ground" });
        var groundHit = Physics.Raycast(ground_ray,
            out hitInfo,
            STAND_DISTANCE + threathold,
            layer
            );
        if (!groundHit) return false;

        var ground = hitInfo.collider.GetComponent<GameGround>();
        if (ground == null) return false;

        hitGround = ground;
        return true;
    }

    /// <summary>
    /// 接地判定処理
    /// </summary>
    private void GroundControl()
    {
        var manager = ManagerSceneScript.GetInstance();
        var dt = manager.validDeltaTime;

        // Ray判定
        var groundHit = GroundSearch(out GameGround ground, out RaycastHit hitInfo);

        if (state == PlayerState.Stand)
        {
            // ジャンプ
            if (GameInput.IsPress(GameInput.Buttons.Jump))
            {
                Jump();
            }
            else if (!groundHit)
            {
                // 地面が無くなったら落下
                y_speed = 0f;
                state = PlayerState.Jump;
                anim?.SetBool("Jump", false);
                anim?.SetBool("FreeFall", true);
                anim?.SetBool("Grounded", false);
            }
        }
        else if (state == PlayerState.Jump)
        {
            // 上昇中は着地しない
            if (groundHit)
            {
                if (y_speed > 0f) groundHit = false;
            }

            // 物を踏んだら着地
            if (groundHit)
            {
                jump_count = 0;
                y_speed = 0f;
                transform.position = hitInfo.point;
                state = PlayerState.Stand;
                anim?.SetBool("Jump", false);
                anim?.SetBool("FreeFall", false);
                anim?.SetBool("Grounded", true);
            }
            else if (GameInput.IsPress(GameInput.Buttons.Jump))
            {
                // スペースキーで二段ジャンプ
                Jump();
            }
            else
            {
                // 下に加速
                y_speed -= FALL_G * dt;
                if (y_speed < FALL_MAX) y_speed = FALL_MAX;
                var newPos = transform.position + new Vector3(0, y_speed * dt, 0);
                // 落ち過ぎたら上に出る
                if (newPos.y < -20f) newPos.y = 20f;

                transform.position = newPos;
            }
        }
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        var manager = ManagerSceneScript.GetInstance();
        if (jump_count < jump_count_max)
        {
            var dt = manager.validDeltaTime;
            jump_count++;
            y_speed = JUMP_V0;
            state = PlayerState.Jump;

            transform.position += new Vector3(0, y_speed * dt, 0);

            // ジャンプアニメーション
            anim?.SetBool("Jump", true);
            anim?.SetBool("Grounded", false);
            anim?.SetBool("FreeFall", false);
        }
    }

    #endregion

    #region 移動操作

    /// <summary>
    /// 移動操作
    /// </summary>
    private void MoveControl()
    {
        // Y速度は保持
        var vy = rigid.linearVelocity.y;

        // 移動ベース
        var stick = GameInput.GetLeftStick();

        if (stick.magnitude < 0.1f)
        {
            rigid.linearVelocity = new Vector3(0, vy, 0);
            return;
        }

        // カメラ回転のy成分
        var cam = ManagerSceneScript.GetInstance().GetCamera3D();
        var camRot = cam.RotateLR * Mathf.Rad2Deg;

        // スティック値をカメラ回転と合成
        var move = new Vector3(stick.x, 0, stick.y);
        move = Quaternion.Euler(0, camRot, 0) * move * run_speed;
        rigid.linearVelocity = new Vector3(move.x, vy, move.z);

        // キャラの向き
        var chrRot = Quaternion.LookRotation(move, new Vector3(0, 1f, 0));
        transform.rotation = chrRot;
    }

    #endregion

    #region ダメージ処理

    /// <summary>
    /// 回復
    /// </summary>
    /// <param name="heal"></param>
    private void Heal(int heal)
    {
        hp += heal;
        if (hp > hp_max) hp = hp_max;

        ui_hp.SetHP(hp, hp_max);
    }

    /// <summary>
    /// 敵判定の接触中
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        var atk = other.gameObject.GetComponent<AttackParameter>();
        if (atk == null) return;
        if (!atk.atkToPlayer) return;

        AttackTrigger(atk);
    }

    /// <summary>
    /// ダメージヒット
    /// </summary>
    protected override void DamageHit()
    {
        base.DamageHit();
        ui_hp.SetHP(hp, hp_max);
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    protected override void DamageDeath()
    {
        base.DamageDeath();

        //todo:ゲームオーバー処理
    }

    #endregion

    #region カメラ操作

    /// <summary>
    /// カメラ操作
    /// </summary>
    /// <param name="noInput">入力不可</param>
    private void CameraControl(bool noInput = false)
    {
        var stick = noInput ? Vector2.zero : GameInput.GetRightStick();

        var cam = ManagerSceneScript.GetInstance().GetCamera3D();

        cam.SetTargetPos(transform.position + new Vector3(0, 0.6f, 0));
        if (Mathf.Abs(stick.x) > 0.1f)
            cam.SetRotateLR(stick.x);
        if (Mathf.Abs(stick.y) > 0.1f)
            cam.SetRotateUD(stick.y);

    }

    #endregion

    #region モデルアニメーション制御

    /// <summary>
    /// モデルアニメーション制御
    /// </summary>
    private void UpdateAnim()
    {
        if (state == PlayerState.Stand)
        {
            var v = rigid.linearVelocity;
            v.y = 0f;
            var spd = v.magnitude / 4f;
            anim?.SetFloat("Speed", spd);
            anim?.SetFloat("MotionSpeed", spd);
        }
    }

    #endregion
}
