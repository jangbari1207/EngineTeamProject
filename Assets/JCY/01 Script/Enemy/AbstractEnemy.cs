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

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        AnimCompo = GetComponentInChildren<Animator>();
    }
    public Collider2D Detect()
    {
        return Physics2D.OverlapCircle(transform.position, detectRange, whatIsPlayer);
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
