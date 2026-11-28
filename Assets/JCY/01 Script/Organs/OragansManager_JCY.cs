using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OragansManager_JCY : MonoBehaviour
{
    public Hart_JCY _hart;
    public Eye_JCY _eye;
    [SerializeField] private Leg_JCY _leg;

    private float _lastShiftTime = float.NegativeInfinity;
    private const float DoubleShiftWindow = 1f;

    private void Update()
    {
        if (Keyboard.current != null &&
            (Keyboard.current.leftShiftKey.wasPressedThisFrame || Keyboard.current.rightShiftKey.wasPressedThisFrame))
        {
            if (Time.time - _lastShiftTime <= DoubleShiftWindow)
            {
                if (_leg != null)
                    _leg.Deformation(1);
                _lastShiftTime = float.NegativeInfinity;
            }
            else
            {
                _lastShiftTime = Time.time;
            }
        }

        if (Keyboard.current == null)
            return;

        //테스트 하는 구문 , 빌드시 삭제
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            _hart.Deformation(1);
        }
        
        if (Keyboard.current.qKey.wasPressedThisFrame)
        { 
            _eye.Deformation(1);
        }
        
        if (Keyboard.current.eKey.wasPressedThisFrame)
        { 
            _eye.Deformation(-1);
        }
    }
}
