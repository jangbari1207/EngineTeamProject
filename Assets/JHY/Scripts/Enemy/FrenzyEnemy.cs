using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FrenzyEnemy : Enemy
{
    [Header("순찰")]
    public Transform[] patrolPoints;        // 비워두면 제자리에서 대기
    public float patrolSpeed = 2f;
    [Header("발견")]
    public float sightRange = 8f;
    [Header("돌진")]
    public float startSpeed = 3.5f;
    public float maxSpeed = 6f;             // 직선 최고 속도 (플레이어보다 빠르게)
    public float acceleration = 5f;         // 직선에서 초당 빨라지는 정도
    public float turnSlow = 0.5f;           // 코너를 돌 때 속도가 이 비율로 줄어듦
    public float catchDistance = 0.8f;
    public float searchTime = 2.5f;         // 마지막 위치에서 두리번거리는 시간
    [Header("식별 신호")]
    public AudioSource screamSound;         // 발견 비명
    public AudioSource footstepSound;       // 발소리 (loop 체크)

    private float _speed;
    private Vector3Int _lastFacing;
    private Vector2 _lastSeenPos;
    private Vector2 _home;
    private int _patrolIndex;
    private bool _caught;

    protected override void Awake()
    {
        base.Awake();
        stateMachine.AddState(EnemyType.Patrol, new PatrolState(this, stateMachine, "Patrol"));
        stateMachine.AddState(EnemyType.Charge, new ChargeState(this, stateMachine, "Charge"));
        stateMachine.AddState(EnemyType.Search, new SearchState(this, stateMachine, "Search"));

        Teleport(transform.position);       // 칸 중앙에 맞춤
        _home = transform.position;
        stateMachine.ChangeState(EnemyType.Patrol);
    }

    // 나타난 자리를 대기 위치로 삼는다 (patrolPoints가 비어 있을 때 돌아오는 곳)
    public void SetHome(Vector2 pos)
    {
        _home = pos;
    }

    public bool SeesPlayer()
    {
        return !tracker.IsHidden && CanSee(tracker.PPosition, sightRange);
    }

    // PatrolState가 매 프레임 호출
    public void Patrol()
    {
        Vector2 dest = _home;
        if (patrolPoints.Length > 0) dest = patrolPoints[_patrolIndex].position;

        if (HasArrived(dest))
            _patrolIndex = (_patrolIndex + 1) % Mathf.Max(patrolPoints.Length, 1);
        else
            MoveTo(dest, patrolSpeed, Vector3Int.down);
    }

    // 발견한 순간 한 번
    public void StartCharge()
    {
        _speed = startSpeed;
        _lastFacing = Vector3Int.zero;
        _lastSeenPos = tracker.PPosition;

        if (screamSound != null) screamSound.Play();
        if (footstepSound != null) footstepSound.Play();
    }

    // ChargeState가 매 프레임 호출
    public void ChargeTarget()
    {
        // 보이는 동안만 위치를 갱신 → 시야가 끊기면 마지막으로 본 곳까지 달림
        if (SeesPlayer()) _lastSeenPos = tracker.PPosition;

        _speed = Mathf.MoveTowards(_speed, maxSpeed, acceleration * Time.deltaTime);
        MoveTo(_lastSeenPos, _speed, tracker.Facing);

        // 방향이 바뀌면(코너) 속도가 줄어든다. 첫 방향은 제외
        if (Facing != _lastFacing)
        {
            if (_lastFacing != Vector3Int.zero) _speed *= turnSlow;
            _lastFacing = Facing;
        }

        // 빠를수록 발소리도 빨라진다
        if (footstepSound != null) footstepSound.pitch = Mathf.Lerp(0.9f, 1.6f, _speed / maxSpeed);
    }

    // 시야가 끊긴 채 마지막으로 본 곳에 도착했는가
    public bool LostPlayer()
    {
        return !SeesPlayer() && HasArrived(_lastSeenPos);
    }

    public void StopFootstep()
    {
        if (footstepSound != null) footstepSound.Stop();
    }

    public void CheckCatch()
    {
        if (_caught || tracker.IsHidden) return;

        if (Vector2.Distance(transform.position, tracker.PPosition) < catchDistance)
        {
            _caught = true;
            Debug.Log("잡힘! (광란형)");
            StopMove();
            EnemyDirector.NotifyCaught();
        }
    }
}
