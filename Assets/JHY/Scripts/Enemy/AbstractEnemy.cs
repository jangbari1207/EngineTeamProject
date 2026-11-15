using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public abstract class AbstractEnemy : MonoBehaviour
{
    [Header("참조")]
    public GameObject visual;
    public Tilemap wallMap;                 // 벽 타일맵 (타일이 있으면 못 지나감)
    [Header("속도")]
    public float chaseSpeed = 3.8f;

    public Animator AnimCompo { get; private set; }

    private Vector3Int _facing = Vector3Int.down;
    private TilePathfinder _pathfinder;
    private Vector3 _moveTarget;
    private bool _moving;
    private Collider2D _colliders;

    protected virtual void Awake()
    {
        _colliders = GetComponent<Collider2D>();
        AnimCompo = GetComponentInChildren<Animator>();
        _pathfinder = new TilePathfinder(wallMap, 256);
    }

    // 매 프레임 호출. 한 칸씩 걸어서 목적지로 간다.
    public void MoveTo(Vector2 dest, float speed, Vector3Int destFacing)
    {
        if (_moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, speed * Time.deltaTime);
            if (transform.position == _moveTarget) _moving = false;
            return;
        }

        Vector3Int myCell = wallMap.WorldToCell(transform.position);
        Vector3Int goalCell = wallMap.WorldToCell(dest);
        Vector3Int nextCell = _pathfinder.GetNextCell(myCell, goalCell, _facing, destFacing);

        if (nextCell == myCell) return;   // 이미 도착했거나 갈 길이 없음

        _facing = nextCell - myCell;
        _moveTarget = wallMap.GetCellCenterWorld(nextCell);
        _moving = true;
    }

    public int GetPathCost(Vector2 from, Vector2 to)
    {
        return _pathfinder.GetPathCost(wallMap.WorldToCell(from), wallMap.WorldToCell(to));
    }

    public void StopMove()
    {
        _moving = false;
    }

    public void Teleport(Vector2 pos)
    {
        Vector3Int cell = wallMap.WorldToCell(pos);
        transform.position = wallMap.GetCellCenterWorld(cell);
        _moving = false;
    }

    public void SetVisible(bool on)
    {
        if (visual != null) visual.SetActive(on);
        if (_colliders != null) _colliders.enabled = on;
    }

    public virtual void AniEndTrigger()
    {

    }
}
