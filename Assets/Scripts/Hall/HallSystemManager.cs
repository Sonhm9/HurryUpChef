using System.Collections.Generic;
using UnityEngine;

public class HallSystemManager : MonoBehaviour
{
    #region Singleton
    static HallSystemManager instance;
    static public HallSystemManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion
    private void Awake()
    {
        // �ν��Ͻ��� �����Ѵٸ�
        if (instance != null)
        {
            Destroy(gameObject);
        }

        // �ν��Ͻ��� ó�� �����ȴٸ�
        else
        {
            instance = this;

            //DontDestroyOnLoad(gameObject);
        }
    }
    public List<GameObject> emptySeats; // ����ִ� �¼� ����Ʈ
    public List<GameObject> fullSeats; // ����ִ� �¼� ����Ʈ

    public List<GuestController> guestQueue; // �մ� ��⿭

    public Vector3 orderPosition; // �ֹ� ������
    public float spacing = 1.5f; // ��⿭ ����
    public bool orderDirection;

    public int angry = 0; // �г� ��ġ

    bool[] isEmptys; // ����ִ��� ���� ����Ʈ
    bool[] isDirtys; // �������� ���� ����Ʈ

    // ����ִ� ��ġ�� ã�� �޼���
    public void GetSeatObject(out GameObject seat)
    {
        int num = Random.Range(0, emptySeats.Count);
        seat = emptySeats[num];

        fullSeats.Add(seat);
        emptySeats.Remove(seat);
    }

    // �г� ��� �޼���
    public void RisingAngry()
    {
        angry++;
    }

    // ��⿭�� ��ġ�� ��ȯ�ϴ� �޼���
    public Vector3 GetQueuePosition(int index)
    {
        if (orderDirection)
        {
            Vector3 targetPosition = orderPosition - new Vector3(spacing * index, 0, 0);

            return targetPosition;

        }
        else
        {
            Vector3 targetPosition = orderPosition - new Vector3(-spacing * index, 0, 0);

            return targetPosition;
        }
    }

    // ��⿭�� ������ ��ȯ�ϴ� �޼���
    public Vector3 GetQueueRotation()
    {
        if (orderDirection)
        {
            return Vector3.right;
        }
        else
        {
            return Vector3.left;
        }
    }

    // �մ��� �ֹ��� ��ȯ�ϴ� �޼���
    public RecipeData GetTakeOrder()
    {
        // �մ��� ���� �� null ��ȯ
        if(guestQueue.Count == 0)
        {
            return null;
        }

        // �ڸ��� ���� �� �ֹ����� ����
        if(emptySeats.Count == 0)
        {
            return null;
        }

        // �մ��� �ֹ��� ������ ��
        if (guestQueue[0].currentState == GuestController.GuestState.Ordering)
        {
            // �ֹ��� �ް� ������ ������ ��ȯ
            guestQueue[0].isOrderClear = true;

            return guestQueue[0].recipeData;
        }
        // �մ��� �ֹ� �� ���°� �ƴ� �� null ��ȯ
        else
        {
            return null;
        }
    }
}
