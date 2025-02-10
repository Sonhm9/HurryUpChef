using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtraSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] extras;
    [SerializeField]
    private Transform parentObject;
    private float spawnTime = 0;
    private float spawnCoolTime;

    private float spawnPosX1;
    private float spawnPosX2;
    private float spawnPosZ1;
    private float spawnPosZ2;

    private void Start()
    {
        SetDataBySceneName();
        SetSpawnCoolTime();
    }

    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime > spawnCoolTime)
        {
            SpawnExtra();
            spawnTime = 0;
            SetSpawnCoolTime();
        }
    }

    private void SpawnExtra()
    {
        float positionX = (Random.Range(0, 2) == 0) ? spawnPosX1 : spawnPosX2;
        float positionZ = (Random.Range(0, 2) == 0) ? Random.Range(spawnPosZ1 - 3f, spawnPosZ1 + 3f) : Random.Range(spawnPosZ2 - 3f, spawnPosZ2 + 3f);
        int rotationY = (positionX > 0) ? -90 : 90;

        GameObject extra = Instantiate(extras[Random.Range(0, 6)], new Vector3(positionX, 0.2f, positionZ), Quaternion.Euler(0, rotationY, 0));
        extra.transform.SetParent(parentObject);
    }

    private void SetSpawnCoolTime()
    {
        spawnCoolTime = Random.Range(3.0f, 5.0f);
    }

    private void SetDataBySceneName()
    {
        // 현재 활성화된 씬의 이름 가져오기
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch(currentSceneName)
        {
            case "TutorialScene":
                spawnPosX1 = 19f;
                spawnPosX2 = -22f;
                spawnPosZ1 = 20f;
                spawnPosZ2 = -6f;
                break;
            case "GameScene":
                spawnPosX1 = 21f;
                spawnPosX2 = -27f;
                spawnPosZ1 = 24f;
                spawnPosZ2 = -6f;
                break;
            case "GameScene2":
                spawnPosX1 = 12f;
                spawnPosX2 = -26f;
                spawnPosZ1 = 29f;
                spawnPosZ2 = -6f;
                break;
            case "GameScene3":
                spawnPosX1 = 31f;
                spawnPosX2 = -6f;
                spawnPosZ1 = 24f;
                spawnPosZ2 = -6f;
                break;
        }
    }
}
