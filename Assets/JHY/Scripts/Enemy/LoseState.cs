using UnityEngine;

public class LoseState : EnemyState
{
    private float _timer;

    public LoseState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Lose");
        _timer = 0f;
    }
    public override void UpdateState()
    {
        base.UpdateState();

        if (!_enemy.HasArrived(_enemy.LostPos))
            _enemy.MoveTo(_enemy.LostPos, _enemy.chaseSpeed, Vector3Int.down);

        _timer += Time.deltaTime;
        if (_timer >= _enemy.loseTime) _enemy.GiveUp();
    }
}
