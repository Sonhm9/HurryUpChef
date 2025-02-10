using UnityEngine;

public class ObjectDispenser : MonoBehaviour
{
    [SerializeField]
    public GameObject prefab;

    // ������ ����
    public Transform SpawnObjectAndReturnTransform()
    {
        GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return newObject.transform;
    }
}
