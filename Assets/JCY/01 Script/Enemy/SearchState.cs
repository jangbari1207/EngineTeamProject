using UnityEngine;

public class SearchState : EnemyState
{
    private float _timer;        
    private float _waitTimer;    

    public SearchState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Search");

        _timer = _enemy.searchDuration;
        _waitTimer = 0f;
        _enemy.MoveTo(_enemy.lastSeenPos, _enemy.searchSpeed);   
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

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _stateMachine.ChangeState(EnemyType.Wander);
            return;
        }

        if (_waitTimer > 0f)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                Vector2 next = _enemy.lastSeenPos + Random.insideUnitCircle * _enemy.searchRadius;
                _enemy.MoveTo(next, _enemy.searchSpeed);
            }
            return;
        }

        if (_enemy.HasArrived())
        {
            _enemy.StopMove();
            _waitTimer = Random.Range(0.3f, 1f);
        }
    }

}
