using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private SettingPopup settingPopup;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private PlayerController[] playerControllers;
    [SerializeField]
    private Transform[] stainPrefabs;
    [SerializeField]
    private Vector3 minStainPos;
    [SerializeField]
    private Vector3 maxStainPos;
    [SerializeField]
    private float[] stageTimeLimits;

    public bool IsFocusedChef { get; set; } = true;
    public int Money { get; set; }
    public bool IsStartedGame { get; set; }
    public float ElapsedTime { get; private set; }
    public float StageTimeLimit { get; private set; }
    public float StainCount { get; set; }
    public bool CanSwitchCharacter { get; set; } = true;
    public bool IsClear { get; set; }
    public PlayerInteractionController CurrentPlayer { get; set; }
    public Action OnCallback { get; set; }
    public int CurrentStage;

    private float elapsedStaineTime;
    private readonly float stainTime = 60f;

    //카메라 이동을 위한 키(Q) 액션
    private InputAction switchCameraAction;
    //세팅 팝업 키(ESC) 액션
    private InputAction settingAction;
    //레시피 팝업 키(R) 액션
    private InputAction displayRecipeAction;
    //레시피 팝업창 넘기기 키(E) 액션
    private InputAction nextRecipeAction;
    //레시피 팝업창 이전 키(Q) 액션
    private InputAction previousRecipeAction;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        switchCameraAction = InputSystem.actions.FindAction("SwitchCamera");
        settingAction = InputSystem.actions.FindAction("Setting");
        displayRecipeAction = InputSystem.actions.FindAction("Recipe");
        nextRecipeAction = InputSystem.actions.FindAction("NextRecipe");
        previousRecipeAction = InputSystem.actions.FindAction("PreviousRecipe");

        StageTimeLimit = stageTimeLimits[SceneManager.GetActiveScene().buildIndex - 1];

    }

    private void Start()
    {
        settingPopup.Initialize();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ChangeBgm(BGM.Game);
            SoundManager.Instance.StopSfx();
        }

        if (SceneManager.GetActiveScene().name != "TutorialScene")
        {
            IsStartedGame = true;
        }

        //게임 시작 시 설정 된 플레이어에게 포커스 초기 조정
        SwitchPlayer();
    }

    private void Update()
    {
        if (settingAction.triggered)
        {
            UiManager.Instance.ToggleSettingPopup();
        }

        if (switchCameraAction.triggered)
        {
            if (!CanSwitchCharacter)
            {
                UiManager.Instance.PreviousRecipePopup();
                return;
            }

            IsFocusedChef = !IsFocusedChef;
            SwitchPlayer();
        }

        if (displayRecipeAction.triggered)
        {
            UiManager.Instance.ShowRecipePopup();
        }

        if (!CanSwitchCharacter)
        {
            if (nextRecipeAction.triggered)
            {
                UiManager.Instance.NextRecipePopup();
            }
        }

        if (IsStartedGame)
        {
            CheckGameOver();

            ElapsedTime += Time.deltaTime;
            elapsedStaineTime += Time.deltaTime;

            UiManager.Instance.SetTimer(StageTimeLimit, ElapsedTime);
            if (ElapsedTime >= StageTimeLimit)
            {
                IsClear = true;
                SoundManager.Instance.PlaySfx(SFX.GameClear);
                GameOver();
            }

            if (elapsedStaineTime >= stainTime)
            {
                elapsedStaineTime = 0f;
                SpawnStain();
            }
        }
    }

    private void CheckGameOver()
    {
        if (HallSystemManager.Instance.angry >= 3)
        {
            IsClear = false;
            GameOver();
        }
    }

    private void GameOver()
    {
        IsStartedGame = false;
        UiManager.Instance.ShowResultPopup(IsClear);
    }

    private void SwitchPlayer()
    {
        if (!CanSwitchCharacter)
        {
            return;
        }

        if (IsFocusedChef)
        {
            CurrentPlayer = playerControllers[0].GetComponent<PlayerInteractionController>();
        }
        else
        {
            CurrentPlayer = playerControllers[1].GetComponent<PlayerInteractionController>();
        }

        cameraController.SwitchCamera(IsFocusedChef, OnCallback);

        foreach (PlayerController player in playerControllers)
        {
            player.SwitchPlayer(IsFocusedChef);
        }
    }

    public void SpawnStain()
    {
        StainCount++;

        if (StainCount >= 4)
        {
            HallSystemManager.Instance.RisingAngry();
        }

        Vector3 stainPos = new Vector3(Random.Range(minStainPos.x, maxStainPos.x),
                    0f, Random.Range(minStainPos.z, maxStainPos.z));
        Instantiate(stainPrefabs[Random.Range(0, stainPrefabs.Length)], stainPos, Quaternion.identity);
    }
}
