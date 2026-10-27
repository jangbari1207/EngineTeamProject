using UnityEngine;

public class InactiveState : EnemyState
{
    private float _timer;
    private float _waitTime;
    public InactiveState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Inactive");
        _timer = 0f;
        _waitTime = Random.Range(10f, 30f);
        _enemy.SetVisible(false);
    }
    public override void UpdateState()
    {
        base.UpdateState();
        _timer += Time.deltaTime;
        if ( _timer >= _waitTime)
        {
            if (_enemy.spawnPoints == null || _enemy.spawnPoints.Length == 0) return;
            _enemy.transform.position = PickSpawnPoint();
            _stateMachine.ChangeState(EnemyType.Wander);
        }
    }
    private Vector2 PickSpawnPoint()
    {
        var sp = _enemy.spawnPoints;
        int start = Random.Range(0, sp.Length);
        for (int i = 0; i < sp.Length; i++)
        {
            Transform p = sp[(start + i) % sp.Length];
            if (Vector2.Distance(p.position, _enemy.player.position) >= _enemy.minSpawnDistance)
                return p.position;
        }
        return sp[start].position;  
    }
    public override void Exit()
    {
        _enemy.SetVisible(true);
        base.Exit();
    }
}
