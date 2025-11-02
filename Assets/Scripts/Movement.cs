using UnityEngine;

[DefaultExecutionOrder(-100)]
public class Movement : MonoBehaviour
{
    private CharacterController _controller;
    private UnityEngine.Camera _mCamera;

    [SerializeField] private GameObject _body;

    private float _jumpBufferTimer;
    private float _currentSpeed;
    private float _coyoteTimer;
    private float _verticalVelocity;
    private bool _isAccelerating;
    private bool _hadInputLastFrame;
    
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _mCamera = GameMaster.mainCamera;
        _mCamera.transform.SetParent(transform);
        _mCamera.transform.localPosition = new Vector3(0, 2, 0);
        _mCamera.transform.localRotation = Quaternion.Euler(30, 40, 0);
        Settings.visibleCursor = false;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
            _jumpBufferTimer = Settings.jumpBufferTime;
        else
            _jumpBufferTimer -= Time.deltaTime;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        bool hasInput = direction.sqrMagnitude > 0f;

        if (hasInput)
        {
            direction.Normalize();

            if (!_hadInputLastFrame)
                _isAccelerating = true;

            if (_isAccelerating)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, Settings.speed, Settings.accelRate * Time.deltaTime);
                if (_currentSpeed >= Settings.speed - 0.001f)
                    _isAccelerating = false;
            }
            else
                _currentSpeed = Settings.speed;

            RotateTowardsDirection(direction);
        }
        else
        {
            _currentSpeed = 0f;
            _isAccelerating = false;
        }

        if (_controller.isGrounded)
        {
            _coyoteTimer = Settings.coyoteTime;
            if (_verticalVelocity < 0f)
                _verticalVelocity = -2f; // небольшое отрицательное значение, чтобы держаться при контакте с землей
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
            _verticalVelocity += Settings.gravity * Time.deltaTime;
        }

        if (_jumpBufferTimer > 0f && _coyoteTimer > 0f)
        {
            _verticalVelocity = Mathf.Sqrt(-2f * Settings.gravity * Settings.jumpHeight);
            _jumpBufferTimer = 0f;
            _coyoteTimer = 0f;
        }

        Vector3 move = direction * _currentSpeed * Time.deltaTime;
        move.y = _verticalVelocity * Time.deltaTime;

        _controller.Move(move);
        _hadInputLastFrame = hasInput;
    }

    private void RotateTowardsDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        _body.transform.rotation = Quaternion.Slerp(
            _body.transform.rotation,
            targetRotation,
            Settings.rotationSpeed * Time.deltaTime
        );
    }
}
