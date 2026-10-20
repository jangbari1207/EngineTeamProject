using System;
using UnityEngine;

[RequireComponent(typeof(OraganRenderer_JCY))]
public abstract class IOrgansEffects_JCY : MonoBehaviour
{
    public OraganRenderer_JCY RenderCompo { get; private set; }
    public abstract void Deformation(int amount);

    public virtual void Awake()
    {
        RenderCompo = GetComponentInChildren<OraganRenderer_JCY>();
    }
}
