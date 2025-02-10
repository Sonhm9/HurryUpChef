using UnityEngine;

// 기본 상태
public class IdleState : IGuestState
{
    GuestBehaviour guestBehaviour;
    GuestController guestController;
    public IdleState(GuestBehaviour guest, GuestController controller)
    {
        guestBehaviour = guest;
        guestController = controller;
    }

    public void Enter()
    {
        guestBehaviour.GetComponent<Animator>().SetTrigger("Idle");
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}
