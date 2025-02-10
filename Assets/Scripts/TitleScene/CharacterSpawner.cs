using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characterPrefabs;

    private float spawnTime = 3.5f;
    private int spawnIndex = 0;

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime > 4)
        {
            SpawnCharacter();
            spawnTime = 0;
        }
    }

    private void SpawnCharacter()
    {
        GameObject characterPrefab = Instantiate(characterPrefabs[spawnIndex % characterPrefabs.Length]);
        spawnIndex++;
    }
}
