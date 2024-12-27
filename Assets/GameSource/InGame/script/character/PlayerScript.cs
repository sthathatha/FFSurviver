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

    /// <summary>�ړ��ō���</summary>
    public float run_speed = 10f;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    protected override IEnumerator InitCharacter()
    {
        var cam = ManagerSceneScript.GetInstance().GetCamera3D();
        cam.SetCameraDist(10f);
        CameraControl(true);

        // �Q�[�����C���Ɏ�����n��
        GameMainSystem.Instance.playerScript = this;
        yield break;
    }

    /// <summary>
    /// �X�V
    /// </summary>
    protected override void UpdateCharacter()
    {
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        MoveControl();
        UpdateAnim();
    }

    /// <summary>
    /// ��ő���X�V
    /// </summary>
    protected override void UpdateCharacter2()
    {
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;
        CameraControl();
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

        var keyboard = Keyboard.current;
        var gamepad = Gamepad.current;
        // �ړ��x�[�X
        //var stick = gamepad.leftStick.ReadValue();
        var stick = new Vector3();
        if (keyboard.dKey.isPressed) stick.x = 1f;
        else if (keyboard.aKey.isPressed) stick.x = -1f;
        if (keyboard.wKey.isPressed) stick.y = 1f;
        else if (keyboard.sKey.isPressed) stick.y = -1f;
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

    #region �J��������

    /// <summary>
    /// �J��������
    /// </summary>
    /// <param name="noInput">���͕s��</param>
    private void CameraControl(bool noInput = false)
    {
        var keyboard = Keyboard.current;
        var gamepad = Gamepad.current;
        var stick = Vector3.zero;
        if (!noInput)
        {
            // stick = gamepad.rightStick.ReadValue();
            if (keyboard.rightArrowKey.isPressed) stick.x = 1f;
            else if (keyboard.leftArrowKey.isPressed) stick.x = -1f;
            if (keyboard.upArrowKey.isPressed) stick.y = 1f;
            else if (keyboard.downArrowKey.isPressed) stick.y = -1f;
        }

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
        var v = rigid.linearVelocity;
        v.y = 0f;
        var spd = v.magnitude / 4f;
        anim?.SetFloat("Speed", spd);
        anim?.SetFloat("MotionSpeed", spd);
    }

    #endregion
}
