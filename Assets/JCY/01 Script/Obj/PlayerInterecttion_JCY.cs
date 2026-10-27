using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInterecttion_JCY : MonoBehaviour
{
    [SerializeField] private FadePanel_JCY _fade;
    [SerializeField] private float _diectRange = 1f;
    public LayerMask _whatIsObj;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , _diectRange);
    }

    public void Update()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, _diectRange, _whatIsObj);
        if (col == null)
        {
            return;
        }
        if (col.TryGetComponent(out Object_JCY obj) && col)
        {
            if(Keyboard.current.eKey.wasPressedThisFrame)
            {
                col = null;
                obj.Interaction(this , _fade);
            }
        }
    }
}
