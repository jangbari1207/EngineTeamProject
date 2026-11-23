using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private static List<Room> _allRooms = new List<Room>();

    [Header("방 설정")]
    public Transform[] appearPoints;
    public bool isSafeRoom;
    [Header("조우 (방에 들어올 때 굴림)")]
    [Range(0, 100)] public float encountChance = 10f;   
    public float chaseTime = 0f;                        
    public float spawnDelay = 3f;                       
    public bool noEncount;
    private Collider2D _area;
    private void Awake()
    {
        _area = GetComponent<Collider2D>();
        _area.isTrigger = true;
    }
    private void OnEnable()
    {
        _allRooms.Add(this);
    }
    private void OnDisable()
    {
        _allRooms.Remove(this);
    }
    public static Room Find(Vector2 pos)
    {
        foreach (Room room in _allRooms)
        {
            if (room._area.OverlapPoint(pos)) return room;
        }
        return null;
    }
    public Vector2 GetRandomPosition()
    {
        if (appearPoints == null || appearPoints.Length == 0)
            return transform.position;

        Transform p = appearPoints[Random.Range(0, appearPoints.Length)];
        return p != null ? (Vector2)p.position : (Vector2)transform.position;
    }
    private Transform FindPoint(Vector2 playerPos, bool nearest)
    {
        if (appearPoints == null) return null;

        Transform best = null;
        float bestDist = nearest ? float.MaxValue : -1f;
        foreach (Transform p in appearPoints)
        {
            if (p == null) continue;
            float d = Vector2.Distance(p.position, playerPos);
            if (nearest ? d < bestDist : d > bestDist)
            {
                bestDist = d;
                best = p;
            }
        }
        return best;
    }
    public Vector2 GetAppearPosition(Vector2 playerPos)
    {
        Transform p = FindPoint(playerPos, true);
        return p != null ? (Vector2)p.position : (Vector2)transform.position;
    }
}
