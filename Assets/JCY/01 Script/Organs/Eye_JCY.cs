using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Debug = UnityEngine.Debug;

public class Eye_JCY : OrgansEffects_JCY
{
    [Header("라이트 관련")]
    [SerializeField] private Light2D _VisualLight;
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private int _currentLight;

    [Header("변형도 관련")]
    [SerializeField] private int currentDeformation; 
    [SerializeField , Min(1)] private int maxDeformation = 3;

    //눈 능력써야 보이는 물체 부모에 몰아넣은 후 이 변수에 부모넣기
    [SerializeField] private GameObject obj;
    [SerializeField] private bool variable = false;
    
    public bool on = true;
    public override void Deformation(int amount)
    {
        if ((currentDeformation >= maxDeformation && amount > 0) || (currentDeformation <= 0 && amount < 0))
        {
            return;
        }
        
        currentDeformation += amount;
        Debug.Log("현재 눈 변형도:" + currentDeformation);
        RenderCompo.SetHash(currentDeformation);
    }

    private void Start()
    {
        _VisualLight.pointLightOuterRadius = _currentLight;
        obj.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (!on)
            {
                return;
            }
            StartCoroutine(Skill());
        }
        
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            variable = false;
            FadeHealth();
            Deformation(-1);
        }

        if (_VisualLight.pointLightOuterRadius > _currentLight)
        {
            _VisualLight.pointLightOuterRadius -= 0.1f;
        }
        
        if (_VisualLight.pointLightOuterRadius < _currentLight)
        {
            _VisualLight.pointLightOuterRadius += 0.1f;
        }
    }

    private IEnumerator Skill()
    {
    
        on = false;

        obj.SetActive(true);
        if (variable)
        {
            Deformation(1);
            Fade();
        }
        yield return new WaitForSeconds(3f);
        on = true;
        variable = !variable;
        obj.SetActive(false);
    }

    private void Fade()
    { 
        switch (currentDeformation)
        {
                case 1:
                    _currentLight -= 10;
                    break;
                case 2:
                    _currentLight -= 15;
                    BeckChange(Color.white);
                    break;
                case 3:
                    BeckChange(Color.darkRed);
                    _currentLight -= 17;
                    break;
                default:
                    break;
        }
    }
    
    private void FadeHealth()
    { 
        switch (currentDeformation)
        {
            case 1:
                _currentLight += 10;
                break;
            case 2:
                _currentLight += 15;
                BeckChange(Color.white);
                break;
            case 3:
                BeckChange(Color.white);
                _currentLight += 17;
                break;
            default:
                break;
        }
    }

    public void BeckChange(Color color)
    {
        _VisualLight.color = color;
    }
}
