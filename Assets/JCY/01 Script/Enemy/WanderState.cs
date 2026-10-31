using UnityEngine;

public class WanderState : EnemyState
{
    private int currentIndex = 0;
    private Vector2 _dest;

    public WanderState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Wander");
        _enemy.waitTimer = 0f;
        _enemy._noDetectTimer = 0f;
        if (_enemy.wanderPoints == null || _enemy.wanderPoints.Length == 0) return;

        float best = float.MaxValue;
        for (int i = 0; i < _enemy.wanderPoints.Length; i++)
        {
            float d = Vector2.Distance(_enemy.transform.position, _enemy.wanderPoints[i].position);
            if (d < best) { best = d; currentIndex = i; }
        }
        SetDestination();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Collider2D player = _enemy.Detect();
        if (player != null)
        {
            _enemy.targetTrm = player.transform;
            _enemy.lastSeenPos = player.transform.position;
            _stateMachine.ChangeState(EnemyType.Chase);
            return;
        }

        _enemy._noDetectTimer += Time.deltaTime;
        if (_enemy._noDetectTimer >= 20f)
        {
            _stateMachine.ChangeState(EnemyType.Inactive);
            return;
        }

        if (_enemy.wanderPoints == null || _enemy.wanderPoints.Length == 0) return;

        if (_enemy.waitTimer > 0f)
        {
            _enemy.waitTimer -= Time.deltaTime;
            if (_enemy.waitTimer <= 0f) SetDestination();    
            return;
        }

        if (_enemy.HasArrived())
        {
            _enemy.StopMove();
            _enemy.waitTimer = Random.Range(0.5f, 1.5f);
            currentIndex = PickNextIndex();                 
        }
    }
    private int PickNextIndex()
    {
        int count = _enemy.wanderPoints.Length;
        if (count == 1) return 0;

        int next;
        do { next = Random.Range(0, count); }
        while (next == currentIndex);
        return next;
    }
    private void SetDestination()
    {
        _dest = (Vector2)_enemy.wanderPoints[currentIndex].position
                + Random.insideUnitCircle * _enemy.wanderJitter;
        _enemy.MoveTo(_dest, _enemy.wanderSpeed);
    }

}
