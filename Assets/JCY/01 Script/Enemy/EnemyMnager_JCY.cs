using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMnager_JCY : MonoBehaviour
{
    [SerializeField] private int _currentEnemyNum;
    [SerializeField] private GameObject[] _enemyTfm;
    [SerializeField] private float enemyMoveTime;
    [SerializeField] private float enemyDestTime;
    
    public static EnemyMnager_JCY Instance { get; private set; }
    public bool _isChase;
    public Action OnChase;
    public Action OffChase;

    private GameObject currentEnemy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        EnemySpawn();
    }

    public void EnemySpawn()
    {
        _currentEnemyNum = Random.Range(0, _enemyTfm.Length - 1);
        currentEnemy = _enemyTfm[_currentEnemyNum];
        currentEnemy = Instantiate(currentEnemy , transform.position , Quaternion.identity);
        _isChase = true;
        StartCoroutine(EnemyDestTimer());
        OnChase?.Invoke();
        //이벤트에 뭐 배경음 출력 등 하지 안을까???
    }

    public void EnemyDespawn()
    {
        _isChase = false;
        Destroy(currentEnemy);
        currentEnemy = null;
        OffChase?.Invoke();
    }
    
    public void PlayerMove(Transform doorTfm)
    {
        StartCoroutine(Enemymove(doorTfm));
    }

    private IEnumerator Enemymove(Transform transform)
    {
        yield return new WaitForSeconds(enemyMoveTime);
        currentEnemy.transform.position = transform.position;
    }
    
    private IEnumerator EnemyDestTimer()
    {
        yield return new WaitForSeconds(enemyDestTime);
        EnemyDespawn();
    }
}
