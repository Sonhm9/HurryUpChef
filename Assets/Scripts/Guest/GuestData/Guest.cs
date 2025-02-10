using UnityEngine;

public abstract class Guest : ScriptableObject
{
    public string guestName; // 손님 이름

    public abstract bool ShouldPerformAction(string currentState); // 특수 행동을 할 상태 
    public float eatingPatienceTime; // 음식 기다리는 시간
    public float orderingPatienceTime; // 주문 기다리는 시간
    public GameObject[] prefabs; // 손님 프리팹

    public abstract void PerformAction(); // 손님 별 특수행동
}
