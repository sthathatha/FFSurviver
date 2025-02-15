using System.Collections;
using UnityEngine;

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

    /// <summary>身長</summary>
    private float height;

    /// <summary>プレイヤー状態</summary>
    public enum PlayerState
    {
        /// <summary>立ち</summary>
        Stand = 0,
        /// <summary>ジャンプ</summary>
        Jump,
        /// <summary>ダメージ</summary>
        Damage,
        /// <summary>死亡処理</summary>
        Death,
    }
    public PlayerState state { get; private set; }

    /// <summary>空中上下速度</summary>
    private float y_speed = 0f;

    /// <summary>ジャンプ回数</summary>
    private int jump_count = 0;

    /// <summary>浮遊時間</summary>
    private float float_time = 0f;

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
        height = 1.2f;

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
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active)
            return;
        if (state == PlayerState.Death)
            return;

        // メニュー処理中は下の処理をしない
        if (!MenuControl())
            return;

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
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active)
            return;
        if (state == PlayerState.Death)
            return;
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
        var game = main.prm_Game;
        var pprm = main.prm_Player;
        var center = GetComponent<Collider>().bounds.center;

        // キャラの向き
        var direction = transform.rotation * new Vector3(0, 0, 1);

        // 通常攻撃
        if (GameInput.IsPress(GameInput.Buttons.NormalAttack)
            && (state != PlayerState.Damage))
        {
            // ウーラの場合敵を検索
            if (GameConstant.GetTempPID() == GameConstant.PlayerID.Worra)
            {
                const float r = 3f;
                const float dist = 10f;
                var enemyMask = LayerMask.GetMask("Enemy");
                if (Physics.SphereCast(center, r, direction, out RaycastHit hInfo, dist, enemyMask))
                {
                    // 見つけたらそっちに撃つ
                    direction = hInfo.collider.bounds.center - center;
                }
            }

            var na = Instantiate(normal_attack, main.attackParent);
            if (GameConstant.GetTempPID() == GameConstant.PlayerID.Koob)
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
    public static bool GroundSearch(Vector3 nowPos, float threathold, Ray useRay, out GameGround hitGround, out RaycastHit hitInfo)
    {
        hitGround = null;
        var layer = LayerMask.GetMask(new string[] { "Ground" });

        // 0.fは必ず地面扱いにする
        if (nowPos.y < 0f)
        {
            // 固定座標で設定
            hitGround = null;
            hitInfo = new RaycastHit();
            hitInfo.point = new Vector3(nowPos.x, 0f, nowPos.z);

            return true;
        }

        // Ray更新
        if (threathold < 0.2f) threathold = 0.2f;
        useRay.origin = nowPos + new Vector3(0, threathold, 0);

        // 検索
        var groundHit = Physics.Raycast(useRay,
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
        var wpn = GameMainSystem.Instance.weaponManager;

        if (state == PlayerState.Stand)
        {
            var groundHit = GroundSearch(transform.position, 0f, ground_ray, out GameGround ground, out RaycastHit hitInfo);

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

                if (wpn.HaveWeapon(WeaponManager.ID.FloatBody))
                    float_time = wpn.GetWeaponSlot(WeaponManager.ID.FloatBody).AsFloat().validTime;
                else
                    float_time = 0f;
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

                // 浮遊処理
                if (y_speed <= 0f &&
                    float_time > 0f &&
                    GameInput.IsKeep(GameInput.Buttons.Jump))
                {
                    y_speed = 0;
                    float_time -= dt;
                }

                var newPos = transform.position + new Vector3(0, y_speed * dt, 0);
                // 落ち過ぎたら上に出る
                if (newPos.y < -20f) newPos.y = 20f;
                transform.position = newPos;

                // 地面判定
                var groundHit = GroundSearch(newPos, old_y - newPos.y, ground_ray, out GameGround ground, out RaycastHit hitInfo);

                // 物を踏んだら着地
                if (groundHit && (old_y - newPos.y) > 0f)
                {
                    Grounding(hitInfo.point);

                    //
                    GameMainSystem.Instance.isStandingBase = ground == null || ground.IsBaseGround;
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
        var wpn = GameMainSystem.Instance.weaponManager;

        if (jump_count < pprm.stat_jump.value)
        {
            var manager = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();
            var dt = manager.inGameDeltaTime;
            jump_count++;
            y_speed = JUMP_V0;
            state = PlayerState.Jump;
            if (wpn.HaveWeapon(WeaponManager.ID.FloatBody))
                float_time = wpn.GetWeaponSlot(WeaponManager.ID.FloatBody).AsFloat().validTime;
            else
                float_time = 0f;

            transform.position += new Vector3(0, y_speed * dt, 0);

            // ジャンプアニメーション
            anim?.SetBool("Jump", true);
            anim?.SetBool("Grounded", false);
            anim?.SetBool("FreeFall", false);
        }
    }

    /// <summary>
    /// 着地処理
    /// </summary>
    /// <param name="hitPos"></param>
    private void Grounding(Vector3 hitPos)
    {
        jump_count = 0;
        y_speed = 0f;
        transform.position = hitPos;
        state = PlayerState.Stand;
        anim?.SetBool("Jump", false);
        anim?.SetBool("FreeFall", false);
        anim?.SetBool("Grounded", true);

        var wpn = GameMainSystem.Instance.weaponManager;
        if (wpn.HaveWeapon(WeaponManager.ID.Quake))
        {
            var sys = wpn.GetWeaponSlot(WeaponManager.ID.Quake).AsQuake();
            sys.CreateQuake();
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
        move = Quaternion.Euler(0, camRot, 0) * move * pprm.GetSpeedVelocity();
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
        var atk = other.gameObject.GetComponentInParent<AttackParameter>();
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

        // ゲームオーバー処理
        GameMainSystem.Instance.WaitGameover();
        state = PlayerState.Death;
        StartCoroutine(DeathCoroutine());
    }

    /// <summary>
    /// 死亡処理コルーチン
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DeathCoroutine()
    {
        // 水平停止
        rigid.linearVelocity = Vector3.zero;

        var y = new DeltaFloat();
        y.Set(transform.position.y);
        y.MoveTo(transform.position.y + 10f, 0.7f, DeltaFloat.MoveType.DECEL);
        // はねて下まで落ちる
        while (y.IsActive())
        {
            yield return null;
            y.Update();
            var p = transform.position;
            p.y = y.Get();
            transform.position = p;
        }

        y.MoveTo(-5f, 1f, DeltaFloat.MoveType.ACCEL);
        while (y.IsActive())
        {
            yield return null;
            y.Update();
            var p = transform.position;
            p.y = y.Get();
            transform.position = p;
        }

        // 閉じる
        gameObject.SetActive(false);
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
        var camTarget = transform.position;
        camTarget.y += height / 2f; // Collider.bounds.centerを使うとモデルより１フレ遅れるためpositionで操作
        cam.SetTargetPos(camTarget);

        // カメラリセット
        if (GameInput.IsPress(GameInput.Buttons.CameraReset))
        {
            var resetDir = transform.rotation * new Vector3(0, -0.15f, 1);
            cam.SetRotateTime(resetDir, 0.2f);
        }
        else
        {
            if (Mathf.Abs(stick.x) > 0.1f)
                cam.SetRotateLR(stick.x);
            if (Mathf.Abs(stick.y) > 0.1f)
                cam.SetRotateUD(stick.y);
        }

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
