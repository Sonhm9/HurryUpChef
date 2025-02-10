using UnityEngine;

public class HallLevelDesigner : MonoBehaviour
{
    public float time; // ��ü �ð�
    float totalTime;

    void Start()
    {
        GetComponent<GuestSpawnManager>().SetParameter(30);

        // GameManager���� �ð� �޾ƿ���

        time = GameManager.Instance.StageTimeLimit;
        totalTime = time; // �ʱ� time ���� ����
    }

    void Update()
    {
        time -= Time.deltaTime;

        if (time < 0)
        {
            time = 0;
            GetComponent<GuestSpawnManager>().StopGuestSpawn();
        }
        else if (0 <= time && time < totalTime / 5 * 1) // 6����
        {
            GetComponent<GuestSpawnManager>().SetParameter(55);
        }
        else if (totalTime / 5 * 1 <= time && time < totalTime / 5 * 2) // 5����
        {
            GetComponent<GuestSpawnManager>().SetParameter(45);
        }
        else if (totalTime / 5 * 2 <= time && time < totalTime / 5 * 3) // 4����
        {
            GetComponent<GuestSpawnManager>().SetParameter(50);
        }
        else if (totalTime / 5 * 3 <= time && time < totalTime / 5 * 4) // 3����
        {
            GetComponent<GuestSpawnManager>().SetParameter(55); // 2����
        }
        else
        {
            GetComponent<GuestSpawnManager>().SetParameter(60); // 1����
        }
    }
}
