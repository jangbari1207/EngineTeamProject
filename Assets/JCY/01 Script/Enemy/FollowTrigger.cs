using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowTrigger : MonoBehaviour
{
    public event Action<Vector2> itemTrigger;

    [SerializeField] private bool once = true;

    private bool _used;
    private Transform _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        _player = collision.transform;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == _player)
            _player = null;
    }
    private void Update()
    {
        if (_player == null) return;
        if (once && _used) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            _used = true;
            itemTrigger?.Invoke(transform.position);   
        }
    }
}
