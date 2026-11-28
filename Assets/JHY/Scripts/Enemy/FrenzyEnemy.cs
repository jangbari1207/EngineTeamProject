using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FrenzyEnemy : Enemy
{
    [Header("순찰")]
    public Transform[] patrolPoints;        // 비워두면 제자리에서 대기
    public float patrolSpeed = 2f;
    [Header("발견")]
    public float sightRange = 8f;
    public float loseRange = 7f;    
    [Header("돌진")]
    private Vector3Int _burstDir;
    public float burstSpeed = 8f;      // 발견 직후 확 튀어나가는 속도
    public float burstTime = 0.4f;     // 그 속도를 유지하는 시간
    public float minSpeed = 2.5f;      // 코너에서 느려져도 이 아래로는 안 내려감

    private float _burstTimer;
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
    public void SetPatrolPoints(Transform[] points)
    {
        patrolPoints = points != null ? points : new Transform[0];
        _patrolIndex = 0;
    }
    public bool SeesPlayer()
    {
        return !tracker.IsSafe && CanSee(tracker.PPosition, sightRange);
    }
    private bool KeepsSight()
    {
        return !tracker.IsSafe && CanSee(tracker.PPosition, loseRange);
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
        Teleport(transform.position);   

        Vector2 d = tracker.PPosition - (Vector2)transform.position;
        if (Mathf.Abs(d.x) >= Mathf.Abs(d.y))
            _burstDir = d.x >= 0 ? Vector3Int.right : Vector3Int.left;
        else
            _burstDir = d.y >= 0 ? Vector3Int.up : Vector3Int.down;

        _speed = startSpeed;
        _burstTimer = burstTime;
        _lastFacing = _burstDir;
        _lastSeenPos = tracker.PPosition;

        if (screamSound != null) screamSound.Play();
        if (footstepSound != null) footstepSound.Play();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;      // 발견 범위
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;   // 놓치는 범위
        Gizmos.DrawWireSphere(transform.position, loseRange);
    }
    // ChargeState가 매 프레임 호출
    public void ChargeTarget()
    {
        if (KeepsSight()) _lastSeenPos = tracker.PPosition;

        // 처음 돌진: 정해진 방향으로만
        if (_burstTimer > 0f)
        {
            _burstTimer -= Time.deltaTime;
            if (!MoveStraight(_burstDir, burstSpeed)) _burstTimer = 0f;   // 벽이면 돌진 끝
            if (footstepSound != null) footstepSound.pitch = 1.6f;
            return;
        }

        _speed = Mathf.MoveTowards(_speed, maxSpeed, acceleration * Time.deltaTime);
        MoveTo(_lastSeenPos, _speed, tracker.Facing);

        if (Facing != _lastFacing)
        {
            _speed = Mathf.Max(minSpeed, _speed * turnSlow);
            _lastFacing = Facing;
        }

        if (footstepSound != null) footstepSound.pitch = Mathf.Lerp(0.9f, 1.6f, _speed / maxSpeed);
    }

    // 시야가 끊긴 채 마지막으로 본 곳에 도착했는가
    public bool LostPlayer()
    {
        return _burstTimer <= 0f && !KeepsSight() && HasArrived(_lastSeenPos);
    }

    public void StopFootstep()
    {
        if (footstepSound != null) footstepSound.Stop();
    }

    public void CheckCatch()
    {
        if (_caught || tracker.IsSafe) return;

        if (Vector2.Distance(transform.position, tracker.PPosition) < catchDistance)
        {
            _caught = true;
            Debug.Log("잡힘! (광란형)");
            StopMove();
            EnemyDirector.NotifyCaught();
        }
    }
}
