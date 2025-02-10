using UnityEngine;

// 주문 후 대기 상태
public class WatingState : IGuestState
{
    GuestBehaviour guestBehaviour;
    GuestController guestController;
    float patienceTime;
    public WatingState(GuestBehaviour guest, GuestController controller)
    {
        guestBehaviour = guest;
        guestController = controller;
    }

    public void Enter()
    {
        patienceTime = guestBehaviour.guest.eatingPatienceTime;
        guestBehaviour.GetComponent<Animator>().SetTrigger("Sit");
    }

    public void Execute()
    {
        patienceTime -= Time.deltaTime;
        if(patienceTime <= 0)
        {
            // 분노 수치 증가
            HallSystemManager.Instance.RisingAngry();
            EffectSpawner.Instance.SpawnAngryEffect(guestController.transform.position + new Vector3(0, 3, 0));

            // 퇴장
            guestController.ExitTable();
        }
    }

    public void Exit()
    {
    }
}
