using UnityEngine;

public class HallLevelDesigner : MonoBehaviour
{
    public float time; // 전체 시간
    float totalTime;

    void Start()
    {
        GetComponent<GuestSpawnManager>().SetParameter(30);

        // GameManager에서 시간 받아오기

        time = GameManager.Instance.StageTimeLimit;
        totalTime = time; // 초기 time 값을 저장
    }

    void Update()
    {
        time -= Time.deltaTime;

        if (time < 0)
        {
            time = 0;
            GetComponent<GuestSpawnManager>().StopGuestSpawn();
        }
        else if (0 <= time && time < totalTime / 5 * 1) // 6구간
        {
            GetComponent<GuestSpawnManager>().SetParameter(55);
        }
        else if (totalTime / 5 * 1 <= time && time < totalTime / 5 * 2) // 5구간
        {
            GetComponent<GuestSpawnManager>().SetParameter(45);
        }
        else if (totalTime / 5 * 2 <= time && time < totalTime / 5 * 3) // 4구간
        {
            GetComponent<GuestSpawnManager>().SetParameter(50);
        }
        else if (totalTime / 5 * 3 <= time && time < totalTime / 5 * 4) // 3구간
        {
            GetComponent<GuestSpawnManager>().SetParameter(55); // 2구간
        }
        else
        {
            GetComponent<GuestSpawnManager>().SetParameter(60); // 1구간
        }
    }
}
