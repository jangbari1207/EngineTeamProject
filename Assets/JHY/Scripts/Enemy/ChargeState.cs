using UnityEngine;

public class ChargeState : EnemyState
{
    private FrenzyEnemy _frenzy;

    public ChargeState(FrenzyEnemy enemy, EnemyStateMachine stateMachine, string animBoolName): base(enemy, stateMachine, animBoolName)
    {
        _frenzy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Charge");
        _frenzy.StartCharge();
    }
    public override void UpdateState()
    {
        base.UpdateState();
        _frenzy.ChargeTarget();
        _frenzy.CheckCatch();

        // 시야가 끊긴 채 마지막으로 본 곳에 도착하면 두리번거린다
        if (_frenzy.LostPlayer())
            _stateMachine.ChangeState(EnemyType.Search);
    }
    public override void Exit()
    {
        _frenzy.StopFootstep();
        base.Exit();
    }
}
