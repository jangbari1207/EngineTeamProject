using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Eye_JCY : IOrgansEffects_JCY
{
    private bool isEffect = false;
    private bool isStoryLightRadiusOverridden;
    [SerializeField] private Light2D light;
    [SerializeField] private int degree;
    [SerializeField] private int currentLight;

    [Header("라이트 수치 , 총 3개씩")]
    [SerializeField] private int[] damageValue;
    [SerializeField] private int[] healthValue;

    public override void Deformation(int amount)
    {
        if (amount > 0)
        {
            if(degree >= 3)
                return;
            
            if(isEffect)
                OragonDamage();
            isEffect = !isEffect;
        }
        else if(amount < 0)
        {
            if(degree <= 0)
                return;

            OragonHealth();
        }
    }

    private void Start()
    {
        light.pointLightOuterRadius = currentLight;
    }

    private void Update()
    {
        if (isStoryLightRadiusOverridden)
            return;

        if (light.pointLightOuterRadius > currentLight)
        {
            light.pointLightOuterRadius -= 0.1f;
        }
        
        if (light.pointLightOuterRadius < currentLight)
        {
            light.pointLightOuterRadius += 0.1f;
        }
    }

    public void OverrideStoryLightRadius(float radius)
    {
        if (light == null)
            return;

        isStoryLightRadiusOverridden = true;
        light.pointLightOuterRadius = Mathf.Max(0f, radius);
    }

    public void RestoreStoryLightRadius()
    {
        isStoryLightRadiusOverridden = false;
        if (light != null)
            light.pointLightOuterRadius = currentLight;
    }

    public override void OragonDamage()
    {
        degree++;
        switch (degree)
        {
            case 1:
                currentLight = damageValue[0];
                break;
            case 2:
                currentLight = damageValue[1];
                light.color = Color.white;
                break;
            case 3:
                currentLight = damageValue[2];
                light.color = Color.darkRed;
                break;
        }
    }
    
    public override void OragonHealth()
    {
        switch (degree)
        {
            case 1:
                currentLight = healthValue[0];
                break;
            case 2:
                light.color = Color.white;
                currentLight = healthValue[1];
                break;
            case 3:
                light.color = Color.white;
                currentLight = healthValue[2];
                break;
        }
        degree--;
        isEffect = false;
    }
}
