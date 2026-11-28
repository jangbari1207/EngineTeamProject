using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInterecttion_JCY : MonoBehaviour
{
    [SerializeField] private FadePanel_JCY _fade;
    [SerializeField] private float _diectRange = 1f;
    [SerializeField] private Key interactKey = Key.E;
    [SerializeField] private Key descriptionKey = Key.F;
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
        
        if (Keyboard.current == null) return;

        if (Keyboard.current[interactKey].wasPressedThisFrame && col.TryGetComponent(out IObject_JCY obj))
        {
            obj.Interaction(this, _fade);
        }

        if (Keyboard.current[descriptionKey].wasPressedThisFrame && col.TryGetComponent(out IObjectDescription_JCY description))
            description.ShowDescription();
    }
}
