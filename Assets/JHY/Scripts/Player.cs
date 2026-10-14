using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInput InputCompo { get; private set; }
    public PlayerMovement MoveCompo { get; private set; }
    public PlayerAnimation AnimCompo { get; private set; }

    private void Awake()
    {
        InputCompo = GetComponent<PlayerInput>();
        MoveCompo = GetComponent<PlayerMovement>();

        AnimCompo = GetComponentInChildren<PlayerAnimation>();
    }

    private void Update()
    {
        Vector2 moveDir = InputCompo.MoveDir;
        AnimCompo.SetAnimation(moveDir);
        MoveCompo.SetDir(moveDir);
    }
}
