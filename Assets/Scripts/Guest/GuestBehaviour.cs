using UnityEngine;

public class GuestBehaviour : MonoBehaviour
{
    public Guest guest;

    GuestStateMachine stateMachine;
    GuestController guestController;

    void Awake()
    {
        guestController = GetComponent<GuestController>();
        stateMachine = new GuestStateMachine(this, guestController);
        stateMachine.stateChanged += OnStateChanged; // 상태 변화 이벤트 구독
    }

    void Start()
    {
        stateMachine.Initialize(stateMachine.idleState); // 시작 시 기본상태 진입
    }

    void Update()
    {
        stateMachine.Execute(); // 프레임마다 상태 실행
    }

    // 상태가 변화했을 때
    private void OnStateChanged(IGuestState newState)
    {
        if (guest.ShouldPerformAction(stateMachine.CurrentState.ToString()))
        {
            guest?.PerformAction(); // 특수 행동 실행
        }
    }

    // 기본 상태 선택
    public void OnIdle()
    {
        if (stateMachine.CurrentState != stateMachine.idleState)
        {
            stateMachine.Transition(stateMachine.idleState);
        }
    }

    // 이동 상태 선택
    public void OnMove()
    {
        if (stateMachine.CurrentState != stateMachine.moveState)
        {
            stateMachine.Transition(stateMachine.moveState);
        }
    }

    // 주문하기 상태 선택
    public void OnOrder()
    {
        if (stateMachine.CurrentState != stateMachine.orderState)
        {
            stateMachine.Transition(stateMachine.orderState);
        }
    }

    // 대기 상태 선택
    public void OnWating()
    {
        if (stateMachine.CurrentState != stateMachine.watingState)
        {
            stateMachine.Transition(stateMachine.watingState);
        }
    }

    // 먹기 상태 선택
    public void OnEating()
    {
        if (stateMachine.CurrentState != stateMachine.eatingState)
        {
            stateMachine.Transition(stateMachine.eatingState);
        }
    }
}
