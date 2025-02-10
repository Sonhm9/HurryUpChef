using UnityEngine;
[CreateAssetMenu(fileName = "TutorialGuest", menuName = "Scriptable Objects/TutorialGuest")]
public class TutorialGuest : Guest
{
    public override void PerformAction()
    {
        // 기본 손님이므로, 특수 행동 없음
        Debug.Log("Tutorial");
    }

    public override bool ShouldPerformAction(string currentState)
    {
        return currentState == "OrderState";
    }
}
