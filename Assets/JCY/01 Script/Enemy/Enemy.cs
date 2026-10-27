using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : AbstractEnemy
{
    public EnemyStateMachine stateMachine = new EnemyStateMachine();
    private ItemTrigger item;

    protected override void Awake()
    {
        base.Awake();


        stateMachine.AddState(EnemyType.Inactive, new InactiveState(this, stateMachine, "Inactive"));
        stateMachine.AddState(EnemyType.Wander, new WanderState(this, stateMachine, "Wander"));
        stateMachine.AddState(EnemyType.Chase, new ChaseState(this, stateMachine, "Chase"));
        stateMachine.AddState(EnemyType.Search, new SearchState(this, stateMachine, "Search"));


        stateMachine.Initialize(EnemyType.Inactive, this);
    }

    private void Start()
    {
        //스폰 이벤트
        item.itemTrigger += () => stateMachine.ChangeState(EnemyType.Wander);
    }

    private void Update()
    {
        stateMachine.CurrentState.UpdateState();
    }

    public override void AniEndTrigger()
    {
        base.AniEndTrigger();
        stateMachine.CurrentState.AnimationEndTrigger();
    }
}
