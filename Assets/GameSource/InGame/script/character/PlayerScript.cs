using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[
/// </summary>
public class PlayerScript : CharacterScript
{
    #region �����o�[

    #region �p�����[�^

    /// <summary>HP</summary>
    private UIHpGauge ui_hp;

    #endregion

    /// <summary>�ʏ�U��</summary>
    public SimpleAttack normal_attack;

    /// <summary>�ړ��ō���</summary>
    public float run_speed = 10f;

    /// <summary>�ڒn����pRay</summary>
    private Ray ground_ray;

    /// <summary>�v���C���[</summary>
    public enum PlayerState
    {
        /// <summary>����</summary>
        Stand = 0,
        /// <summary>�W�����v</summary>
        Jump,
        /// <summary>�_���[�W</summary>
        Damage,
    }
    public PlayerState state { get; private set; }

    /// <summary>�󒆏㉺���x</summary>
    private float y_speed = 0f;

    /// <summary>�W�����v�\��</summary>
    private int jump_count_max = 1;

    /// <summary>�W�����v��</summary>
    private int jump_count = 0;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();

        // �ŏ��̎擾�Ȃ�
        ui_hp = GameMainSystem.Instance.ui_hp;
        normal_attack.gameObject.SetActive(false);

        var cam = ManagerSceneScript.GetInstance().GetCamera3D();
        cam.SetCameraDist(10f);
        CameraControl(true);

        ground_ray = new Ray();
        ground_ray.direction = new Vector3(0, -1f, 0);
        state = PlayerState.Stand;

        // �Q�[�����C���Ɏ�����n��
        GameMainSystem.Instance.playerScript = this;

        //todo:�p�����[�^������
        jump_count_max = 1;
        hp_max = 100;
        hp = hp_max;
        ui_hp.SetHP(hp, hp_max);
    }

    /// <summary>
    /// �X�V
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
    /// ��ő���X�V
    /// </summary>
    protected override void UpdateCharacter2()
    {
        base.UpdateCharacter2();
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;
        CameraControl();
    }

    #endregion

    #region �U������

    /// <summary>
    /// �U������
    /// </summary>
    private void AttackControl()
    {
        var main = GameMainSystem.Instance;

        // �L�����̌���
        var direction = transform.rotation * new Vector3(0, 0, 1);

        // �ʏ�U��
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

    #region �ڒn���菈��

    /// <summary>
    /// �n�ʌ���
    /// </summary>
    /// <param name="hitGround"></param>
    /// <param name="hitInfo"></param>
    /// <returns></returns>
    private bool GroundSearch(out GameGround hitGround, out RaycastHit hitInfo)
    {
        hitGround = null;

        // Ray�X�V
        // ����P�\
        var threathold = -y_speed * ManagerSceneScript.GetInstance().validDeltaTime;
        if (threathold < 0.2f) threathold = 0.2f;
        ground_ray.origin = transform.position + new Vector3(0, threathold, 0);

        // ����
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
    /// �ڒn���菈��
    /// </summary>
    private void GroundControl()
    {
        var manager = ManagerSceneScript.GetInstance();
        var dt = manager.validDeltaTime;

        // Ray����
        var groundHit = GroundSearch(out GameGround ground, out RaycastHit hitInfo);

        if (state == PlayerState.Stand)
        {
            // �W�����v
            if (GameInput.IsPress(GameInput.Buttons.Jump))
            {
                Jump();
            }
            else if (!groundHit)
            {
                // �n�ʂ������Ȃ����痎��
                y_speed = 0f;
                state = PlayerState.Jump;
                anim?.SetBool("Jump", false);
                anim?.SetBool("FreeFall", true);
                anim?.SetBool("Grounded", false);
            }
        }
        else if (state == PlayerState.Jump)
        {
            // �㏸���͒��n���Ȃ�
            if (groundHit)
            {
                if (y_speed > 0f) groundHit = false;
            }

            // ���𓥂񂾂璅�n
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
                // �X�y�[�X�L�[�œ�i�W�����v
                Jump();
            }
            else
            {
                // ���ɉ���
                y_speed -= FALL_G * dt;
                if (y_speed < FALL_MAX) y_speed = FALL_MAX;
                var newPos = transform.position + new Vector3(0, y_speed * dt, 0);
                // �����߂������ɏo��
                if (newPos.y < -20f) newPos.y = 20f;

                transform.position = newPos;
            }
        }
    }

    /// <summary>
    /// �W�����v����
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

            // �W�����v�A�j���[�V����
            anim?.SetBool("Jump", true);
            anim?.SetBool("Grounded", false);
            anim?.SetBool("FreeFall", false);
        }
    }

    #endregion

    #region �ړ�����

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void MoveControl()
    {
        // Y���x�͕ێ�
        var vy = rigid.linearVelocity.y;

        // �ړ��x�[�X
        var stick = GameInput.GetLeftStick();

        if (stick.magnitude < 0.1f)
        {
            rigid.linearVelocity = new Vector3(0, vy, 0);
            return;
        }

        // �J������]��y����
        var cam = ManagerSceneScript.GetInstance().GetCamera3D();
        var camRot = cam.RotateLR * Mathf.Rad2Deg;

        // �X�e�B�b�N�l���J������]�ƍ���
        var move = new Vector3(stick.x, 0, stick.y);
        move = Quaternion.Euler(0, camRot, 0) * move * run_speed;
        rigid.linearVelocity = new Vector3(move.x, vy, move.z);

        // �L�����̌���
        var chrRot = Quaternion.LookRotation(move, new Vector3(0, 1f, 0));
        transform.rotation = chrRot;
    }

    #endregion

    #region �_���[�W����

    /// <summary>
    /// ��
    /// </summary>
    /// <param name="heal"></param>
    private void Heal(int heal)
    {
        hp += heal;
        if (hp > hp_max) hp = hp_max;

        ui_hp.SetHP(hp, hp_max);
    }

    /// <summary>
    /// �G����̐ڐG��
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
    /// �_���[�W�q�b�g
    /// </summary>
    protected override void DamageHit()
    {
        base.DamageHit();
        ui_hp.SetHP(hp, hp_max);
    }

    /// <summary>
    /// ���S����
    /// </summary>
    protected override void DamageDeath()
    {
        base.DamageDeath();

        //todo:�Q�[���I�[�o�[����
    }

    #endregion

    #region �J��������

    /// <summary>
    /// �J��������
    /// </summary>
    /// <param name="noInput">���͕s��</param>
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

    #region ���f���A�j���[�V��������

    /// <summary>
    /// ���f���A�j���[�V��������
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
