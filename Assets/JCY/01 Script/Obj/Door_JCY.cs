using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_JCY : MonoBehaviour , Object_JCY
{
    [SerializeField] private Door_JCY _endDoor;
    public Transform _doorTfm;
    public static float _time;

    public void Interaction(PlayerInterecttion_JCY player, FadePanel_JCY fadePanelJcy = null)
    {
        if (Time.time < _time)
        {
            return;
        }

        if (fadePanelJcy != null)
        {
            fadePanelJcy.Fade();
        }
        
        player.transform.position = _endDoor._doorTfm.position;
        _time = Time.time + 3f;
        Debug.Log("쿨타임:" + _time);
        Debug.Log("현 시간:" + Time.time);
        //문 열리는 사운드 출력
        //이동
        if (EnemyMnager_JCY.Instance._isChase)
        {
            EnemyMnager_JCY.Instance.PlayerMove(_endDoor._doorTfm);
        }
    }
}
