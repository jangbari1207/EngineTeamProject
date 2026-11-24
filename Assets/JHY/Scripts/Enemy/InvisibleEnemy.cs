using System;
using UnityEngine;

public class InvisibleEnemy : Enemy
{
    [Header("투명 적")]
    public Transform[] routePoints;         // 정해진 동선 (순서대로 돌고 처음으로 돌아감)
    public float patrolSpeed = 2f;
    public float detectRange = 4f;          // 이 안에 들어오면 공격
    public float loseTime = 2f;             // 이 시간 동안 감지 못 하면 동선으로 복귀
    public float catchDistance = 0.8f;
    public float revealTime = 3f;           // 눈 변형 사용 시 드러나는 시간
    [Header("식별 신호")]
    public AudioSource footstepSound;       // 움직일 때 나는 소리 (loop 체크, Spatial Blend 3D)
    public float walkPitch = 1f;
    public float attackPitch = 1.5f;        // 공격 중엔 더 빠른 소리

    public static event Action EyeUsed;

    private SpriteRenderer[] _sprites;
    private int _routeIndex;
    private bool _caught;

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
        SetSeen(false);

        Teleport(transform.position);       // 칸 중앙에 맞춤
        stateMachine.ChangeState(EnemyType.Patrol);
    }

    private void Reveal()
    {
        CancelInvoke(nameof(Vanish));
        SetSeen(true);
        Invoke(nameof(Vanish), revealTime);
    }

    private void Vanish()
    {
        SetSeen(false);
    }

    private void SetSeen(bool on)
    {
        foreach (SpriteRenderer s in _sprites)
            s.enabled = on;
    }

    public bool Detects()
    {
        return !tracker.IsHidden && CanSee(tracker.PPosition, detectRange);
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

    public void CheckCatch()
    {
        if (_caught || tracker.IsHidden) return;

        if (Vector2.Distance(transform.position, tracker.PPosition) < catchDistance)
        {
            _caught = true;
            Debug.Log("잡힘! (투명 적)");
            StopMove();
            EnemyDirector.NotifyCaught();
        }
    }
}
