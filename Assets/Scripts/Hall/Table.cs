using UnityEngine;

public class Table : MonoBehaviour
{
    public bool isDirty;
    public GameObject dirtyPlatePrefab;
    public GameObject table;
    public GameObject seat;

    GameObject dirtyPlate;

    // 빈 접시를 생성하는 메서드
    public void SetDirtyPlate()
    {
        dirtyPlate = Instantiate(dirtyPlatePrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        dirtyPlate.transform.parent = transform;

        isDirty = true;
    }

    // 빈 접시 오브젝트를 넘겨주는 메서드
    public GameObject GetDirtyPlate()
    {
        // 빈 접시가 있다면
        if(dirtyPlate != null)
        {
            GameObject plateToReturn = dirtyPlate;
            dirtyPlate = null;

            isDirty = false;

            return plateToReturn;
        }

        return null;
    }
}
