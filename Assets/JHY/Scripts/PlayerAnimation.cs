using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private readonly int _hashX = Animator.StringToHash("VelocityX");
    private readonly int _hashY = Animator.StringToHash("VelocityY");
    private readonly int _hashIsMove = Animator.StringToHash("IsMove");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetAnimation(Vector2 moveDir)
    {
        bool isMove = moveDir != Vector2.zero;
        _animator.SetBool(_hashIsMove, isMove);
        if (!isMove) return;   

        Vector2 dir;
        if (Mathf.Abs(moveDir.x) >= Mathf.Abs(moveDir.y))
            dir = new Vector2(Mathf.Sign(moveDir.x), 0f);
        else
            dir = new Vector2(0f, Mathf.Sign(moveDir.y));

        _animator.SetFloat(_hashX, dir.x);
        _animator.SetFloat(_hashY, dir.y);

        _spriteRenderer.flipX = dir.x < 0f;
    }

}
