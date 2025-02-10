using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField]
    private Transform putDownPoint;
    [SerializeField]
    private GameObject tool;

    public Transform PickUpPoint;
    public Transform InteractableObject { get; private set; }

    private bool isCarrying = false; // 오브젝트를 들고 있는 상태인지 체크
    //private bool isPerformingAction = false; // 동작을 수행 중인지 체크
    private bool hasKnife = false; // 나이프를 들고 있는 상태인지 체크
    // 빗자루를 들고 있는 상태인지 체크
    private bool hasAirbrush = false;

    private PlayerController playerController;
    private Animator animator;
    private ObjectDetector objectDetector;
    public InputAction interactAction;

    private Transform interactingObject;

    public void Initialize()
    {
        animator = GetComponent<Animator>();
        objectDetector = GetComponentInChildren<ObjectDetector>();
        objectDetector.OnInteractableObject = OnInteractableObject;
        interactAction = InputSystem.actions.FindAction("Interact");
        playerController = GetComponent<PlayerController>();
    }

    public void HandleInteraction()
    {
        // 상호작용 키 감지
        if (interactAction.triggered)
        {
            if (InteractableObject != null)
            {
                RotateBodyDirection();
            }

            if (!isCarrying)
            {
                // 주문을 받을 때는 줍는 애니메이션 없이 상호작용 하기
                if (InteractableObject != null && InteractableObject.tag == "Reception")
                {
                    HallSystemManager.Instance.GetTakeOrder();
                    return;
                }
                // 코드를 별도로 뺴지 않고 줍기 애니메이션 실행만 되지 않도록 구현
                // 칼질, 청소 등에 사용되는 액션 애니메이션에도 줍기와 동일한 이벤트 함수 포함
                else if (InteractableObject != null && (hasKnife || hasAirbrush)
                    && InteractableObject.tag != "CuttingBoard"
                    && InteractableObject.tag != "CleaningToolBox"
                    && InteractableObject.tag != "Table")
                {
                    playerController.SetIsPerformingAction(true);
                    animator.SetTrigger("Action");
                    return;
                }

                PickUpObject();
            }
            else
            {
                // 주문대에서는 내려놓는 애니메이션 하지 않기
                if (InteractableObject != null && InteractableObject.tag == "Reception")
                {
                    return;
                }

                PutDownObject();
            }
        }
    }

    //물건 들어올릴 때 호출
    private void PickUpObject()
    {
        if (InteractableObject == null)
        {
            return;
        }

        playerController.SetIsPerformingAction(true);
        animator.SetTrigger("PickUp");
    }

    //물건 내려놓을 때 호출
    private void PutDownObject()
    {
        if (InteractableObject == null)
        {
            return;
        }

        playerController.SetIsPerformingAction(true);
        animator.SetTrigger("PutDown");
    }

    private void OnInteractableObject(Transform interactableObject)
    {
        this.InteractableObject = interactableObject;
    }

    //물건 들어올리는중에 호출, 애니메이션 이벤트
    public void OnPickingUpObject()
    {
        if (InteractableObject == null)
        {
            return;
        }

        switch (InteractableObject.tag)
        {
            case "ObjectDispenser":
                HandleObjectDispenserPickUp();
                break;
            case "Kitchentable":
                HandleKitchenTablePickUp();
                break;
            case "Table":
                HandleTablePickUp();
                break;
            case "Stove":
                HandleStovePickUp();
                break;
            case "CuttingBoard":
                HandleCuttingBoardPickUp();
                break;
            case "CleaningToolBox":
                HandleAirbrushPickUp();
                break;
            case "Stain":
                HandleCleaning();
                break;
            case "InteractableObject":
                HandleGenericInteractableObject();
                break;
            default:
                Debug.LogWarning($"No handling implemented for tag: {InteractableObject.tag}");
                break;
        }
    }

    //물건 내려놓는중에 호출, 애니메이션 이벤트
    public void OnPuttingDownObject()
    {
        if (InteractableObject == null)
        {
            return;
        }

        switch (InteractableObject.tag)
        {
            case "ObjectDispenser":
                HandleObjectDispenserPutDown();
                break;
            case "Kitchentable":
                HandleKitchenTablePutDown();
                break;
            case "Stove":
                HandleStovePutDown();
                break;
            case "Table":
                HandleTablePutDown();
                break;
            case "Sink":
                HandleSinkPutDown();
                break;
            case "Trashcan":
                HandleTrashcanPutDown();
                break;
        }
    }

    //물건 들어올리기가 끝나면 호출, 애니메이션 이벤트
    public void OnPickedUpObject()
    {
        if (isCarrying)
        {
            animator.SetLayerWeight(1, 1);
        }

        playerController.SetIsPerformingAction(false);
    }

    //물건 내려놓기가 끝나면 호출, 애니메이션 이벤트
    public void OnPutDownObject()
    {
        if (!isCarrying)
        {
            animator.SetLayerWeight(1, 0);
        }

        playerController.SetIsPerformingAction(false);
    }

    private void HandleObjectDispenserPickUp()
    {
        if (hasKnife)
        {
            return;
        }

        isCarrying = true;

        var outputObject = InteractableObject.GetComponent<ObjectDispenser>();
        interactingObject = outputObject.SpawnObjectAndReturnTransform();

        PlaceObjectInHand();
    }

    private void HandleObjectDispenserPutDown()
    {
        SoundManager.Instance.PlaySfx(SFX.PutDown);

        string nameA = interactingObject.GetComponent<Ingredient>().IngredientData.name;
        string nameB = InteractableObject.GetComponent<ObjectDispenser>().prefab.GetComponent<Ingredient>().IngredientData.IngredientName;

        if (nameA.Equals(nameB))
        {
            Destroy(interactingObject.gameObject);
            interactingObject = null;
            isCarrying = false;
        }
    }

    private void HandleCleaning()
    {
        if (InteractableObject.tag == "Stain" && hasAirbrush)
        {
            SoundManager.Instance.PlaySfx(SFX.Polish);
            GameManager.Instance.StainCount--;
            Destroy(InteractableObject.gameObject);
            InteractableObject = null;
        }
    }

    private void HandleAirbrushPickUp()
    {
        if (hasAirbrush)
        {
            SoundManager.Instance.PlaySfx(SFX.PutDown);
        }
        else
        {
            SoundManager.Instance.PlaySfx(SFX.PickUp);
        }

        hasAirbrush = !hasAirbrush;
        tool.SetActive(hasAirbrush);
        InteractableObject.GetChild(0).gameObject.SetActive(!hasAirbrush);
        InteractableObject.GetChild(1).gameObject.SetActive(hasAirbrush);
    }

    private void HandleCuttingBoardPickUp()
    {
        if (hasKnife)
        {
            SoundManager.Instance.PlaySfx(SFX.PutDown);
        }
        else
        {
            SoundManager.Instance.PlaySfx(SFX.PickUp);
        }

        hasKnife = !hasKnife;
        tool.SetActive(hasKnife);
        InteractableObject.GetChild(0).gameObject.SetActive(!hasKnife);
        InteractableObject.GetChild(1).gameObject.SetActive(hasKnife);
    }

    private void HandleGenericInteractableObject()
    {
        isCarrying = true;
        interactingObject = InteractableObject;
        PlaceObjectInHand();
    }

    private void HandleKitchenTablePickUp()
    {
        if (InteractableObject.childCount == 0)
        {
            return;
        }

        GameObject obj = InteractableObject.GetChild(0).gameObject;

        if (hasKnife)
        {
            SoundManager.Instance.PlaySfx(SFX.Cut);

            var cuttable = obj.GetComponent<CuttableObject>();
            if (cuttable != null && !cuttable.IsCuttedObject())
            {
                obj.transform.GetChild(0).gameObject.SetActive(false);
                obj.transform.GetChild(1).gameObject.SetActive(true);
                cuttable.SetIsCuttedBoolean(true);
            }
        }
        else
        {
            var cuttable = obj.GetComponent<CuttableObject>();
            if (cuttable != null && cuttable.IsCuttedObject())
            {
                isCarrying = true;
                interactingObject = cuttable.SpawnObjectAndReturnTransform();
            }
            else
            {
                isCarrying = true;
                interactingObject = obj.transform;
            }

            PlaceObjectInHand();
        }
    }

    private void HandleKitchenTablePutDown()
    {
        if (InteractableObject.childCount == 0) // 하위 오브젝트가 없는 경우 (테이블 위에 오브젝트가 없는 경우)
        {
            PlaceObject(InteractableObject, new Vector3(0f, 1f, 0f), Quaternion.Euler(0, 90, 0));
        }
        else if (InteractableObject.GetChild(0).name == "plate(Clone)") // 테이블 위에 접시가 있는 경우
        {
            var plate = InteractableObject.GetChild(0).GetComponent<Plate>();
            float height = plate.getHeight();
            float offset = interactingObject.GetComponent<Ingredient>().IngredientData.Offset;

            HandleBunLogic(plate);

            PlaceObject(InteractableObject.GetChild(0), new Vector3(0f, height, 0f), Quaternion.Euler(0, 90, 0));
            plate.setHeight(height + offset);
        }
    }

    private void HandleStovePickUp()
    {
        if (InteractableObject.childCount < 4)
        {
            return;
        }

        isCarrying = true;
        interactingObject = InteractableObject.GetChild(3);
        PlaceObjectInHand();

        var patty = interactingObject.GetComponent<Patty>();
        if (patty != null)
        {
            patty.SetIsOnStove(false);
            InteractableObject.GetChild(1).gameObject.SetActive(false);
            InteractableObject.GetChild(2).gameObject.SetActive(false);
        }
    }

    private void HandleStovePutDown()
    {
        if (InteractableObject.childCount > 3) 
        { 
            return; 
        }

        var patty = interactingObject.GetComponent<Patty>();
        PlaceObject(InteractableObject, new Vector3(0f, 1.3f, 0f), Quaternion.Euler(0, 90, 0));
        patty.SetIsOnStove(true);
        patty.OnSmokeByState();
    }

    private void HandleTablePickUp()
    {
        if (InteractableObject.childCount < 2 || hasAirbrush)
        {
            return;
        }

        isCarrying = true;

        Table table = InteractableObject.GetComponent<Table>();
        if (table != null)
        {
            table.isDirty = false;
        }

        interactingObject = InteractableObject.GetChild(1);
        PlaceObjectInHand();
    }

    private void HandleTablePutDown()
    {
        if (InteractableObject.childCount >= 2)
        {
            return;
        }

        PlaceObject(InteractableObject, new Vector3(0f, 1.2f, 0f), Quaternion.Euler(0, 90, 0));
    }

    private void HandleSinkPutDown()
    {
        if (interactingObject.name != "plate_dirty(Clone)")
        {
            return;
        }

        SoundManager.Instance.PlaySfx(SFX.PutDown);
        Destroy(interactingObject.gameObject);
        interactingObject = null;
        isCarrying = false;
    }

    private void HandleTrashcanPutDown()
    {
        SoundManager.Instance.PlaySfx(SFX.Trashcan);

        int price = 0;

        if (interactingObject.name == "plate(Clone)")
        {
            // 하위 오브젝트 전체를 순회
            foreach (Transform child in interactingObject)
            {
                Ingredient ingredient = child.GetComponent<Ingredient>();
                if (ingredient != null && ingredient.IngredientData != null)
                {
                    price += ingredient.IngredientData.Price;
                }
            }

            price += 10;
        }
        else if (interactingObject.name == "plate_dirty(Clone)")
        {
            price += 10;
        }
        else
        {
            price = interactingObject.GetComponent<Ingredient>().IngredientData.Price;
        }

        price = price / 10 * -6;

        Destroy(interactingObject.gameObject);
        interactingObject = null;
        isCarrying = false;

        Vector3 pos = new Vector3(transform.position.x, 3f, transform.position.z);
        EffectSpawner.Instance.SpawnMoneyEffect(pos, "" + price);
        GameManager.Instance.Money += price;
        UiManager.Instance.UpdateMoney();
    }

    private void PlaceObjectInHand()
    {
        SoundManager.Instance.PlaySfx(SFX.PickUp);
        interactingObject.SetParent(PickUpPoint);
        interactingObject.localPosition = Vector3.zero;
        interactingObject.localRotation = Quaternion.identity;
    }

    private void PlaceObject(Transform parent, Vector3 position, Quaternion rotation)
    {
        SoundManager.Instance.PlaySfx(SFX.PutDown);
        isCarrying = false;
        interactingObject.SetParent(parent);
        interactingObject.localPosition = position;
        interactingObject.localRotation = rotation;
        interactingObject = null;
    }

    private void RotateBodyDirection()
    {
        Vector3 direction = InteractableObject.position - transform.position;

        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    private void HandleBunLogic(Plate plate)
    {
        if (interactingObject.name == "food_ingredient_bun(Clone)")
        {
            if (plate.IsFirstBunOnPlate())
            {
                interactingObject.transform.GetChild(0).gameObject.SetActive(false);
                interactingObject.transform.GetChild(1).gameObject.SetActive(false);
                interactingObject.transform.GetChild(2).gameObject.SetActive(true);
                plate.SetIsFirstBoolean(false);
            }
            else
            {
                interactingObject.transform.GetChild(0).gameObject.SetActive(false);
                interactingObject.transform.GetChild(1).gameObject.SetActive(true);
                interactingObject.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }
}
