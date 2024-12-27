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

    /// <summary>移動最高速</summary>
    public float run_speed = 10f;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    protected override IEnumerator InitCharacter()
    {
        var cam = ManagerSceneScript.GetInstance().GetCamera3D();
        cam.SetCameraDist(10f);
        CameraControl(true);

        // ゲームメインに自分を渡す
        GameMainSystem.Instance.playerScript = this;
        yield break;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void UpdateCharacter()
    {
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        MoveControl();
        UpdateAnim();
    }

    /// <summary>
    /// 後で走る更新
    /// </summary>
    protected override void UpdateCharacter2()
    {
        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;
        CameraControl();
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

        var keyboard = Keyboard.current;
        var gamepad = Gamepad.current;
        // 移動ベース
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

    #region カメラ操作

    /// <summary>
    /// カメラ操作
    /// </summary>
    /// <param name="noInput">入力不可</param>
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

    #region モデルアニメーション制御

    /// <summary>
    /// モデルアニメーション制御
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
