using UnityEngine;

public class Hart_JCY : IOrgansEffects_JCY
{
    public int currentDeformation; 
    [SerializeField , Min(1)] private int maxDeformation;

    public override void Awake()
    {
        base.Awake();
        currentDeformation = 0;
    }

    public override void OragonDamage()
    {
        
    }

    public override void OragonHealth()
    {
        
    }

    public override void Deformation(int amount)
    {
        currentDeformation += 1;
        Debug.Log("심장 변형");
        Debug.Log("현재 심장 변형도:" + currentDeformation);
        RenderCompo.SetHash(currentDeformation);
        if (currentDeformation >= maxDeformation)
        {
            RenderCompo.Renderer.color = Color.black;
            return;
        }
        if (currentDeformation > maxDeformation / 2f)
        {
            RenderCompo.Renderer.color = Color.red;
        }
    }
}
