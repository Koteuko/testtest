using UnityEngine;

public static class Settings
{
    [Header("Player")]
    public static float speed = 5;
    public static float accelTime = 0.12f;
    public static float rotationSpeed = 10;
    public static float gravity = -20.81f;
    public static float jumpBufferTime = 0.1f;
    public static float coyoteTime = 0.2f;
    public static float accelRate => speed / Mathf.Max(0.0001f, accelTime);
    public static float jumpHeight = 1.5f;
    
    [Header("Camera")]
    public static KeyCode zoomInKey = KeyCode.LeftControl;
    public static float zoomSpeed = 10;
    public static float minZoom = 5;
    public static float maxZoom = 30;
    public static float panSensitivity = 0.0125f;
    public static float maxPanDistance = 25;
    public static float returnSpeed = 8;
    
    [Header("Other")]
    private static bool _visibleCoursor;

    public static bool visibleCursor
    {
        get => _visibleCoursor;
        set
        {
            _visibleCoursor = value;
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
