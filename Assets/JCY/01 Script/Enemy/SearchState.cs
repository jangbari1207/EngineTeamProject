using UnityEngine;

public class SearchState : EnemyState
{
    public SearchState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    private float _timer;        // 남은 수색 시간
    private Vector2 _dest;       // 지금 향하는 좌표
    private float _waitTimer;    // 도착 후 잠깐 멈춤

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Search");

        _timer = _enemy.searchDuration;
        _waitTimer = 0f;
        _dest = _enemy.lastSeenPos;   
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
            return;
        }

        _enemy.transform.position = Vector2.MoveTowards(
            _enemy.transform.position, _dest, _enemy.searchSpeed * Time.deltaTime);

        if (Vector2.Distance(_enemy.transform.position, _dest) <= 0.1f)
        {
            _waitTimer = Random.Range(0.3f, 1f);
            _dest = _enemy.lastSeenPos + Random.insideUnitCircle * _enemy.searchRadius;
        }
    }

}
