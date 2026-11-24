using UnityEngine;

public class InactiveState : EnemyState
{
    public InactiveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName): base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Inactive");
        _enemy.StopMove();
        _enemy.SetVisible(false);
    }
    public override void UpdateState()
    {
        base.UpdateState();

        // 나타날 시간이 되면 true
        if (_enemy.TickAppear(Time.deltaTime))
            _stateMachine.ChangeState(EnemyType.Chase);
    }
    public override void Exit()
    {
        _enemy.SetVisible(true);
        base.Exit();
    }
}
