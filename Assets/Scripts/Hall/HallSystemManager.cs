using System.Collections.Generic;
using UnityEngine;

public class HallSystemManager : MonoBehaviour
{
    #region Singleton
    static HallSystemManager instance;
    static public HallSystemManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion
    private void Awake()
    {
        // 인스턴스가 존재한다면
        if (instance != null)
        {
            Destroy(gameObject);
        }

        // 인스턴스가 처음 생성된다면
        else
        {
            instance = this;

            //DontDestroyOnLoad(gameObject);
        }
    }
    public List<GameObject> emptySeats; // 비어있는 좌석 리스트
    public List<GameObject> fullSeats; // 비어있는 좌석 리스트

    public List<GuestController> guestQueue; // 손님 대기열

    public Vector3 orderPosition; // 주문 포지션
    public float spacing = 1.5f; // 대기열 간격
    public bool orderDirection;

    public int angry = 0; // 분노 수치

    bool[] isEmptys; // 비어있는지 여부 리스트
    bool[] isDirtys; // 더러운지 여부 리스트

    // 비어있는 위치를 찾는 메서드
    public void GetSeatObject(out GameObject seat)
    {
        int num = Random.Range(0, emptySeats.Count);
        seat = emptySeats[num];

        fullSeats.Add(seat);
        emptySeats.Remove(seat);
    }

    // 분노 상승 메서드
    public void RisingAngry()
    {
        angry++;
    }

    // 대기열의 위치를 반환하는 메서드
    public Vector3 GetQueuePosition(int index)
    {
        if (orderDirection)
        {
            Vector3 targetPosition = orderPosition - new Vector3(spacing * index, 0, 0);

            return targetPosition;

        }
        else
        {
            Vector3 targetPosition = orderPosition - new Vector3(-spacing * index, 0, 0);

            return targetPosition;
        }
    }

    // 대기열의 방향을 반환하는 메서드
    public Vector3 GetQueueRotation()
    {
        if (orderDirection)
        {
            return Vector3.right;
        }
        else
        {
            return Vector3.left;
        }
    }

    // 손님의 주문을 반환하는 메서드
    public RecipeData GetTakeOrder()
    {
        // 손님이 없을 때 null 반환
        if(guestQueue.Count == 0)
        {
            return null;
        }

        // 자리가 없을 때 주문받지 않음
        if(emptySeats.Count == 0)
        {
            return null;
        }

        // 손님이 주문중 상태일 때
        if (guestQueue[0].currentState == GuestController.GuestState.Ordering)
        {
            // 주문을 받고 레시피 데이터 반환
            guestQueue[0].isOrderClear = true;

            return guestQueue[0].recipeData;
        }
        // 손님이 주문 중 상태가 아닐 때 null 반환
        else
        {
            return null;
        }
    }
}
