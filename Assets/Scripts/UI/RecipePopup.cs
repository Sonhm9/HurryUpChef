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

    // ���� �������� �ѱ�� �޼���
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

    // ���� �������� �ѱ�� �޼���
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

    // ������ �˾�â�� Ȱ��ȭ �� ��Ȱ��ȭ �ϴ� �޼���
    public void ShowPopup()
    {
        // Ȱ��ȭ
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            GameManager.Instance.CanSwitchCharacter = false;
        }
        // ��Ȱ��ȭ
        else
        {
            gameObject.SetActive(false);
            GameManager.Instance.CanSwitchCharacter = true;
        }
    }

    // �˾�â ������ �ؽ�Ʈ �޼���
    public void DisplayPageText()
    {
        pageText.text = (index + 1).ToString() + "/" + pages.Length.ToString();
    }
}
