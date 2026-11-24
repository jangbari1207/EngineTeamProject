using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName): base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Chase");
    }
    public override void UpdateState()
    {
        base.UpdateState();
        _enemy.ChaseTarget();
    }
}
