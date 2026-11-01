using UnityEngine;

public class InactiveState : EnemyState
{
    private float _timer;
    private float _waitTime;

    public InactiveState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Inactive");
        _timer = 0f;
        _waitTime = Random.Range(10f, 30f);
        _enemy.StopMove();
        _enemy.SetVisible(false);
    }
    public override void UpdateState()
    {
        base.UpdateState();
        _timer += Time.deltaTime;
        if (_timer >= _waitTime)
        {
            if (_enemy.spawnPoints == null || _enemy.spawnPoints.Length == 0) return;
            _enemy.Teleport(_enemy.PickSpawnPoint());
            _stateMachine.ChangeState(EnemyType.Wander);
        }
    }
    public override void Exit()
    {
        _enemy.SetVisible(true);
        base.Exit();
    }
}
