using UnityEngine.InputSystem;

public static class UtilsMouse
{
    public static bool IsLeftButtonPressedThisFrame()
    {
        return Mouse.current.leftButton.wasPressedThisFrame;
    }
    
    public static bool IsRightButtonPressedThisFrame()
    {
        return Mouse.current.rightButton.wasPressedThisFrame;
    }
}