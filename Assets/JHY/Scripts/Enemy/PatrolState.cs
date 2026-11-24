using UnityEngine;

public class PatrolState : EnemyState
{
    private FrenzyEnemy _frenzy;

    public PatrolState(FrenzyEnemy enemy, EnemyStateMachine stateMachine, string animBoolName): base(enemy, stateMachine, animBoolName)
    {
        _frenzy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        _frenzy.StopMove();
    }
    public override void UpdateState()
    {
        base.UpdateState();

        // 발견하면 바로 돌진
        if (_frenzy.SeesPlayer())
        {
            _stateMachine.ChangeState(EnemyType.Charge);
            return;
        }
        _frenzy.Patrol();
    }
}
