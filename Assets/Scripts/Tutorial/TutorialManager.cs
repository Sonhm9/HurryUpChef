using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GuestSpawnManager guestSpawnManager;
    [SerializeField]
    private Transform platePoint;
    [SerializeField]
    private Transform kitchenTablePoint;
    [SerializeField]
    private Transform fridgePoint;
    [SerializeField]
    private Transform stovePoint;
    [SerializeField]
    private Transform bunPoint;
    [SerializeField]
    private Transform cheesePoint;
    [SerializeField]
    private Transform knifePoint;
    [SerializeField]
    private Transform anotherKitchenTablePoint;
    [SerializeField]
    private Transform foodTablePoint;
    [SerializeField]
    private Transform sinkPoint;
    [SerializeField]
    private Transform airBrushPoint;

    public GameObject marker;
    public GameObject display;
    public GameObject UISelector;

    public string[] texts;

    public int step = 0;

    private Patty currentPatty;
    private GuestController guestController;

    private void Start()
    {
        Time.timeScale = 0f;
        DisplayText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (step)
            {
                //스탭 1으로 이동
                case 0:
                    HideText();
                    step++;
                    DisplayText();
                    Time.timeScale = 1f;
                    break;

                //튜토리얼 종료, 스테이지 이동
                case 26:
                    HideText();
                    Time.timeScale = 1f;
                    PlayerPrefs.SetString("IsFirstTutorial", "False");
                    SceneManager.LoadScene("StageSelectScene");
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (step)
            {
                //스탭 2 으로 이동
                case 1:
                    HideText();
                    step++;
                    guestSpawnManager.InstantiateGuest(guestSpawnManager.guestTypes[0]);
                    guestSpawnManager.StopGuestSpawn();
                    guestController = GameObject.FindGameObjectWithTag("Guest").GetComponent<GuestController>();

                    GameManager.Instance.OnCallback = () =>
                    {
                        GameManager.Instance.CanSwitchCharacter = false;
                    };
                    break;
                //스탭 4으로 이동
                case 3:
                    HideText();
                    step++;
                    DisplayText();
                    break;

                //스탭 21로 이동
                case 20:
                    HideText();
                    step++;
                    DisplayText();
                    LocateUISelector(new Vector2(0f, -5f), 250f, 100f);
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (step)
            {
                //스탭 3으로 이동
                case 2:
                    if (guestController.currentState == GuestController.GuestState.Ordering)
                    {
                        HideMarker();
                        HideText();
                        step++;
                        DisplayText();
                        guestController.guestBehaviour.guest.eatingPatienceTime = 3600f;
                        GameManager.Instance.CanSwitchCharacter = true;
                    }
                    break;

                case 5:
                    LocateMarker(kitchenTablePoint.position);
                    break;

                //스탭 7으로 이동
                case 6:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Crate_Buns"))
                        {
                            LocateMarker(kitchenTablePoint.position);
                        }
                    }
                    break;

                //스탭 8로 이동
                case 7:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Fridge"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(stovePoint.position);
                        }
                    }
                    break;

                //스탭 10으로 이동
                case 9:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Stove"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(kitchenTablePoint.position);
                        }
                    }
                    break;

                //스탭 11으로 이동
                case 10:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Kitchentable"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(cheesePoint.position);
                        }
                    }
                    break;

                //스탭 12로 이동
                case 11:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Crate_Cheese"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(anotherKitchenTablePoint.position);
                        }
                    }
                    break;

                //스탭 13로 이동
                case 12:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Kitchentable")
                            && interactedObject.childCount <= 0)
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(knifePoint.position);
                        }
                    }
                    break;

                //스탭 14로 이동
                case 13:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("CuttingBoard"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(anotherKitchenTablePoint.position);
                        }
                    }
                    break;

                //스탭 15로 이동
                case 14:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Kitchentable")
                            && interactedObject.childCount > 0 && interactedObject.GetChild(0).name.Contains("cheese"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(knifePoint.position);
                        }
                    }
                    break;

                //스탭 16로 이동
                case 15:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("CuttingBoard"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(anotherKitchenTablePoint.position);
                        }
                    }
                    break;

                //스탭 17로 이동
                case 16:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Kitchentable")
                            && interactedObject.childCount > 0 && interactedObject.GetChild(0).name.Contains("cheese"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(kitchenTablePoint.position);
                        }
                    }
                    break;

                //스탭 18로 이동
                case 17:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Kitchentable")
                            && interactedObject.childCount > 0 && interactedObject.GetChild(0).name.Contains("plate"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                        }
                    }
                    break;

                //스탭 19로 이동
                case 18:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Kitchentable")
                            && interactedObject.childCount > 0 && interactedObject.GetChild(0).name.Contains("plate"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(foodTablePoint.position);
                        }
                    }
                    break;

                //스탭 20로 이동
                case 19:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Window"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            HideMarker();
                            GameManager.Instance.CanSwitchCharacter = true;
                        }
                    }
                    break;

                //스탭 22로 이동
                case 21:
                    {
                        GameObject interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject.gameObject;
                        if (interactedObject == guestController.currentTable)
                        {
                            HideText();
                            step++;
                            DisplayText();
                            HideUISelector();
                        }
                    }
                    break;

                //스탭 23로 이동
                case 22:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Window"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            LocateMarker(sinkPoint.position);
                            GameManager.Instance.CanSwitchCharacter = true;
                            HideUISelector();
                        }
                    }
                    break;

                //스탭 24로 이동
                case 23:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("Sink"))
                        {
                            HideText();
                            step++;
                            DisplayText();
                            GameManager.Instance.SpawnStain();
                            LocateMarker(airBrushPoint.position);
                            GameManager.Instance.CanSwitchCharacter = true;
                        }
                    }
                    break;

                //스탭 25로 이동
                case 24:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.name.Contains("CleaningToolBox"))
                        {
                            HideText();
                            step++;
                            HideMarker();
                        }
                    }
                    break;

                //스탭 26로 이동
                case 25:
                    {
                        Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
                        if (interactedObject != null && interactedObject.tag == "Stain")
                        {
                            HideText();
                            step++;
                            DisplayText();
                        }
                    }
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            switch (step)
            {
                //스탭 5으로 이동
                case 4:
                    HideText();
                    step++;
                    DisplayText();
                    LocateMarker(platePoint.position);
                    break;
            }
        }

        if (step == 2)
        {
            GameObject guest = GameObject.FindGameObjectWithTag("Guest");
            if (guest != null)
            {
                LocateMarker(guest.transform.position);
                GuestController controller = guest.GetComponent<GuestController>();
                if (controller.currentState == GuestController.GuestState.Ordering)
                {
                    DisplayText();
                }
            }
        }
        //스탭 6으로 이동
        else if (step == 5)
        {
            Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
            if (interactedObject != null && interactedObject.name.Contains("Kitchentable")
                && interactedObject.childCount > 0 && interactedObject.GetChild(0).name.Contains("plate"))
            {
                HideText();
                step++;
                DisplayText();
                LocateMarker(bunPoint.position);
            }
        }
        //스탭 7으로 이동
        else if (step == 6)
        {
            Transform interactedObject = GameManager.Instance.CurrentPlayer.InteractableObject;
            if (interactedObject != null && interactedObject.name.Contains("Kitchentable")
                && interactedObject.childCount > 0 && interactedObject.GetChild(0).name.Contains("plate")
                && interactedObject.GetChild(0).childCount > 0
                && interactedObject.GetChild(0).GetChild(0).name.Contains("bun"))
            {
                HideText();
                step++;
                DisplayText();
                LocateMarker(fridgePoint.position);
            }
        }
        //스탭 9으로 이동
        else if (step == 8)
        {
            if (currentPatty == null && GameManager.Instance.CurrentPlayer.PickUpPoint.childCount > 0)
            {
                currentPatty = GameManager.Instance.CurrentPlayer.PickUpPoint.GetChild(0).GetComponent<Patty>();
            }

            if (currentPatty != null && currentPatty.transform.GetChild(1).gameObject.activeSelf)
            {
                currentPatty.enabled = false;
                HideText();
                step++;
                DisplayText();
            }
        }
    }

    // 텍스트에 원하는 내용을 표시하는 메서드
    public void DisplayText()
    {
        display.SetActive(true);
        display.GetComponentInChildren<TextMeshProUGUI>().text = texts[step];
    }

    // 텍스트 표시를 숨기는 메서드
    public void HideText()
    {
        display.SetActive(false);
    }

    // 마커를 원하는 위치에 표시하는 메서드
    public void LocateMarker(Vector3 position)
    {
        marker.SetActive(true);
        marker.transform.position = position + new Vector3(0, 0.1f, 0);
    }

    // 마커를 숨기는 메서드
    public void HideMarker()
    {
        marker.SetActive(false);
    }

    // UI마커를 원하는 위치에 표시하고 사이즈를 조절하는 메서드
    public void LocateUISelector(Vector2 position, float width, float height)
    {
        UISelector.SetActive(true);
        UISelector.GetComponent<RectTransform>().anchoredPosition = position;

        UISelector.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    // 마커를 숨기는 메서드
    public void HideUISelector()
    {
        marker.SetActive(false);
    }
}
