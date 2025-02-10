using UnityEngine;

public class ObjectDispenser : MonoBehaviour
{
    [SerializeField]
    public GameObject prefab;

    // ÇÁ¸®ÆÕ »ý¼º
    public Transform SpawnObjectAndReturnTransform()
    {
        GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return newObject.transform;
    }
}
