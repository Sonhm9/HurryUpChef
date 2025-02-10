using UnityEngine;

[CreateAssetMenu(fileName = "DirtyGuest", menuName = "Scriptable Objects/DirtyGuest")]
public class DirtyGuest : Guest
{
    // 해당 손님의 특수 행동
    public override void PerformAction()
    {
        // 바닥 얼룩 생성
        GameManager.Instance.SpawnStain();
    }

    // 특수 행동 실행시기
    public override bool ShouldPerformAction(string currentState)
    {
        return currentState == "OrderState"; // 주문 상태
    }
}
