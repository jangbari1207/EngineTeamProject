using UnityEngine;

public class SearchState : EnemyState
{
    private FrenzyEnemy _frenzy;
    private float _timer;

    public SearchState(FrenzyEnemy enemy, EnemyStateMachine stateMachine, string animBoolName): base(enemy, stateMachine, animBoolName)
    {
        _frenzy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Search");
        _timer = 0f;
        _frenzy.StopMove();
    }
    public override void UpdateState()
    {
        base.UpdateState();

        if (_frenzy.SeesPlayer())
        {
            _stateMachine.ChangeState(EnemyType.Charge);
            return;
        }

        // 못 찾으면 순찰로 돌아간다
        _timer += Time.deltaTime;
        if (_timer >= _frenzy.searchTime)
            _stateMachine.ChangeState(EnemyType.Patrol);
    }
}
