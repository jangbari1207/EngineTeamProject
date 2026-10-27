using System;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    public event Action itemTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            itemTrigger?.Invoke();
        }
    }
}
