using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pointImages;
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private TextMeshProUGUI nextButtonText;

    private bool isClear;

    public void ShowPopup(bool isClear)
    {
        //Time.timeScale = 0f;
        this.isClear = isClear;
        if (isClear)
        {
            nextButtonText.text = "다음";
        }
        else
        {
            nextButtonText.text = "선택 이동";
        }

        if (GameManager.Instance.Money >= 300)
        {
            pointImages[2].SetActive(true);
        }

        if (GameManager.Instance.Money >= 200)
        {
            pointImages[1].SetActive(true);
        }

        if (GameManager.Instance.Money >= 100)
        {
            pointImages[0].SetActive(true);
        }

        PlayerPrefs.SetInt($"Point{GameManager.Instance.CurrentStage}", GameManager.Instance.Money / 100);

        moneyText.text = $"{GameManager.Instance.Money.ToString()} Points";
        gameObject.SetActive(true);
    }

    public void OnClickRetryBtn()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickNextBtn()
    {
        //Time.timeScale = 1f;
        if (isClear)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene("StageSelectScene");
        }
    }
}
