using System;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Transform target_Tfm;
    [SerializeField] private float speed;

    private Rigidbody2D _rb;
    private Vector3 _dir;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        Player player = FindAnyObjectByType<Player>();
        target_Tfm = player.gameObject.transform;
    }

    private void Update()
    {
        _dir = target_Tfm.position - transform.position;
        _dir.Normalize();
        _rb.linearVelocity = _dir * speed;
    }
}
