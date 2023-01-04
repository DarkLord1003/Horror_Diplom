using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _input;

    [Header("Look Parameters")]
    [SerializeField] [Range(0f, 10f)] private float _xSens;
    [SerializeField] [Range(0f, 10f)] private float _ySens;
    [SerializeField] [Range(0f, 1f)] private float _smoothSpeedAxisX;
    [SerializeField] [Range(0f, 1f)] private float _smoothSpeedAxisY;
    [SerializeField] private float _yClampMin;
    [SerializeField] private float _yClampMax;
    [SerializeField] private bool _inverseX;
    [SerializeField] private bool _inverseY;

    private Vector3 _targetPositionLookAtObject;
    private Vector3 _targetPositionlookAtObjectVelocity;

    [Header("Move Parameters")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _walkSpeedCrouch;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jumpHeight;

    [Header("Gravity Parameters")]
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _stickToGroundForce;
    private float _fallingTimer;

    [Header("Stamina Parameters")]
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _staminaDrain;
    [SerializeField] private float _staminaRestore;
    [SerializeField] private float _staminaRestoreTime;

    [Header("Crouch/Stand Parameters")]
    [SerializeField] private PlayerBodyState _stand;
    [SerializeField] private PlayerBodyState _crouch;
    private bool _duringCrouchAnimation;

    [Header("Smoothing Parameters")]
    [SerializeField] private float _smoothSpeed;

  

    private PlayerStateController _stateController;

    private PlayerStateMachine _stateMachine;
    private CharacterController _characterController;
    private Camera _camera;
    private Animator _anim;
    private PlayerBodyState _currentBodyState;
    private PlayerMoveStatus _moveStatus;

    private Vector3 _inputVector;
    private Vector3 _cameraRotation;
    private Vector3 _targetCameraRotation;
    private Vector3 _targetCameraRotationVelocity;
    private Vector3 _playerRotation;
    private Vector3 _targetPlayerRotation;
    private Vector3 _targetPlayerRotationVelocity;
    private Vector3 _moveDirection;

    private bool _isWalking;
    private bool _isSprinting;
    private bool _isJumping;
    private bool _isCrouching;

    private bool _previouslyIsGrounded;
    private float _currentStamina;
    private float _currentStaminaRestoreTime;

    public PlayerMoveStatus MoveStatus => _moveStatus;
    public Vector3 InputVector => _inputVector;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _anim = GetComponentInChildren<Animator>();
        _camera = Camera.main;
        _stateController = new PlayerStateController();

        _stateController.InitStates(_stateMachine, this);
    }

    private void Start()
    {
        _currentStamina = _maxStamina;
        _currentStaminaRestoreTime = _staminaRestoreTime;
        _currentBodyState = _stand;

    }

    private void Update()
    {
        FallingTimer();
        Look();
        Move();
        CalculateStamina();
        CalculatePlayerStatus();

    }


    private void Look()
    {
        _targetCameraRotation.x += _ySens * (_inverseY ? _input.LookInput.y : -_input.LookInput.y) * Time.deltaTime;
        _targetPlayerRotation.y += _xSens * (_inverseX ? -_input.LookInput.x : _input.LookInput.x) * Time.deltaTime;

        _targetCameraRotation.x = Mathf.Clamp(_targetCameraRotation.x, _yClampMin, _yClampMax);

        _cameraRotation = Vector3.SmoothDamp(_cameraRotation, _targetCameraRotation, ref _targetCameraRotationVelocity,
                                             _smoothSpeedAxisY * Time.deltaTime);
        _playerRotation = Vector3.SmoothDamp(_playerRotation, _targetPlayerRotation, ref _targetPlayerRotationVelocity,
                                             _smoothSpeedAxisX * Time.deltaTime);

        _camera.transform.localRotation = Quaternion.Euler(_cameraRotation);

        transform.localRotation = Quaternion.Euler(_playerRotation);
    }

    private void Move()
    {
        _isSprinting = _input.SprintPress && !(_currentStamina < _maxStamina / 4f) && !_isCrouching;
        _isWalking = !_isSprinting;

        if (_isSprinting)
        {
            if (_currentStamina < _maxStamina / 4f)
            {
                _isWalking = true;
                _currentStaminaRestoreTime = _staminaRestoreTime;
            }
        }

        float speed = _isWalking ? (_isCrouching ? _walkSpeed : _walkSpeed) : _sprintSpeed;

        _inputVector = new Vector2(_input.MoveInput.x, _input.MoveInput.y);

        if (Mathf.Abs(_input.MoveInput.sqrMagnitude) < 0.2f)
            _isWalking = false;

        if (_inputVector.sqrMagnitude > 1f)
            _inputVector.Normalize();

        Vector3 desiredMove = transform.right * _inputVector.x + transform.forward * _inputVector.y;

        if (Physics.SphereCast(transform.position, _characterController.radius, Vector2.down, out RaycastHit hit,
                              _characterController.height / 2f, 1))
        {
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hit.normal);
        }

        _moveDirection.x = Mathf.Lerp(_moveDirection.x, desiredMove.x * speed, _smoothSpeed);
        _moveDirection.z = Mathf.Lerp(_moveDirection.z, desiredMove.z * speed, _smoothSpeed);

        if (_characterController.isGrounded)
        {
            _moveDirection.y = -_stickToGroundForce;

            if (_input.JumpPress && !_isCrouching)
            {
                _moveDirection.y = _jumpHeight;
                _isJumping = true;
            }
        }
        else
        {
            _moveDirection += Physics.gravity * _gravityMultiplier * Time.deltaTime;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private void CalculatePlayerStatus()
    {
        if (!_previouslyIsGrounded && _characterController.isGrounded)
        {
            if (_fallingTimer > 0.5f)
            {
            }

            _moveDirection.y = 0f;
            _isJumping = false;
            _moveStatus = PlayerMoveStatus.Landing;
        }
        else if(_previouslyIsGrounded && !_characterController.isGrounded)
        {
            _moveStatus = PlayerMoveStatus.Jumping;
        }
        else if (!_characterController.isGrounded)
        {
            _moveStatus = PlayerMoveStatus.NotGrounded;
        }
        else if (_isWalking && !_isCrouching)
        {
            _moveStatus = PlayerMoveStatus.Walking;
        }
        else if(_isSprinting && !_isCrouching)
        {
            _moveStatus = PlayerMoveStatus.Sprinting;
        }
        else if(_isCrouching)
        {
            _moveStatus = PlayerMoveStatus.Crouching;
        }
        else
        {
            _moveStatus = PlayerMoveStatus.NotMoving;
        }

        _previouslyIsGrounded = _characterController.isGrounded;
    }

    private void FallingTimer()
    {
        if (_characterController.isGrounded)
        {
            _fallingTimer = 0f;
        }
        else
        {
            _fallingTimer += Time.deltaTime;
        }
    }

    private void CalculateStamina()
    {
        if (_isSprinting)
        {
            _currentStamina -= _staminaDrain * Time.deltaTime;
        }
        else
        {
            if (_currentStamina < _maxStamina)
            {
                if (_currentStaminaRestoreTime <= 0f)
                {
                    _currentStamina += _staminaRestore * Time.deltaTime;

                    if (_currentStamina > _maxStamina)
                    {
                        _currentStamina = _maxStamina;
                    }
                }
                else
                {
                    _currentStaminaRestoreTime -= Time.deltaTime;
                }
            }
            else
            {
                _currentStaminaRestoreTime = _staminaRestoreTime;
            }
        }
    } 

    private void CrouchingStanding()
    {
        if (!_duringCrouchAnimation)
        {
            StartCoroutine(StandCrouch());
        }
    }

    private IEnumerator StandCrouch()
    {
        _duringCrouchAnimation = true;
        float elepsedTime = 0;

        _currentBodyState = !_isCrouching ? _crouch : _stand;
        _isCrouching = !_isCrouching;

        while(elepsedTime < _currentBodyState.TimeToState)
        {
            _characterController.height = Mathf.Lerp(_characterController.height, _currentBodyState.Height,
                                                     elepsedTime / _currentBodyState.TimeToState);
            _characterController.center = Vector3.Lerp(_characterController.center, _currentBodyState.ColliderCenter,
                                                       elepsedTime / _currentBodyState.TimeToState);


            elepsedTime += Time.deltaTime;
            yield return null;
        }

        _duringCrouchAnimation = false;

    }

    private void OnEnable()
    {
        _input.Crouch = CrouchingStanding;
    }

}

public enum PlayerMoveStatus
{
    NotMoving,
    Walking,
    Sprinting,
    Landing,
    NotGrounded,
    Jumping,
    Crouching
}

[System.Serializable]
public class PlayerBodyState
{
    [SerializeField] private float _height;
    [SerializeField] private float _cameraHeight;
    [SerializeField] private float _timeToState;
    [SerializeField] private Vector3 _coliiderCenter;

    public float Height
    {
        get => _height;
        set => _height = value;
    }

    public float CameraHeight
    {
        get => _cameraHeight;
        set => _cameraHeight = value;
    }

    public float TimeToState
    {
        get => _timeToState;
        set => _timeToState = value;
    }

    public Vector3 ColliderCenter
    {
        get => _coliiderCenter;
        set => _coliiderCenter = value;
    }


}
