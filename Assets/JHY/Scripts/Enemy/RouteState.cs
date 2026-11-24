using UnityEngine;

public class RouteState : EnemyState
{
    private InvisibleEnemy _invisible;
    public RouteState(InvisibleEnemy enemy, EnemyStateMachine stateMachine, string animBoolName): base(enemy, stateMachine, animBoolName)
    {
        _invisible = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        _invisible.PlayFootstep(_invisible.walkPitch);
    }
    public override void UpdateState()
    {
        base.UpdateState();

        if (_invisible.Detects())
        {
            _stateMachine.ChangeState(EnemyType.Attack);
            return;
        }
        _invisible.FollowRoute();
    }
}
