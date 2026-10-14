using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    private readonly int _hashX = Animator.StringToHash("VelocityX");
    private readonly int _hashY = Animator.StringToHash("VelocityY");

    private readonly int _hashIsMove = Animator.StringToHash("IsMove");
    public void SetAnimation(Vector2 moveDir)
    {
        bool isMove = moveDir != Vector2.zero;
        _animator.SetBool(_hashIsMove, isMove);
        if (moveDir != Vector2.zero)
        {
            _animator.SetFloat(_hashX, moveDir.x);
            _animator.SetFloat(_hashY, moveDir.y);
        }
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

}
