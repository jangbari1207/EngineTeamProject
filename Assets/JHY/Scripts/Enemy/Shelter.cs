using UnityEngine;
using UnityEngine.InputSystem;

public class Shelter : MonoBehaviour
{
    public PlayerTracker tracker;
    public EnemyDirector director;
    public GameObject playerVisual;         // 플레이어 그림 (숨으면 꺼짐)
    public MonoBehaviour playerMove;
    public Collider2D playerCollider;  // 플레이어 이동 스크립트 (숨으면 꺼짐)
    public float useDistance = 1.2f;        // 이 거리 안에서만 숨을 수 있다

    private bool _hiding;

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (_hiding)
                Exit();
            else if (Vector2.Distance(transform.position, tracker.PPosition) <= useDistance)
                Enter();
        }
    }

    private void Enter()
    {
        _hiding = true;
        playerVisual.SetActive(false);
        playerMove.enabled = false;
        playerCollider.enabled = false;
        director.OnPlayerHide();        // 일반 적의 추적이 끝나고, 적이 플레이어를 못 본다
    }

    private void Exit()
    {
        _hiding = false;
        playerVisual.SetActive(true);
        playerMove.enabled = true;
        playerCollider.enabled = true;
        director.OnPlayerUnhide();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, useDistance);
        Gizmos.color = Color.red;
    }
}
