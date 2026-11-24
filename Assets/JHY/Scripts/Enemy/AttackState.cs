using UnityEngine;

public class AttackState : EnemyState
{
    private InvisibleEnemy _invisible;
    private float _lostTimer;

    public AttackState(InvisibleEnemy enemy, EnemyStateMachine stateMachine, string animBoolName): base(enemy, stateMachine, animBoolName)
    {
        _invisible = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Attack");
        _lostTimer = 0f;
        _invisible.PlayFootstep(_invisible.attackPitch);
    }
    public override void UpdateState()
    {
        base.UpdateState();
        _invisible.ChaseTarget();
        _invisible.CheckCatch();

        // 한동안 감지하지 못하면 동선으로 돌아간다
        if (_invisible.Detects()) _lostTimer = 0f;
        else _lostTimer += Time.deltaTime;

        if (_lostTimer >= _invisible.loseTime)
            _stateMachine.ChangeState(EnemyType.Patrol);
    }
}
