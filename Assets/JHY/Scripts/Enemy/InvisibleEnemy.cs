using System;
using UnityEngine;

public class InvisibleEnemy : Enemy
{
    [Header("투명 적")]
    public Transform[] routePoints;         // 정해진 동선 (순서대로 돌고 처음으로 돌아감)
    public float patrolSpeed = 2f;
    public float detectRange = 4f;          // 이 안에 들어오면 공격
    public float revealTime = 3f;           // 눈 변형 사용 시 드러나는 시간

    [Header("공격")]
    public float showTime = 0.5f;           // 공격할 때 모습이 보이는 시간
    public float attackCooldown = 1f;       // 공격 후 다시 감지하기까지의 시간
    public int damage = 1;

    [Header("식별 신호")]
    public AudioSource footstepSound;       // 움직일 때 나는 소리 (loop 체크, Spatial Blend 3D)
    public float walkPitch = 1f;

    public static event Action EyeUsed;
    public static event Action<int> PlayerHit;  // 플레이어 체력 코드에서 구독

    private SpriteRenderer[] _sprites;
    private int _routeIndex;
    private bool _revealed;                 // 눈 변형으로 드러난 상태
    private bool _attacking;                // 공격으로 드러난 상태
    private float _nextAttackTime;

    // 플레이어의 눈 변형 코드에서 호출
    public static void NotifyEyeUsed()
    {
        if (EyeUsed != null) EyeUsed();
    }

    private void OnEnable()
    {
        EyeUsed += Reveal;
    }

    private void OnDisable()
    {
        EyeUsed -= Reveal;
    }

    protected override void Awake()
    {
        base.Awake();
        stateMachine.AddState(EnemyType.Patrol, new RouteState(this, stateMachine, "Patrol"));
        stateMachine.AddState(EnemyType.Attack, new AttackState(this, stateMachine, "Attack"));

        _sprites = GetComponentsInChildren<SpriteRenderer>(true);   // 꺼져 있는 visual 안의 것도 포함
        RefreshVisible();

        Teleport(transform.position);       // 칸 중앙에 맞춤
        stateMachine.ChangeState(EnemyType.Patrol);
    }

    private void Reveal()
    {
        CancelInvoke(nameof(Vanish));
        _revealed = true;
        RefreshVisible();
        Invoke(nameof(Vanish), revealTime);
    }

    private void Vanish()
    {
        _revealed = false;
        RefreshVisible();
    }

    public void SetAttackVisible(bool on)
    {
        _attacking = on;
        RefreshVisible();
    }

    // 눈 변형이나 공격 중 하나라도 해당하면 보임
    private void RefreshVisible()
    {
        bool on = _revealed || _attacking;
        foreach (SpriteRenderer s in _sprites)
            s.enabled = on;
    }

    public bool Detects()
    {
        if (Time.time < _nextAttackTime) return false;
        return !tracker.IsSafe && CanSee(tracker.PPosition, detectRange);
    }

    public void TryHit()
    {
        Debug.Log("공격 명중! (투명 적)");
        PlayerHit?.Invoke(damage);
    }

    public void StartCooldown()
    {
        _nextAttackTime = Time.time + attackCooldown;
    }
    public void FollowRoute()
    {
        if (routePoints.Length == 0) return;

        Vector2 dest = routePoints[_routeIndex].position;

        if (HasArrived(dest))
            _routeIndex = (_routeIndex + 1) % routePoints.Length;
        else
            MoveTo(dest, patrolSpeed, Vector3Int.down);
    }
    public void PlayFootstep(float pitch)
    {
        if (footstepSound == null) return;

        footstepSound.pitch = pitch;
        if (!footstepSound.isPlaying) footstepSound.Play();
    }
    public void StopFootstep()
    {
        if (footstepSound != null) footstepSound.Stop();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
