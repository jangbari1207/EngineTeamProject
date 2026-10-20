using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class OraganRenderer_JCY : MonoBehaviour
{
    private readonly int deforHash = Animator.StringToHash("Deformation");
    public Animator AniCompo { get; private set; }
    public SpriteRenderer Renderer { get; private set; }

    private void Awake()
    {
        AniCompo = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void SetHash(float amount)
    {
        AniCompo.SetFloat(deforHash, amount);
    }
}


