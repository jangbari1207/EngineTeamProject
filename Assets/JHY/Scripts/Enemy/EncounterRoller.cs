using UnityEngine;

public class EncounterRoller : MonoBehaviour
{
    public float voltagePerRoom = 3f;   // 방 이동마다 쌓이는 확률(%)
    public int cooldownSteps = 50;      // 추적 끝난 뒤 이 걸음 수 동안은 안 나옴

    public float Voltage { get; private set; }

    private int _lastChaseEndSteps;

    private void Awake()
    {
        _lastChaseEndSteps = -cooldownSteps;
    }

    // 추적 중이 아닐 때 방을 옮길 때마다 호출
    public void AddVoltage()
    {
        Voltage = Mathf.Min(100f, Voltage + voltagePerRoom);
    }

    // true면 이번 방에서 적이 나온다
    public bool Roll(Room room, int steps)
    {
        if (room.noEncount || room.isSafeRoom) return false;
        if (steps - _lastChaseEndSteps < cooldownSteps) return false;
        if (Random.Range(0f, 100f) >= room.encountChance + Voltage) return false;

        Voltage = 0f;
        return true;
    }

    // 추적이 끝났을 때 호출 (쿨다운 시작)
    public void MarkChaseEnded(int steps)
    {
        _lastChaseEndSteps = steps;
    }
}
