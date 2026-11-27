using Unity.Cinemachine;
using UnityEngine;

public class RoomBounds : MonoBehaviour
{
    [SerializeField] private GameObject roomCamera;   // 이 방의 시네머신 카메라

    private void Awake()
    {
        roomCamera.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            roomCamera.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            roomCamera.SetActive(false);
    }
}
