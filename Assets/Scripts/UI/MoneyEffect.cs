using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    private TextMeshPro moneyText;

    private void Awake()
    {
        moneyText = GetComponent<TextMeshPro>();
    }

    public void ActivateEffect(Vector3 spawnPos, string money)
    {
        moneyText.text = money;

        switch (money.Contains("-"))
        {
            case true:
                moneyText.color = Color.red;
                break;

            case false:
                moneyText.color = Color.green;
                break;
        }

        transform.DOMoveY(spawnPos.y + 1f, 1f).OnComplete(() => Destroy(gameObject));
    }
}
