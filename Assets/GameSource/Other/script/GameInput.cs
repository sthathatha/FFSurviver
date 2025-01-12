using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput
{
    /// <summary>
    /// ボタン
    /// </summary>
    public enum Buttons
    {
        Jump = 0,
        NormalAttack,
        Avoid,
        Menu,
        OK,
        Cancel,
    }

    /// <summary>
    /// ボタン操作
    /// </summary>
    /// <returns></returns>
    public static bool IsPress(Buttons btn)
    {
        var keyboard = Keyboard.current;
        var gamepad = Gamepad.current;

        switch (btn)
        {
            case Buttons.Jump:
                return keyboard?.spaceKey.wasPressedThisFrame == true ||
                    gamepad?.buttonSouth.wasPressedThisFrame == true;
            case Buttons.NormalAttack:
                return keyboard?.enterKey.wasPressedThisFrame == true ||
                    gamepad?.buttonWest.wasPressedThisFrame == true;
            case Buttons.Avoid:
                return keyboard?.cKey.wasPressedThisFrame == true ||
                    gamepad?.rightTrigger.wasPressedThisFrame == true;
            case Buttons.Menu:
                return keyboard?.backspaceKey.wasPressedThisFrame == true ||
                    gamepad?.buttonNorth.wasPressedThisFrame == true;
            case Buttons.OK:
                return keyboard?.enterKey.wasPressedThisFrame == true ||
                    gamepad?.buttonSouth?.wasPressedThisFrame == true;
            case Buttons.Cancel:
                return keyboard?.backspaceKey.wasPressedThisFrame == true ||
                    gamepad?.buttonEast.wasPressedThisFrame == true;

        }

        return false;
    }

    /// <summary>
    /// 左スティック操作
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetLeftStick()
    {
        var keyInput = false;
        var ret = Vector2.zero;
        var keyboard = Keyboard.current;
        var gamepad = Gamepad.current;

        if (keyboard?.dKey.isPressed == true) { ret.x = 1f; keyInput = true; }
        else if (keyboard?.aKey.isPressed == true) { ret.x = -1f; keyInput = true; }
        if (keyboard?.wKey.isPressed == true) { ret.y = 1f; keyInput = true; }
        else if (keyboard?.sKey.isPressed == true) { ret.y = -1f; keyInput = true; }

        if (gamepad != null && !keyInput)
        {
            ret = gamepad.leftStick.ReadValue();
        }

        return ret;
    }

    /// <summary>
    /// 右スティック操作
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetRightStick()
    {
        var keyInput = false;
        var ret = Vector2.zero;
        var keyboard = Keyboard.current;
        var gamepad = Gamepad.current;

        if (keyboard?.rightArrowKey.isPressed == true) { ret.x = 1f; keyInput = true; }
        else if (keyboard?.leftArrowKey.isPressed == true) { ret.x = -1f; keyInput = true; }
        if (keyboard?.upArrowKey.isPressed == true) { ret.y = 1f; keyInput = true; }
        else if (keyboard?.downArrowKey.isPressed == true) { ret.y = -1f; keyInput = true; }

        if (gamepad != null && !keyInput)
        {
            ret = gamepad.rightStick.ReadValue();
        }

        return ret;
    }
}
