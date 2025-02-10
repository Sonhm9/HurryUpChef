using UnityEngine;

[CreateAssetMenu(fileName = "ImpatientGuest", menuName = "Scriptable Objects/ImpatientGuest")]
public class ImpatientGuest : Guest
{
    // 해당 손님의 특수 행동
    public override void PerformAction()
    {
        // 바닥 얼룩 생성
        Debug.Log("Impatient");
    }

    // 특수 행동 실행시기
    public override bool ShouldPerformAction(string currentState)
    {
        return currentState == "EatingState"; // 먹는 상태
    }
}
