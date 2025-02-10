using System;
using UnityEngine;

public class GuestStateMachine
{
    public IGuestState CurrentState { get; private set; } // 현재 상태

    // 상태 목록
    public IdleState idleState;
    public MoveState moveState;
    public WatingState watingState;
    public OrderState orderState;
    public EatingState eatingState;

    public event Action<IGuestState> stateChanged; // 상태 변화에 대한 이벤트

    GuestBehaviour guestBehaviour;
    GuestController guestController;

    public GuestStateMachine(GuestBehaviour guest, GuestController controller)
    {
        guestBehaviour = guest;
        guestController = controller;

        idleState = new IdleState(guest, controller);
        moveState = new MoveState(guest, controller);
        watingState = new WatingState(guest, controller);
        orderState = new OrderState(guest, controller);
        eatingState = new EatingState(guest, controller);
    }

    // 상태 진입
    public void Initialize(IGuestState state)
    {
        CurrentState = state;
        state.Enter();

        stateChanged?.Invoke(state);
    }

    // 상태 전환
    public void Transition(IGuestState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();

        stateChanged?.Invoke(CurrentState);

    }

    // 상태 실행
    public void Execute()
    {
        CurrentState.Execute();
    }
}
