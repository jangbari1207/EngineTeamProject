using System;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    [Header("참조")]
    public Enemy enemy;
    public PlayerTracker tracker;
    public EncounterRoller roller;
    public ChaseTimer timer;

    [Header("잡힘")]
    public float catchDistance = 0.8f;

    public static event Action PlayerCaught;
    public bool IsChasing { get; private set; }
    private bool _caught;

    private void OnEnable()
    {
        tracker.RoomChanged += OnRoomChanged;
    }

    private void OnDisable()
    {
        tracker.RoomChanged -= OnRoomChanged;
    }

    private void Update()
    {
        if (!IsChasing) return;

        timer.Tick();

        if (timer.IsOver && !enemy.IsChasing)
        {
            EndChase();
            return;
        }

        CheckCatch();
    }

    private void CheckCatch()
    {
        if (_caught || !enemy.IsChasing) return;

        if (Vector2.Distance(enemy.transform.position, tracker.Position) < catchDistance)
        {
            _caught = true;
            Debug.Log("잡힘!");
            enemy.StopMove();
            if (PlayerCaught != null) PlayerCaught();
        }
    }

    private void OnRoomChanged(Room room, Vector2 exitPos, Vector2 oldEntryPos)
    {
        Debug.Log("방 이동: " + room.name + " / 추적중=" + IsChasing + " / 남은시간=" + timer.Remaining);

        if (!IsChasing)
        {
            roller.AddVoltage();
            if (roller.Roll(room, tracker.Steps)) StartChase(room);
            return;
        }

        if (room.isSafeRoom || timer.IsOver)
        {
            EndChase();
            return;
        }

        timer.AddRoomBonus();
        FollowToRoom(room, exitPos, oldEntryPos);
    }

    private void StartChase(Room room)
    {
        IsChasing = true;
        _caught = false;
        timer.Begin(room.chaseTime);
        enemy.Appear(room.GetRandomPosition(tracker.Position), room.spawnDelay);
    }

    // 추적 중 플레이어가 방을 옮김 → 적이 사라졌다가 지연 후 새 방에 나타남
    private void FollowToRoom(Room room, Vector2 exitPos, Vector2 oldEntryPos)
    {
        bool waiting = enemy.WaitingAppear;
        Vector2 from = waiting ? oldEntryPos : (Vector2)enemy.transform.position;
        int cost = enemy.GetPathCost(from, exitPos);
        float delay = Mathf.Max(cost, 1) / enemy.chaseSpeed;
        Vector2 appearPos = room.GetAppearPosition(tracker.Position);

        Debug.Log("따라오기 지연: 칸=" + cost + " / 시간=" + delay + " / 누적=" + waiting);
        enemy.Appear(appearPos, delay, waiting);
    }

    public void EndChase()
    {
        Debug.Log("추적 종료! 남은시간=" + timer.Remaining);
        IsChasing = false;
        timer.Stop();
        roller.MarkChaseEnded(tracker.Steps);
        enemy.Hide();
    }

    // 옷장 등에 숨었을 때 호출
    public void OnPlayerHide()
    {
        if (IsChasing) EndChase();
    }
}
