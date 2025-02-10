using UnityEngine;

[CreateAssetMenu(fileName = "OriginGuest", menuName = "Scriptable Objects/OriginGuest")]
public class OriginGuest : Guest
{
    
    public override void PerformAction()
    {
        // 기본 손님이므로, 특수 행동 없음
        Debug.Log("Say Hello");
    }

    public override bool ShouldPerformAction(string currentState)
    {
        return currentState == "OrderState";
    }
}
