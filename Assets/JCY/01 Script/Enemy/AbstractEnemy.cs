using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractEnemy : MonoBehaviour
{
    [Header("참조")]
    public Transform player;
    public GameObject visual;
    public Transform[] wanderPoints;
    public Transform[] spawnPoints;
    [Header("속도")]
    public float wanderSpeed = 1.5f;
    public float chaseSpeed = 2.8f;
    public float searchSpeed = 2.0f;
    [Header("감지 / 추적")]
    public float detectRange = 5f;
    public LayerMask whatIsPlayer;
    public Transform targetTrm;
    public float loseTime = 1.5f;
    public float _lostTimer;
    public float _noDetectTimer;
    public float catchDistance = 0.5f;
    public float repathInterval = 0.3f;
    public float waitTime = 1f;
    public float waitTimer = 0f;
    [Header("수색")]
    public float searchRadius = 5f;
    public float searchDuration = 10f;
    public Vector2 lastSeenPos;
    public Animator AnimCompo { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public float minSpawnDistance = 8f;
    public float wanderJitter = 0.5f;
    private Camera _cam;
    [Header("문 추적")]
    public DoorTrigger pendingDoor;
    public Transform doorPlayer;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;  
        agent.updateUpAxis = false;    
        AnimCompo = GetComponentInChildren<Animator>();
        _cam = Camera.main;
    }
    public Collider2D Detect()
    {
        return Physics2D.OverlapCircle(transform.position, detectRange, whatIsPlayer);
    }
    public void MoveTo(Vector2 dest, float speed)
    {
        if (!agent.isOnNavMesh) return;

        NavMeshHit hit;                                        // 목적지를 NavMesh 위로 보정
        if (NavMesh.SamplePosition(dest, out hit, 2f, NavMesh.AllAreas))
            dest = hit.position;

        agent.isStopped = false;
        agent.speed = speed;
        agent.SetDestination(dest);
    }
    public void StopMove()
    {
        if (!agent.isOnNavMesh) return;
        agent.isStopped = true;
    }
    public bool HasArrived()
    {
        if (!agent.isOnNavMesh) return false;
        return !agent.pathPending && agent.remainingDistance <= 0.2f;
    }
    public void Teleport(Vector2 pos)
    {
        agent.Warp(pos);
    }

    public Vector2 PickSpawnPoint()
    {
        List<Transform> outCamera = new List<Transform>();
        foreach (Transform p in spawnPoints)
        {
            if (!IsInCameraView(p.position))
                outCamera.Add(p);
        }

        if (outCamera.Count > 0)
            return outCamera[Random.Range(0, outCamera.Count)].position;

        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }
    public bool IsInCameraView(Vector2 pos, float dis = 1f)
    {
        if (_cam == null) _cam = Camera.main;
        if (_cam == null) return false;

        float halfHeight = _cam.orthographicSize;
        float halfWidth = halfHeight * _cam.aspect;

        Vector2 camPos = _cam.transform.position;

        float dx = Mathf.Abs(pos.x - camPos.x);
        float dy = Mathf.Abs(pos.y - camPos.y);

        return dx < halfWidth + dis && dy < halfHeight + dis;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
    public void SetVisible(bool on)
    {
        visual.SetActive(on);
    }
    public virtual void AniEndTrigger()
    {

    }
}
