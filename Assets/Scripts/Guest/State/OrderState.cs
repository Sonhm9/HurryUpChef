using UnityEngine;

// 주문하기 상태
public class OrderState : IGuestState
{
    GuestBehaviour guestBehaviour;
    GuestController guestController;
    float patienceTime;

    public OrderState(GuestBehaviour guest, GuestController controller)
    {
        guestBehaviour = guest;
        guestController = controller;
    }

    public void Enter()
    {
        patienceTime = guestBehaviour.guest.orderingPatienceTime;
        guestBehaviour.GetComponent<Animator>().SetTrigger("Idle");
    }

    public void Execute()
    {

        patienceTime -= Time.deltaTime;
        if (patienceTime <= 0)
        {
            // 분노 수치 증가
            HallSystemManager.Instance.RisingAngry();
            EffectSpawner.Instance.SpawnAngryEffect(guestController.transform.position + new Vector3(0, 3, 0));

            // 주문 대기열에서 퇴장
            guestController.LeaveQueue();
            guestController.currentState = GuestController.GuestState.Leaving;
            guestBehaviour.OnMove();
        }
    }

    public void Exit()
    {
    }
}
