using System.Collections.Generic;
using UnityEngine;
public enum EnemyType
{
    Inactive,
    Wander,
    Chase,
    Search,
    DoorFollow
}
public class EnemyStateMachine
{
    private Dictionary<EnemyType, EnemyState> stateDict = new();   

    public EnemyState CurrentState { get; private set; }    
    private AbstractEnemy _enemyBase; 

    public void Initialize(EnemyType startState, AbstractEnemy enemy)
    {
        _enemyBase = enemy;
        CurrentState = stateDict[startState];
        CurrentState.Enter();
    }

    public void AddState(EnemyType stateType, EnemyState state)
    {
        stateDict.Add(stateType, state);
    }

    public void ChangeState(EnemyType stateType)
    {
        if (CurrentState != null)
            CurrentState.Exit();
        CurrentState = stateDict[stateType];
        if (CurrentState != null)
            CurrentState.Enter();

    }
}
