using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Reads the keyboard. Works with both the old and the new Unity input system.
public static class GameInput
{
    // Returns the move direction: x = left/right, y = forward/back.
    public static Vector2 Move()
    {
        float x = 0f;
        float y = 0f;
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        if (k != null)
        {
            if (k.aKey.isPressed || k.leftArrowKey.isPressed) x -= 1f;
            if (k.dKey.isPressed || k.rightArrowKey.isPressed) x += 1f;
            if (k.sKey.isPressed || k.downArrowKey.isPressed) y -= 1f;
            if (k.wKey.isPressed || k.upArrowKey.isPressed) y += 1f;
        }
#else
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
#endif
        return new Vector2(x, y);
    }

    // True on the frame the R key is pressed.
    public static bool RestartPressed()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        return k != null && k.rKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.R);
#endif
    }
}
