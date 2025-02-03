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

    /// <summary>HP</summary>
    private UIHpGauge ui_hp;

    /// <summary>通常攻撃</summary>
    public SimpleAttack normal_attack;

    /// <summary>自分について動く攻撃用</summary>
    public Transform body_parent;

    /// <summary>接地判定用Ray</summary>
    private Ray ground_ray;

    /// <summary>プレイヤー状態</summary>
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

        // パラメータ初期化
        var game = GameMainSystem.Instance.prm_Game;
        var pprm = GameMainSystem.Instance.prm_Player;

        hp = pprm.stat_maxHp.value;
        ui_hp.SetHP(hp, hp);
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        // メニュー処理中は下の処理をしない
        if (!MenuControl()) return;

        AttackControl();
        MoveControl();
        GroundControl();
        UpdateAnim();

        // 攻撃用のやつ
        body_parent.position = GetComponent<Collider>().bounds.center;
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

    #region メニュー操作

    /// <summary>
    /// メニュー操作
    /// </summary>
    /// <returns>false:以降の処理を無視</returns>
    private bool MenuControl()
    {
        if (GameInput.IsPress(GameInput.Buttons.Menu))
        {
            // 物理は止められないのでXZ速度止める
            var vy = rigid.linearVelocity.y;
            rigid.linearVelocity = new Vector3(0, vy, 0);
            GameMainSystem.Instance.OpenMenu();
            return false;
        }

        return true;
    }

    #endregion

    #region 攻撃処理

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void AttackControl()
    {
        var main = GameMainSystem.Instance;
        var tmpData = GlobalData.GetTemporaryData();
        var game = main.prm_Game;
        var pprm = main.prm_Player;
        var center = GetComponent<Collider>().bounds.center;

        // キャラの向き
        var direction = transform.rotation * new Vector3(0, 0, 1);

        // 通常攻撃
        if (GameInput.IsPress(GameInput.Buttons.NormalAttack)
            && (state != PlayerState.Damage))
        {
            var na = Instantiate(normal_attack, main.attackParent);
            if (tmpData.GetGeneralDataInt(GameConstant.DATA_PLAYERID, 0) == (int)GameConstant.PlayerID.Koob)
                na.SetAttackRate(pprm.stat_magic.value);
            else
                na.SetAttackRate(pprm.stat_melee.value);
            na.gameObject.SetActive(false);
            na.Shoot(center, direction);
        }
    }

    #endregion

    #region 接地判定処理

    /// <summary>
    /// 地面検索
    /// </summary>
    /// <param name="threathold">上方向の猶予</param>
    /// <param name="hitGround"></param>
    /// <param name="hitInfo"></param>
    /// <returns></returns>
    private bool GroundSearch(float threathold, out GameGround hitGround, out RaycastHit hitInfo)
    {
        hitGround = null;

        // Ray更新
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
        var dt = manager.GetComponent<OriginManager>().inGameDeltaTime;
        var old_y = transform.position.y;

        if (state == PlayerState.Stand)
        {
            var groundHit = GroundSearch(0f, out GameGround ground, out RaycastHit hitInfo);

            // ジャンプ
            if (GameInput.IsPress(GameInput.Buttons.Jump))
            {
                Jump();
            }
            else if (!groundHit)
            {
                // 地面が無くなったら落下
                jump_count = 1;
                y_speed = 0f;
                state = PlayerState.Jump;
                anim?.SetBool("Jump", false);
                anim?.SetBool("FreeFall", true);
                anim?.SetBool("Grounded", false);
            }
        }
        else if (state == PlayerState.Jump)
        {
            if (GameInput.IsPress(GameInput.Buttons.Jump))
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

                // 地面判定
                var groundHit = GroundSearch(old_y - newPos.y, out GameGround ground, out RaycastHit hitInfo);

                // 物を踏んだら着地
                if (groundHit && (old_y - newPos.y) > 0f)
                {
                    jump_count = 0;
                    y_speed = 0f;
                    transform.position = hitInfo.point;
                    state = PlayerState.Stand;
                    anim?.SetBool("Jump", false);
                    anim?.SetBool("FreeFall", false);
                    anim?.SetBool("Grounded", true);
                }
            }
        }
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        var pprm = GameMainSystem.Instance.prm_Player;

        if (jump_count < pprm.stat_jump.value)
        {
            var manager = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();
            var dt = manager.inGameDeltaTime;
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
        var pprm = GameMainSystem.Instance.prm_Player;

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
        move = Quaternion.Euler(0, camRot, 0) * move * pprm.stat_speed.value;
        rigid.linearVelocity = new Vector3(move.x, vy, move.z);

        // キャラの向き
        var chrRot = Quaternion.LookRotation(move, new Vector3(0, 1f, 0));
        transform.rotation = chrRot;
    }

    #endregion

    #region ダメージ処理

    /// <summary>
    /// 固定値回復
    /// </summary>
    /// <param name="heal"></param>
    public void Heal(int heal)
    {
        var pprm = GameMainSystem.Instance.prm_Player;

        hp += heal;
        if (hp > pprm.stat_maxHp.value) hp = pprm.stat_maxHp.value;

        ui_hp.SetHP(hp, pprm.stat_maxHp.value);
    }

    /// <summary>
    /// 最大値の割合回復
    /// </summary>
    /// <param name="rate"></param>
    public void HealRate(float rate)
    {
        var pprm = GameMainSystem.Instance.prm_Player;
        var healVal = Mathf.FloorToInt(rate * pprm.stat_maxHp.value);
        Heal(healVal);
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
        var pprm = GameMainSystem.Instance.prm_Player;

        ui_hp.SetHP(hp, pprm.stat_maxHp.value);
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
