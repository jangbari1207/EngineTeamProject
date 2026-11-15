using UnityEngine;

public class ChaseTimer : MonoBehaviour
{
    public float defaultTime = 9f;      // 방에 chaseTime이 없을 때 쓰는 기본 시간
    public float timePerRoom = 1f;      // 방 이동마다 추가되는 시간

    public float Remaining { get; private set; }
    public bool IsOver { get { return Remaining <= 0f; } }

    public void Begin(float roomChaseTime)
    {
        Remaining = roomChaseTime > 0f ? roomChaseTime : defaultTime;
    }

    public void Tick()
    {
        Remaining = Mathf.Max(0f, Remaining - Time.deltaTime);
    }

    public void AddRoomBonus()
    {
        Remaining += timePerRoom;
    }

    public void Stop()
    {
        Remaining = 0f;
    }
}
