using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D RbCompo { get; private set; }
    [SerializeField] private float _speed = 5f;

    [Header("Dash stamina")]
    [SerializeField, Min(0f)] private float maxStamina = 100f;
    [SerializeField, Min(0f)] private float staminaDrainPerSecond = 10f;
    [SerializeField, Min(0f)] private float staminaRecoveryPerSecond = 7.5f;
    [SerializeField, Min(0f)] private float dashSpeedMultiplier = 1.2f;
    [SerializeField] private Leg_JCY leg;

    public float CurrentStamina { get; private set; }
    public float MaxStamina => maxStamina;
    public bool IsDashing { get; private set; }
    public float BaseSpeed => _speed;

    private Vector2 _moveDir;
    private float _organSpeedMultiplier = 1f;

    private void Awake()
    {
        RbCompo = GetComponent<Rigidbody2D>();
        CurrentStamina = maxStamina;
        if (leg == null)
            leg = FindFirstObjectByType<Leg_JCY>();
    }

    private void Update()
    {
        bool shiftHeld = Keyboard.current != null &&
                         (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed);
        IsDashing = shiftHeld && CurrentStamina > 0f;

        if (IsDashing)
            CurrentStamina = Mathf.Max(0f, CurrentStamina - staminaDrainPerSecond * Time.deltaTime);
        else
            CurrentStamina = Mathf.Min(maxStamina, CurrentStamina + staminaRecoveryPerSecond * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float dashMultiplier = IsDashing ? dashSpeedMultiplier : 1f;
        RbCompo.linearVelocity = _moveDir * _speed * _organSpeedMultiplier * dashMultiplier;
    }

    public void SetOrganSpeedMultiplier(float multiplier)
    {
        _organSpeedMultiplier = Mathf.Max(0f, multiplier);
    }

    public void SetDir(Vector2 inputVector)
    {
        _moveDir = inputVector;
    }
}
