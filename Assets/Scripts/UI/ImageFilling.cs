using UnityEngine;
using UnityEngine.UI;

public class ImageFilling : MonoBehaviour
{
    public Image image;
    public float duration;

    float currentAmount;
    float speed;

    private void OnEnable()
    {
        InitializeFill();
        speed = 1.0f / duration;
    }

    void Update()
    {
        if (currentAmount < 1.0f)
        {
            currentAmount += speed * Time.deltaTime;
            image.fillAmount = Mathf.Clamp01(currentAmount);
        }
    }

    public void InitializeFill()
    {
        currentAmount = 0;
        image.fillAmount = 0;
    }
}
