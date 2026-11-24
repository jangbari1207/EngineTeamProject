using UnityEngine;

public class RoomAppear : MonoBehaviour
{
    [Range(0, 100)] public float appearChance = 50f;   

    private FrenzyEnemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<FrenzyEnemy>();
    }

    private void OnEnable()
    {
        _enemy.tracker.RoomChanged += OnRoomChanged;
    }

    private void OnDisable()
    {
        _enemy.tracker.RoomChanged -= OnRoomChanged;
    }

    private void Start()
    {
        Enter(Room.Find(_enemy.tracker.PPosition));
    }

    private void OnRoomChanged(Room room, Vector2 exitPos, Vector2 oldEntryPos)
    {
        Enter(room);
    }

    private void Enter(Room room)
    {
        if (room == null || room.isSafeRoom || room.noEncount || Random.Range(0f, 100f) >= appearChance)
        {
            _enemy.Hide();
            return;
        }

        Vector2 pos = room.GetFarPosition(_enemy.tracker.PPosition);
        _enemy.Teleport(pos);
        _enemy.SetHome(pos);
        _enemy.stateMachine.ChangeState(EnemyType.Patrol);
    }
}
