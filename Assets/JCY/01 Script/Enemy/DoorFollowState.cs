using UnityEngine;

public class DoorFollowState : EnemyState
{
    private const float ChaseFollowDelay = 1f;  
    private const float warningDelayMin = 3f;     
    private const float warningDelayMax = 5f;

    private float _timer;
    private float _waitTime;

    public DoorFollowState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("DoorFollow");
        _timer = 0f;

        var door = _enemy.pendingDoor;
        bool isChaseFollow = _enemy.doorPlayer != null;

        if (door == null) _waitTime = 0f;
        else _waitTime = isChaseFollow ? door.chaseFollowDelay : door.GetDangerDelay();

        if (!isChaseFollow) _enemy.SetVisible(false);
    }
    public override void UpdateState()
    {
        base.UpdateState();

        var door = _enemy.pendingDoor;
        if (door == null)
        {
            _stateMachine.ChangeState(EnemyType.Search);
            return;
        }

        _timer += Time.deltaTime;
        if (_timer < _waitTime) return;

        var appear = door.enemyAppearPoint != null ? door.enemyAppearPoint : door.transform;
        _enemy.transform.position = appear.position;
        _enemy.pendingDoor = null;

        if (_enemy.doorPlayer != null)
        {
            _enemy.lastSeenPos = _enemy.doorPlayer.position;
            _enemy.targetTrm = _enemy.doorPlayer;
            _enemy.doorPlayer = null;
            _stateMachine.ChangeState(EnemyType.Chase);
        }
        else
        {
            _enemy.lastSeenPos = _enemy.player.position;
            _stateMachine.ChangeState(EnemyType.Search);
        }
    }
    public override void Exit()
    {
        _enemy.SetVisible(true);
        base.Exit();
    }
}
