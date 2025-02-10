using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRequireTo : MonoBehaviour
{
    public Sprite[] displayImage;
    public TextMeshProUGUI recipeText;
    public GameObject recipeInfo;
    
    public void DisplayOrder()
    {
        GetComponent<Image>().sprite = displayImage[0];
    }

    public void DisplayDirtyPlate()
    {
        GetComponent<Image>().sprite = displayImage[1];
    }

    public void DisplayFood(Sprite sprite, string text)
    {
        GetComponent<Image>().sprite = sprite;
        recipeText.text = text;
    }
}
