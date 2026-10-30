using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorTrigger : MonoBehaviour
{
    public static event Action<DoorTrigger, Transform> PlayerPassed;

    [Header("오니 등장")]
    public Transform enemyAppearPoint;                

    [Header("추적 중에 문을 썼을 때")]
    public float chaseFollowDelay = 1f;               

    [Header("위험 구역 문")]
    public bool isDangerDoor;                       
    public Vector2 dangerFollowDelay = new Vector2(3f, 5f);

    [SerializeField] private float cooldown = 0.5f;

    private Transform _player;
    private float _lastTime = -999f;

    public float GetDangerDelay()
    {
        return UnityEngine.Random.Range(dangerFollowDelay.x, dangerFollowDelay.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            _player = collision.transform;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == _player)
            _player = null;
    }
    private void Update()
    {
        if (_player == null) return;
        if (Keyboard.current == null || !Keyboard.current.eKey.wasPressedThisFrame) return;
        if (Time.time - _lastTime < cooldown) return;

        _lastTime = Time.time;
        PlayerPassed?.Invoke(this, _player);
    }
}
