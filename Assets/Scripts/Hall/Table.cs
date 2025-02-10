using UnityEngine;

public class Table : MonoBehaviour
{
    public bool isDirty;
    public GameObject dirtyPlatePrefab;
    public GameObject table;
    public GameObject seat;

    GameObject dirtyPlate;

    // �� ���ø� �����ϴ� �޼���
    public void SetDirtyPlate()
    {
        dirtyPlate = Instantiate(dirtyPlatePrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        dirtyPlate.transform.parent = transform;

        isDirty = true;
    }

    // �� ���� ������Ʈ�� �Ѱ��ִ� �޼���
    public GameObject GetDirtyPlate()
    {
        // �� ���ð� �ִٸ�
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
