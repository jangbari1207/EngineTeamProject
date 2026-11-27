using UnityEngine;

public class AttackState : EnemyState
{
    private InvisibleEnemy _invisible;
    private float _timer;

    public AttackState(InvisibleEnemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        _invisible = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        _timer = 0f;

        _invisible.StopMove();
        _invisible.StopFootstep();
        _invisible.SetAttackVisible(true);   // 바로 모습 드러냄
        _invisible.TryHit();                 // 바로 타격
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _timer += Time.deltaTime;

        if (_timer >= _invisible.showTime)
        {
            _invisible.SetAttackVisible(false);
            _invisible.StartCooldown();
            _stateMachine.ChangeState(EnemyType.Patrol);
        }
    }
}
