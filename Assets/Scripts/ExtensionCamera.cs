using UnityEngine;

public class ExtensionCamera : MonoBehaviour
{
    private Camera cam;
    private Vector3 initialLocalPos;
    private bool isPanning;
    private Vector3 lastMousePos;
    private float targetZoom;

    void Awake()
    {
        cam = GameMaster.mainCamera;
        initialLocalPos = cam.transform.localPosition;
        targetZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;
    }

    void Update()
    {
        Zoom();
        HandlePan();
    }

    private void Zoom()
    {
        bool zoomAllowed = Input.GetKey(Settings.zoomInKey);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (zoomAllowed && Mathf.Abs(scroll) >= 0.0001f)
        {
            int step = (int)Mathf.Sign(scroll);
            targetZoom -= step * 1f;
            targetZoom = Mathf.Clamp(targetZoom, Settings.minZoom, Settings.maxZoom);
        }

        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoom, Settings.zoomSpeed * Time.deltaTime);
    }

    private void HandlePan()
    {
        bool cursorFree = Cursor.visible && Cursor.lockState == CursorLockMode.None;

        if (Input.GetMouseButtonDown(2))
        {
            isPanning = true;
            if (cursorFree)
                lastMousePos = Input.mousePosition;
            else
                lastMousePos = Vector3.zero;
        }

        if (Input.GetMouseButton(2) && isPanning)
        {
            Vector3 delta;
            if (cursorFree)
            {
                Vector3 currentMouse = Input.mousePosition;
                delta = currentMouse - lastMousePos;
                lastMousePos = currentMouse;
            }
            else
            {
                float dx = Input.GetAxis("Mouse X");
                float dy = Input.GetAxis("Mouse Y");
                const float axisScale = 15f;
                delta = new Vector3(dx * axisScale, dy * axisScale, 0f);
            }

            if (delta.sqrMagnitude > 0f)
            {
                Vector3 worldDelta = cam.transform.right * (-delta.x) + cam.transform.up * (-delta.y);
                Transform parent = cam.transform.parent;
                Vector3 localDelta;
                if (parent != null)
                    localDelta = parent.InverseTransformDirection(worldDelta) * Settings.panSensitivity;
                else
                    localDelta = cam.transform.InverseTransformDirection(worldDelta) * Settings.panSensitivity;

                cam.transform.localPosition += localDelta;

                Vector3 offset = cam.transform.localPosition - initialLocalPos;
                if (offset.magnitude > Settings.maxPanDistance)
                    cam.transform.localPosition = initialLocalPos + offset.normalized * Settings.maxPanDistance;
            }
        }

        if (Input.GetMouseButtonUp(2))
            isPanning = false;

        if (!isPanning)
        {
            if ((cam.transform.localPosition - initialLocalPos).sqrMagnitude > 0.000001f)
            {
                cam.transform.localPosition = Vector3.Lerp(
                    cam.transform.localPosition,
                    initialLocalPos,
                    Mathf.Clamp01(Settings.returnSpeed * Time.deltaTime)
                );
            }
        }
    }
}