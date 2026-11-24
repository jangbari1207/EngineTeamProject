using UnityEngine;

public class RoomEnemy : MonoBehaviour
{
    public Room room;                       

    private Enemy _enemy;
    private Vector2 _startPos;
    private bool _active;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _startPos = transform.position;
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
        if (Room.Find(_enemy.tracker.PPosition) == room) Activate();
        else Deactivate();
    }

    private void OnRoomChanged(Room newRoom, Vector2 exitPos, Vector2 oldEntryPos)
    {
        if (newRoom == room) Activate();
        else Deactivate();
    }

    private void Activate()
    {
        if (_active) return;

        _active = true;
        _enemy.Teleport(_startPos);                        
        _enemy.stateMachine.ChangeState(EnemyType.Patrol);
    }

    private void Deactivate()
    {
        if (!_active && _enemy.stateMachine.CurrentState is InactiveState) return;

        _active = false;
        _enemy.Hide();                                    
    }
}
