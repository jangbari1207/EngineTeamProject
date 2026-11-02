using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : AbstractEnemy
{
    public EnemyStateMachine stateMachine = new EnemyStateMachine();
    [SerializeField] private FollowTrigger[] items;
    [Header("문 추적 거리")]
    [SerializeField] private float doorFollowRange = 17f;   

    private bool IsNearDoor(DoorTrigger door)
    {
        return Vector2.Distance(transform.position, door.transform.position) <= doorFollowRange;
    }
    protected override void Awake()
    {
        base.Awake();

        stateMachine.AddState(EnemyType.Inactive, new InactiveState(this, stateMachine, "Inactive"));
        stateMachine.AddState(EnemyType.Wander, new WanderState(this, stateMachine, "Wander"));
        stateMachine.AddState(EnemyType.Chase, new ChaseState(this, stateMachine, "Chase"));
        stateMachine.AddState(EnemyType.Search, new SearchState(this, stateMachine, "Search"));
        stateMachine.AddState(EnemyType.DoorFollow, new DoorFollowState(this, stateMachine, "Chase"));

        stateMachine.Initialize(EnemyType.Inactive, this);
    }

    private void Start()
    {
        DoorTrigger.PlayerPassed += OnPlayerPassedDoor;

        if (items == null) return;
        foreach (var item in items)
            item.itemTrigger += OnItemPicked;
    }

    private void OnDestroy()
    {
        DoorTrigger.PlayerPassed -= OnPlayerPassedDoor;

        if (items == null) return;
        foreach (var item in items)
            if (item != null) item.itemTrigger -= OnItemPicked;
    }

    private void OnPlayerPassedDoor(DoorTrigger door, Transform playerTrm)
    {
        var cur = stateMachine.CurrentState;
        bool near = IsNearDoor(door);
        if (cur is DoorFollowState follow)
        {
            if (!near) return;
            if (doorPlayer != null)           
            {
                pendingDoor = door;
                doorPlayer = playerTrm;
                follow.Restart();
            }
            else if (door.isDangerDoor)         
            {
                pendingDoor = door;
                follow.Restart();
            }
            return;
        }

        bool chasing = cur is ChaseState;
        bool justLost = cur is SearchState && Time.time - lastChaseTime <= doorFollowWindow;

        if (chasing || justLost)
        {
            if (!near) return;
            pendingDoor = door;
            doorPlayer = playerTrm;
            stateMachine.ChangeState(EnemyType.DoorFollow);
            return;
        }

        if (door.isDangerDoor)
        {
            pendingDoor = door;
            doorPlayer = null;
            stateMachine.ChangeState(EnemyType.DoorFollow);
        }
    }
    private void OnItemPicked(Vector2 itemPos)
    {
        var cur = stateMachine.CurrentState;
        if (cur is ChaseState || cur is DoorFollowState) return;

        if (cur is InactiveState)
        {
            if (spawnPoints == null || spawnPoints.Length == 0) return;
            Teleport(PickSpawnPoint());
        }

        lastSeenPos = itemPos;
        stateMachine.ChangeState(EnemyType.Search);
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
