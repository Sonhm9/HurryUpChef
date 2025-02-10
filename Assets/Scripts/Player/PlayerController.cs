using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected PlayerMovementController movementController;
    [SerializeField]
    protected PlayerInteractionController interactionController;
    //[SerializeField]
    //private PlayerSwitchController switchController;

    [SerializeField]
    protected GameObject playerMarker;

    protected bool isPerformingAction = false; // 동작을 수행 중인지 체크
    protected bool isChef = false; // 자신의 플레이어가 셰프인지, 웨이터인지 체크
    protected bool isFocusedChef = true; //현재 화면이 빛추고 있는 플레이어를 체크

    protected Animator animator;

    //private float moveX;
    //private float moveZ;

    protected virtual void Awake()
    {
        // 각 컨트롤러 초기화
        movementController.Initialize();
        interactionController.Initialize();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        isChef = this is Chef;
        //objectDetector.OnInteractableObject = OnInteractableObject;
    }

    protected virtual void Update()
    {
        //화면에 포커스 된 플레이어만 조작 가능.        
        if (isChef != isFocusedChef)
        {
            //전환 시 움직임 정지
            movementController.StopMoving();
            return;
        }

        if (isPerformingAction)
        {
            movementController.StopMoving();
            return; // 동작 수행 중 이동 불가
        }

        movementController.HandleMovement();
        interactionController.HandleInteraction();
    }

    protected virtual void FixedUpdate()
    {
        //화면에 포커스 된 플레이어만 조작 가능.        
        if (isChef != isFocusedChef)
        {
            return;
        }

        if (isPerformingAction)
        {
            return; // 동작 수행 중 이동 불가
        }

        movementController.ApplyMovement();
    }

    //조작 할 플레이어 변경 시 호출
    public void SwitchPlayer(bool isFocusedChef)
    {
        animator.SetBool("Walk", false);

        this.isFocusedChef = isFocusedChef;
        Waiter waiter = GetComponent<Waiter>();
        Chef chef = GetComponent<Chef>();

        if (isFocusedChef)
        {
            waiter?.playerMarker.SetActive(false);
            chef?.playerMarker.SetActive(true);

            return;
        }

        waiter?.playerMarker.SetActive(true);
        chef?.playerMarker.SetActive(false);
    }

    public void SetIsPerformingAction(bool b)
    {
        isPerformingAction = b;
    }
}
