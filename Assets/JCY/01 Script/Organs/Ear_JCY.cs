using UnityEngine;

public class Ear_JCY : OrgansEffects_JCY
{
    public int currentDeformation; 
    [SerializeField , Min(1)] private int maxDeformation;
    
    public override void Deformation(int amount)
    {
        throw new System.NotImplementedException();
    }
}
