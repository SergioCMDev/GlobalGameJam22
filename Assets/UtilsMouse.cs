using UnityEngine.InputSystem;

public static class UtilsMouse
{
    public static bool IsLeftButtonPressedThisFrame()
    {
        return Mouse.current.leftButton.wasPressedThisFrame;
    }
}