using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTracker : MonoBehaviour
{
    public Transform player;
    public Tilemap grid;                       // 벽 타일맵 (칸 계산용)

    public Vector2 PPosition { get { return player.position; } }
    public Vector3Int Facing { get; private set; } = Vector3Int.down;
    public int Steps { get; private set; }

    public event Action<Room, Vector2, Vector2> RoomChanged;
    private Vector3Int _lastCell;
    private Room _currentRoom;
    private Vector2 _lastRoomPos;     // 현재 방에서 마지막으로 서 있던 곳
    private Vector2 _roomEntryPos;    // 현재 방에 들어온 자리

    private void Start()
    {
        _lastCell = grid.WorldToCell(player.position);
        _lastRoomPos = PPosition;
        _roomEntryPos = PPosition;
        _currentRoom = Room.Find(PPosition);
    }

    private void Update()
    {
        CountSteps();
        CheckRoomChange();
    }
    private void CountSteps()
    {
        Vector3Int cell = grid.WorldToCell(player.position);
        if (cell == _lastCell) return;

        Vector3Int d = cell - _lastCell;
        if (Mathf.Abs(d.x) >= Mathf.Abs(d.y))
            Facing = new Vector3Int((int)Mathf.Sign(d.x), 0, 0);
        else
            Facing = new Vector3Int(0, (int)Mathf.Sign(d.y), 0);

        _lastCell = cell;
        Steps++;
    }
    private void CheckRoomChange()
    {
        Room room = Room.Find(PPosition);
        if (room == null) return;

        if (room == _currentRoom)
        {
            _lastRoomPos = PPosition;
            return;
        }

        Vector2 exitPos = _lastRoomPos;
        Vector2 oldEntryPos = _roomEntryPos;
        _currentRoom = room;
        _lastRoomPos = PPosition;
        _roomEntryPos = PPosition;

        if (RoomChanged != null) RoomChanged(room, exitPos, oldEntryPos);
    }
}
