using UnityEngine;

public class StagePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pointImages;
    [SerializeField]
    private int currentStage;

    void Start()
    {
        int point = PlayerPrefs.GetInt($"Point{currentStage}", 0);
        for (int i = 0; i < point; i++)
        {
            pointImages[i].SetActive(true);
        }
    }
}
