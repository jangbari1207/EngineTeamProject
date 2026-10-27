using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Chase");
        _enemy._lostTimer = 0;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_enemy.targetTrm != null &&
         Vector2.Distance(_enemy.transform.position, _enemy.targetTrm.position) <= _enemy.catchDistance)
        {
            Debug.Log("잡힘!");   
            return;
        }
        Collider2D player = _enemy.Detect();
        if (player != null)
        {
            _enemy.targetTrm = player.transform;
            _enemy.lastSeenPos = player.transform.position;
            _enemy._lostTimer = 0f;
        }
        else
        {
            _enemy._lostTimer += Time.deltaTime;
            if (_enemy._lostTimer >= _enemy.loseTime)
            {
                _stateMachine.ChangeState(EnemyType.Search);
                return;
            }
        }

        if (_enemy.targetTrm != null)
        {
            _enemy.transform.position = Vector2.MoveTowards(
                _enemy.transform.position,
                _enemy.lastSeenPos,
                _enemy.chaseSpeed * Time.deltaTime);
        }
    }
    public override void Exit()
    {
        base.Exit();
        _enemy.targetTrm = null;
    }
}
