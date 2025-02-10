using TMPro;
using UnityEngine;

public class RecipePopup : MonoBehaviour
{
    public GameObject[] pages;
    public TextMeshProUGUI pageText;
    int index = 0;

    void OnEnable()
    {
        DisplayPageText();
    }

    // 다음 페이지로 넘기는 메서드
    public void NextPage()
    {
        if (index < pages.Length -1)
        {
            index++;
            foreach (var page in pages)
            {
                page.SetActive(false);
            }
            pages[index].SetActive(true);

            DisplayPageText();
        }
    }

    // 이전 페이지로 넘기는 메서드
    public void PreviousPage()
    {
        if (index > 0)
        {
            index--;
            foreach (var page in pages)
            {
                page.SetActive(false);
            }
            pages[index].SetActive(true);

            DisplayPageText();
        }
    }

    // 레시피 팝업창을 활성화 및 비활성화 하는 메서드
    public void ShowPopup()
    {
        // 활성화
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            GameManager.Instance.CanSwitchCharacter = false;
        }
        // 비활성화
        else
        {
            gameObject.SetActive(false);
            GameManager.Instance.CanSwitchCharacter = true;
        }
    }

    // 팝업창 페이지 텍스트 메서드
    public void DisplayPageText()
    {
        pageText.text = (index + 1).ToString() + "/" + pages.Length.ToString();
    }
}
