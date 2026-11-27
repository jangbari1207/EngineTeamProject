using System.Collections.Generic;
using UnityEngine;
public enum EnemyType
{
    Inactive,
    Chase,
    Patrol,       
    Charge,
    Search,
    Attack,
    Lose
}
public class EnemyStateMachine
{
    private Dictionary<EnemyType, EnemyState> stateDict = new();
    public EnemyState CurrentState { get; private set; }
    public void Initialize(EnemyType startState)
    {
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
