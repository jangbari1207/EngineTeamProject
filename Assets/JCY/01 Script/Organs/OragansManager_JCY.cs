using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OragansManager_JCY : MonoBehaviour
{
    public Hart_JCY _hart;

    private void Update()
    {
        //테스트 하는 구문 , 빌드시 삭제
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            _hart.Deformation(1);
        }
    }
}
