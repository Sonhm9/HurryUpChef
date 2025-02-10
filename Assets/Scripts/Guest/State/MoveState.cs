using UnityEngine;

// 이동 상태
public class MoveState : IGuestState
{
    GuestBehaviour guestBehaviour;
    GuestController guestController;

    public MoveState(GuestBehaviour guest, GuestController controller)
    {
        guestBehaviour = guest;
        guestController = controller;
    }

    public void Enter()
    {
        guestBehaviour.GetComponent<Animator>().SetTrigger("Move");
    }

    public void Execute()
    {

    }

    public void Exit()
    {
    }
}
