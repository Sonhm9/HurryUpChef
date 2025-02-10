using UnityEngine;

public class CuttableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject ingredientPrefab;
    [SerializeField]
    private int reuseCount;

    private GameObject uncuttedObject;
    private GameObject cuttedObject;

    private bool isCutted = false;

    void Start()
    {
        uncuttedObject = transform.GetChild(0).gameObject;
        cuttedObject = transform.GetChild(1).gameObject;
    }

    public bool IsCuttedObject()
    {
        return isCutted;
    }

    public void SetIsCuttedBoolean(bool b)
    {
        isCutted = b;
    }

    public Transform SpawnObjectAndReturnTransform()
    {
        GameObject newObject = Instantiate(ingredientPrefab, Vector3.zero, Quaternion.identity);
        SetReuseCount(reuseCount - 1);
        return newObject.transform;
    }

    private void SetReuseCount(int num) 
    { 
        reuseCount = num;

        if (reuseCount == 0)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}