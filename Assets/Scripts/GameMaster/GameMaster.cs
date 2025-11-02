using UnityEngine;

public static class GameMaster
{
    private static UnityEngine.Camera _mainCamera;
    
    public static UnityEngine.Camera mainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = UnityEngine.Camera.main;
            
            return _mainCamera;
        }
    }
}
