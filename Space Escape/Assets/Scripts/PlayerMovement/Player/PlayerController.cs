using UnityEngine;

public class PlayerController : StateMachine<PlayerState>
{
    #region Getters
    public PlayerInputActions Actions => _actions;
    public Transform Orientation => _orientation;
    public Transform MantleCheck => _mantleCheck;
    public Transform CameraHolder => _cameraHolder;
    public Rigidbody Rigidbody => _rigidbody;
    public CapsuleCollider Collider => _collider;
    public Inventory Inventory => _inventory;
    public Transform ItemHolder => _itemHolder;
    public float WalkSpeed => _walkSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float CrouchMultiplier => _crouchMultiplier;
    public float MoveSpeed => _moveSpeed;
    public float GroundDrag => _groundDrag;
    public Vector3 InputDirection => _inputDirection;
    public float JumpForce => _jumpForce;
    public float AirMultiplier => _airMultiplier;
    public float CrouchHeight => _crouchHeight;
    public float StandHeight => _standHeight;
    public LayerMask Ground => _ground;
    public Animator BodyAnimator => _bodyAnimator;
    public Animator ArmsAnimator => _armsAnimator; 
    #endregion

    PlayerInputActions _actions;
    Transform _transform;

    [Header("Object References")]
    [SerializeField] Transform _orientation;
	[SerializeField] Transform _cameraHolder;

    [Space]

	[SerializeField] Rigidbody _rigidbody;
	[SerializeField] CapsuleCollider _collider;

    [Space]

    [SerializeField] Inventory _inventory;
    [SerializeField] Transform _itemHolder;

    [Space]

	[SerializeField] Transform _mantleCheck;

    [Space]

    [SerializeField] Animator _bodyAnimator;
    [SerializeField] Animator _armsAnimator;

    [Header("Movement")]
    [SerializeField] float _walkSpeed;
    [SerializeField] float _sprintSpeed;
    [SerializeField] float _crouchMultiplier;
    [SerializeField] float _moveSpeed;

    [Space]
    
    [SerializeField] float _groundDrag;

    Vector3 _inputDirection;

	[Header("Jumping")]
    [SerializeField] float _jumpForce;
	[SerializeField] float _jumpCooldown;

    [Space]

	[SerializeField] float _airMultiplier;

    [Header("Crouching")]
    [SerializeField] float _crouchHeight;
    [SerializeField] float _standHeight;

    [Header("Ground Check")]
    [SerializeField] LayerMask _ground;
    [SerializeField] float _groundCheckDistance;

	[Header("Slope Handling")]
    [SerializeField] float _maxSlopeAngle;

    public bool OnGround => Physics.CheckSphere(_transform.position, _groundCheckDistance, _ground);

	// Base States
    public PlayerGroundedState Grounded;
    public PlayerCrouchingState Crouching;
    public PlayerJumpingState Jumping;
    public PlayerFallingState Falling;
    public PlayerDeadState Dead;

    // Sub States
	public PlayerIdleState Idle;
    public PlayerWalkingState Walking;
    public PlayerSprintingState Sprinting;
    public PlayerMantleState Mantle;

	void Awake()
    {
        _transform = transform;

        _actions = new PlayerInputActions();
        _actions.Enable();

        _transform.rotation = Quaternion.identity;
        
        GameManager.OnCursorToggled += ToggleInput;

        Grounded = new PlayerGroundedState(this);
        Crouching = new PlayerCrouchingState(this);
        Jumping = new PlayerJumpingState(this);
        Falling = new PlayerFallingState(this);
        Dead = new PlayerDeadState(this);

        Idle = new PlayerIdleState(this);
        Walking = new PlayerWalkingState(this);
        Sprinting = new PlayerSprintingState(this);
        Mantle = new PlayerMantleState(this);

        SwitchState(Grounded);
    }

    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
    }

	void Update()
	{
        CurrentState.UpdateStates();
	}

	void FixedUpdate()
	{
        CurrentState.FixedUpdate();
	}

	public void MovePlayer(bool ignoreSlopes = false, float speedMultiplier = 1)
    {
        _inputDirection = _orientation.TransformDirection(GetInputDirection());
        Vector3 moveDirection = _inputDirection;

        if (!ignoreSlopes)
        {
            bool onSlope = false;

            Vector3 normal = Vector3.up;
            if (OnGround && Physics.Raycast(_transform.position, Vector3.down, out RaycastHit hit, 0.1f, _ground))
            {
                normal = hit.normal;

                float angle = Vector3.Angle(Vector3.up, normal);
                onSlope = angle <= _maxSlopeAngle && angle > 0f;

                _transform.position = hit.point;
            }

            _rigidbody.useGravity = onSlope;

            moveDirection = onSlope ? _inputDirection : Vector3.ProjectOnPlane(_inputDirection, normal);
        }

        float speed = speedMultiplier * _moveSpeed;
		_rigidbody.AddForce(speed * 10 * moveDirection, ForceMode.Force);

		Vector3 flatVel = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
		if (flatVel.sqrMagnitude > speed * speed)
		{
			Vector3 limitedVel = _moveSpeed * flatVel.normalized;
			_rigidbody.velocity = new(limitedVel.x, _rigidbody.velocity.y, limitedVel.z);
		}
	}

	public Vector3 GetInputDirection()
	{
		Vector2 dir = _actions.Movement.Movement.ReadValue<Vector2>();
		return new Vector3(dir.x, 0, dir.y);
	}

	public void ToggleInput(bool enabled)
	{
        if (!enabled)
        {
            _actions.Camera.Enable();
            _actions.ItemInteraction.Enable();
            _actions.Movement.Enable();
        }
        else
        {
            _actions.Camera.Disable();
            _actions.ItemInteraction.Disable();
            _actions.Movement.Disable();
        }
	}

    public void Die() => SwitchState(Dead);
}