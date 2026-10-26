using UnityEngine;

public class Door_JCY : MonoBehaviour , Object_JCY
{
    [SerializeField] private Door_JCY _endDoor;
    public Transform _doorTfm;
    
    public void Interaction(PlayerInterecttion_JCY player)
    {
        player.transform.position = _endDoor._doorTfm.position;
        //문 열리는 사운드 출력
    }
}
