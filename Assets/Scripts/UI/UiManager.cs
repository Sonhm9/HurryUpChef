using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField]
    private Image timerBar;
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private ResultPopup resultPopup;
    [SerializeField]
    private SettingPopup settingPopup;
    [SerializeField]
    private RecipePopup recipePopup;
    [SerializeField]
    private Image[] angryPointImages;

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
    }

    private void Update()
    {
        UpdateAngryPoints();
    }

    public void UpdateMoney()
    {
        moneyText.text = GameManager.Instance.Money.ToString();
    }

    private void UpdateAngryPoints()
    {
        if (HallSystemManager.Instance.angry > 0 && HallSystemManager.Instance.angry <= 3)
        {
            angryPointImages[HallSystemManager.Instance.angry - 1].gameObject.SetActive(true);
        }
    }

    public void ToggleSettingPopup()
    {
        if (!settingPopup.gameObject.activeSelf)
        {
            settingPopup.ShowPopup();
            return;
        }

        settingPopup.HidePopup();
    }

    public void ShowResultPopup(bool isClear)
    {
        resultPopup.ShowPopup(isClear);
    }

    public void ShowRecipePopup()
    {
        recipePopup.ShowPopup();
    }

    public void NextRecipePopup()
    {
        recipePopup.NextPage();
    }

    public void PreviousRecipePopup()
    {
        recipePopup.PreviousPage();
    }

    public void SetTimer(float timeLimit, float elapsedTime)
    {
        timerBar.fillAmount = 1 - (elapsedTime / timeLimit);
    }
}
