using UnityEngine;

// 먹기 상태
public class EatingState : IGuestState
{
    GuestBehaviour guestBehaviour;
    GuestController guestController;
    float eatingTime; // 먹는 시간
    public EatingState(GuestBehaviour guest, GuestController controller)
    {
        guestBehaviour = guest;
        guestController = controller;
    }

    public void Enter()
    {
        eatingTime = 2f;
        guestBehaviour.GetComponent<Animator>().SetTrigger("Eat");
    }

    public void Execute()
    {
        eatingTime -= Time.deltaTime;
        if (eatingTime <= 0)
        {
            // 식사완료
            guestController.isFoodEating = true;
        }
    }

    public void Exit()
    {
    }
}
