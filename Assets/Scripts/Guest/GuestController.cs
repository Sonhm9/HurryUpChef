using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GuestController : MonoBehaviour
{
    public GameObject uiCanvas;
    public Vector3 initialPosition; // 입장 및 퇴장 위치
    public Vector3 seatPosition;

    public RecipeData recipeData;

    public bool isOrderClear; // 주문이 받아졌는지
    public bool isFoodEating; // 음식을 먹었는지

    float rotationSpeed = 2f; // 회전 속도

    [HideInInspector]
    public GameObject currentTable; // 현재 앉은 테이블
    Plate myPlate;

    [HideInInspector]
    public GuestBehaviour guestBehaviour;
    NavMeshAgent navMeshAgent;
    HallSystemManager hallSystemManager;
    GuestSound guestSound;

    // 손님 상태 
    public enum GuestState
    {
        MoveToOrder, // 입장
        WatingOrder, // 주문 대기
        Ordering, // 주문
        MoveToSeat, // 좌석으로 이동
        Wating, // 주문 후 대기
        Eating, // 식사
        Leaving // 퇴장
    }
    public GuestState currentState;

    void Start()
    {
        guestBehaviour = GetComponent<GuestBehaviour>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        guestSound = GetComponent<GuestSound>();
        hallSystemManager = HallSystemManager.Instance;

        currentState = GuestState.MoveToOrder;
        navMeshAgent.updateRotation = false; // 자동 회전 비활성화

        initialPosition = gameObject.transform.position;

        uiCanvas.SetActive(false);

        JoinQueue(); // 대기줄에 참여
        StartCoroutine(DoActionToState()); // 행동 루틴 실행
    }

    // 매 프레임마다 행동하는 루틴
    IEnumerator DoActionToState()
    {
        yield return new WaitForSeconds(0.5f);
        guestBehaviour.OnMove();

        while (true)
        {
            StateHandler();
            yield return null;
        }
    }

    // 상태를 조건에 따라 전환하는 메서드
    void StateHandler()
    {
        switch (currentState)
        {
            // 주문하러 입장할 떄
            case GuestState.MoveToOrder:
                int index = hallSystemManager.guestQueue.IndexOf(this);
                MoveTo(hallSystemManager.GetQueuePosition(index), () =>
                {
                    // 주문위치에 도달했다면
                    if (index == 0)
                    {
                        // 주문 시작
                        guestBehaviour.OnOrder();
                        currentState = GuestState.Ordering;

                        transform.rotation = Quaternion.LookRotation(hallSystemManager.GetQueueRotation());

                        // 주문 대기 음성
                        guestSound.Throat();

                        // UI 활성화 및 주문표시
                        uiCanvas.GetComponentInChildren<DisplayRequireTo>().DisplayOrder();
                        uiCanvas.GetComponentInChildren<DisplayRequireTo>().recipeInfo.SetActive(false);
                        uiCanvas.GetComponentInChildren<ImageFilling>().duration = guestBehaviour.guest.orderingPatienceTime;
                        uiCanvas.SetActive(true);
                    }
                    // 아직 줄을 서고 있다면
                    else
                    {
                        transform.rotation = Quaternion.LookRotation(hallSystemManager.GetQueueRotation());
                        currentState = GuestState.WatingOrder;
                    }
                });
                break;

            // 줄을 서고 있을 때
            case GuestState.WatingOrder:
                guestBehaviour.OnIdle();
                break;

            // 주문할 때
            case GuestState.Ordering:
                // 주문이 됐다면
                if (isOrderClear)
                {
                    // 줄을 한칸씩 땡김
                    LeaveQueue();

                    // 주문 대기 음성 해제 및 만족 음성
                    guestSound.StopThroat();
                    guestSound.Satisfacton();

                    // 비어있는 좌석으로 이동
                    guestBehaviour.OnMove();
                    hallSystemManager.GetSeatObject(out currentTable);
                    seatPosition = currentTable.GetComponent<Table>().seat.transform.position;

                    // UI 활성화 및 음식표시
                    uiCanvas.SetActive(true);
                    uiCanvas.GetComponentInChildren<DisplayRequireTo>().recipeInfo.SetActive(true);
                    uiCanvas.GetComponentInChildren<DisplayRequireTo>().DisplayFood(recipeData.RecipeImage, recipeData.RecipeName);
                    uiCanvas.GetComponentInChildren<ImageFilling>().enabled = false; // 타이머 비활성화
                    uiCanvas.GetComponentInChildren<ImageFilling>().InitializeFill();

                    currentState = GuestState.MoveToSeat;
                }
                break;

            // 좌석으로 이동할 때
            case GuestState.MoveToSeat:
                // 좌석으로 이동했다면
                MoveTo(seatPosition, () =>
                {
                    // 만약 테이블이 더럽다면
                    if (currentTable.GetComponent<Table>().isDirty)
                    {
                        // 분노 수치 증가
                        hallSystemManager.RisingAngry();
                        EffectSpawner.Instance.SpawnAngryEffect(transform.position + new Vector3(0, 3, 0));

                        // 불만족 음성
                        guestSound.Nod();

                        // 퇴장
                        ExitTable();
                    }
                    else
                    {
                        // 주문 후 대기 시작
                        currentState = GuestState.Wating;

                        guestBehaviour.OnWating();
                    }

                });
                break;

            // 주문 후 대기 
            case GuestState.Wating:

                // 타이머 재활성화
                uiCanvas.GetComponentInChildren<ImageFilling>().duration = guestBehaviour.guest.eatingPatienceTime;
                uiCanvas.GetComponentInChildren<ImageFilling>().enabled = true;

                // 현재 객체와 책상 간의 방향 벡터 계산
                Vector3 direction = currentTable.GetComponent<Table>().table.transform.position - transform.position;
                direction.y = 0;

                // 방향 벡터를 회전 값으로 변환
                transform.rotation = Quaternion.LookRotation(direction);

                transform.position = seatPosition;

                // 음식이 도착했다면
                if (IsFoodArrive())
                {
                    // 음식이 맞게 왔다면
                    if (IsFoodCollect())
                    {
                        // 사운드 플레이(메뉴 맞음)
                        SoundManager.Instance.PlaySfx(SFX.MenuCorrect);

                        // 식사 시작
                        guestBehaviour.OnEating();
                        currentState = GuestState.Eating;

                        // UI 비활성화
                        uiCanvas.SetActive(false);
                    }
                    // 다른 음식이 왔다면
                    else
                    {
                        // 분노 수치 증가
                        hallSystemManager.RisingAngry();
                        EffectSpawner.Instance.SpawnAngryEffect(transform.position + new Vector3(0, 3, 0));

                        // 불만족 음성
                        guestSound.Nod();

                        // 테이블 더러움 상태 활성화
                        currentTable.GetComponent<Table>().isDirty = true;

                        // 퇴장
                        ExitTable();
                    }
                }
                break;

            // 식사할 때
            case GuestState.Eating:
                // 음식을 다 먹으면
                if (isFoodEating)
                {
                    // 놓여진 음식 삭제
                    Destroy(myPlate.gameObject);

                    // 빈 접시 생성
                    currentTable.GetComponent<Table>().SetDirtyPlate();

                    // 테이블 더러움 상태 활성화
                    currentTable.GetComponent<Table>().isDirty = true;

                    // 돈(포인트) 추가
                    EffectSpawner.Instance.SpawnMoneyEffect(transform.position + new Vector3(0, 3, 0), "+" + recipeData.RecipePoint.ToString());
                    GameManager.Instance.Money += recipeData.RecipePoint;
                    UiManager.Instance.UpdateMoney();

                    // 사운드 플레이(포인트 획득)
                    SoundManager.Instance.PlaySfx(SFX.GetPoint);

                    // 퇴장
                    ExitTable();
                }
                break;

            // 퇴장할 때
            case GuestState.Leaving:
                guestSound.StopThroat();

                // UI 비활성화
                uiCanvas.SetActive(false);

                // 퇴장 후 오브젝트 삭제
                MoveTo(initialPosition, () => Destroy(gameObject));
                break;
        }
    }

    // 특정 위치로 이동 후 메서드를 실행하는 메서드
    void MoveTo(Vector3 targetPosition, Action onReachDestination)
    {
        // 현재 위치에서 목표 위치로 이동
        navMeshAgent.SetDestination(targetPosition);

        //목표 방향으로 회전
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // 목표 위치에 도달했는지 확인
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                onReachDestination?.Invoke();
            }
        }
    }

    // 대기열에 등록
    public void JoinQueue()
    {
        if (!hallSystemManager.guestQueue.Contains(this))
        {
            hallSystemManager.guestQueue.Add(this);
        }
    }

    // 대기열에서 해제
    public void LeaveQueue()
    {
        // 대기열에서 손님 제거
        hallSystemManager.guestQueue.Remove(this);

        foreach (var guest in hallSystemManager.guestQueue)
        {
            guest.currentState = GuestState.MoveToOrder;
            guest.guestBehaviour.OnMove();
        }
    }

    // 테이블에 음식이 놓였는지 확인하고 음식데이터를 가져오는 메서드
    public bool IsFoodArrive()
    {
        if (currentTable.GetComponentInChildren<Plate>())
        {
            myPlate = currentTable.GetComponentInChildren<Plate>();
            return true;
        }
        else if (currentTable.GetComponentInChildren<DirtyPlate>())
        {
            // 분노 수치 증가
            hallSystemManager.RisingAngry();
            EffectSpawner.Instance.SpawnAngryEffect(transform.position + new Vector3(0, 3, 0));

            // 불만족 음성
            guestSound.Nod();

            // 퇴장
            ExitTable();

            return false;
        }
        else
        {
            return false;
        }
    }

    // 요구하는 음식이 나왔는지 확인하는 메서드
    public bool IsFoodCollect()
    {
        return myPlate.CompareWithRecipe(recipeData);
    }

    // 테이블에서 퇴장하는 메서드
    public void ExitTable()
    {
        guestBehaviour.OnMove();
        hallSystemManager.fullSeats.Remove(currentTable);
        hallSystemManager.emptySeats.Add(currentTable);
        currentState = GuestState.Leaving;
    }
}
