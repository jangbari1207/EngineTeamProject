using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Leg_JCY : OrgansEffects_JCY
{
    public int currentDeformation; 
    [SerializeField , Min(1)] private int maxDeformation;
    [SerializeField] private Player _player;
    [SerializeField] private bool isBuff;
    [SerializeField] private float currentSpeed;
    
    [SerializeField] private bool variable = false;
    
    
    private void Start()
    { 
        currentSpeed  = _player.MoveCompo._speed;
    }

    public override void Deformation(int amount)
    {
        if ((currentDeformation >= maxDeformation && amount > 0) || (currentDeformation <= 0 && amount < 0))
        {
            return;
        }
        
        currentDeformation += amount;
        Debug.Log("현재 다리 변형도:" + currentDeformation);
        RenderCompo.SetHash(currentDeformation);
        Slow();
    }

    private void Update()
    {
        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            if (isBuff)
            {
                return;
            }
            StartCoroutine(Skill());
        }
    }
    
    private IEnumerator Skill()
    {
        isBuff = true;
        float speed = _player.MoveCompo._speed;
        _player.MoveCompo._speed = currentSpeed * 1.2f;
        yield return new WaitForSeconds(3f);
        _player.MoveCompo._speed = speed;
        if (variable)
        {
            Deformation(1);
        }
        isBuff = false;
        variable = !variable;
    }
    
    private void Slow()
    { 
        switch (currentDeformation)
        {
            case 1:
                _player.MoveCompo._speed = currentSpeed * 0.9f;
                break;
            case 2:
                _player.MoveCompo._speed = currentSpeed * 0.75f;
                break;
            case 3:
                _player.MoveCompo._speed = currentSpeed * 0.5f;
                break;
            default:
                break;
        }
    }
    
    private void SlowHealth()
    { 
        switch (currentDeformation)
        {
            case 1:
                _player.MoveCompo._speed = currentSpeed * 1f;
                break;
            case 2:
                _player.MoveCompo._speed = currentSpeed * 0.9f;
                break;
            case 3:
                _player.MoveCompo._speed = currentSpeed * 0.75f;
                break;
            default:
                break;
        }
    }
}
