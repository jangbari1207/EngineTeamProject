using UnityEngine;

public class Leg_JCY : IOrgansEffects_JCY
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private int degree;

    [Header("Damage speed boost")]
    [SerializeField, Min(1f)] private float damageBoostMultiplier = 1.5f;
    [SerializeField, Min(0f)] private float fullBoostDuration = 5f;
    [SerializeField, Min(0f)] private float fadeDuration = 5f;

    private float _damageBoostElapsed;
    private bool _damageBoostActive;

    public bool IsDamageBoostActive => _damageBoostActive;

    private void Start()
    {
        if (movement == null)
            movement = FindFirstObjectByType<PlayerMovement>();
        ApplySpeedMultiplier();
    }

    private void Update()
    {
        if (!_damageBoostActive)
            return;

        _damageBoostElapsed += Time.deltaTime;
        if (_damageBoostElapsed >= fullBoostDuration + fadeDuration)
        {
            _damageBoostActive = false;
            ApplySpeedMultiplier();
            return;
        }

        float boostMultiplier = _damageBoostElapsed <= fullBoostDuration
            ? damageBoostMultiplier
            : Mathf.Lerp(1.1f, 1f,
                ( _damageBoostElapsed - fullBoostDuration) / Mathf.Max(fadeDuration, 0.001f));
        movement?.SetOrganSpeedMultiplier(boostMultiplier);
    }
    
    public override void Deformation(int amount)
    {
        if (amount > 0)
        {
            // 속도 효과가 진행되는 동안에는 Shift로 추가 변형을 발동하지 않는다.
            if (_damageBoostActive)
                return;

            if(degree >= 3)
                return;

            OragonDamage();
        }
        else if(amount < 0)
        {
            if(degree <= 0)
                return;

            OragonHealth();
        }
    }

    public override void OragonDamage()
    {
        if (degree >= 3)
            return;

        degree++;
        _damageBoostElapsed = 0f;
        _damageBoostActive = true;
        Debug.Log("Leg 손상 효과 발동: 이동속도 1.5배", this);
        ApplySpeedMultiplier();
    }

    public override void OragonHealth()
    {
        if (degree <= 0)
            return;

        degree--;
        ApplySpeedMultiplier();
    }

    private float GetDeformationMultiplier()
    {
        switch (degree)
        {
            case 1: return 0.9f;
            case 2: return 0.75f;
            case 3: return 0.5f;
            default: return 1f;
        }
    }

    private void ApplySpeedMultiplier()
    {
        if (movement == null)
            return;

        float multiplier = _damageBoostActive
            ? (_damageBoostElapsed <= fullBoostDuration
                ? damageBoostMultiplier
                : Mathf.Lerp(1.1f, 1f,
                    Mathf.Clamp01((_damageBoostElapsed - fullBoostDuration) / Mathf.Max(fadeDuration, 0.001f))))
            : GetDeformationMultiplier();
        movement.SetOrganSpeedMultiplier(multiplier);

        if (!_damageBoostActive && degree > 0)
            Debug.Log($"Leg 변형도 {degree}: 이동속도 {multiplier:0.##}배로 감소", this);
    }
}
