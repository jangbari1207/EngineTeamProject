using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : AbstractEnemy
{
    public PlayerTracker tracker;
    public EnemyStateMachine stateMachine = new EnemyStateMachine();

    public bool IsChasing { get { return stateMachine.CurrentState is ChaseState; } }
    public bool WaitingAppear { get { return _waitingAppear; } }

    private Vector2 _appearPos;
    private float _appearDelay;
    private bool _waitingAppear;

    protected override void Awake()
    {
        base.Awake();
        stateMachine.AddState(EnemyType.Inactive, new InactiveState(this, stateMachine, "Inactive"));
        stateMachine.AddState(EnemyType.Chase, new ChaseState(this, stateMachine, "Chase"));

        stateMachine.Initialize(EnemyType.Inactive);
    }

    private void Update()
    {
        stateMachine.CurrentState.UpdateState();
    }

    // Director가 호출: delay초 뒤에 pos에서 나타나서 추적 시작
    public void Appear(Vector2 pos, float delay, bool addDelay = false)
    {
        _appearPos = pos;
        _appearDelay = addDelay ? Mathf.Max(_appearDelay, 0f) + delay : delay;
        _waitingAppear = true;
        stateMachine.ChangeState(EnemyType.Inactive);
    }

    // Director가 호출: 추적이 끝나서 사라짐
    public void Hide()
    {
        _waitingAppear = false;
        stateMachine.ChangeState(EnemyType.Inactive);
    }

    // InactiveState가 매 프레임 호출. 나타날 시간이 되면 순간이동하고 true
    public bool TickAppear(float dt)
    {
        if (!_waitingAppear) return false;

        _appearDelay -= dt;
        if (_appearDelay > 0f) return false;

        _waitingAppear = false;
        Teleport(_appearPos);
        return true;
    }

    // ChaseState가 매 프레임 호출
    public void ChaseTarget()
    {
        MoveTo(tracker.PPosition, chaseSpeed, tracker.Facing);
    }

    public override void AniEndTrigger()
    {
        base.AniEndTrigger();
        stateMachine.CurrentState.AnimationEndTrigger();
    }
}
