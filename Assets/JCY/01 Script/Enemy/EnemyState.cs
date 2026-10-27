using UnityEngine;

public abstract class EnemyState
{
    protected AbstractEnemy _enemy;
    protected EnemyStateMachine _stateMachine;
    protected int _animBoolHash;
    protected bool _animationEndTrigger;
    public EnemyState(AbstractEnemy enemy,EnemyStateMachine stateMachine, string animBoolName)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animBoolName);
    }

    public virtual void Enter()
    {
        _enemy.AnimCompo.SetBool(_animBoolHash, true);
        _animationEndTrigger = false;
    }

    public virtual void UpdateState()
    {

    }
    public virtual void Exit()
    {
        _enemy.AnimCompo.SetBool(_animBoolHash, false);
    }

    public virtual void AnimationEndTrigger()
    {
        _animationEndTrigger = true;
    }
}
