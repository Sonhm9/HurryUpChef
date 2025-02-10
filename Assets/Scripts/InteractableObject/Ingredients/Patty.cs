using UnityEngine;

public class Patty : MonoBehaviour
{
    private float roastTime = 0f; // 패티가 구워진 시간을 저장하는 변수
    private bool isOnStove = false; // 패티가 Stove 위에 있는지 체크하는 변수
    private int currentState = 0; // 0: uncooked, 1: cooked, 2: trash
    private int audioIndex = -1;

    private GameObject uncookedPatty;
    private GameObject cookedPatty;
    private GameObject trashPatty;

    void Start()
    {
        uncookedPatty = transform.GetChild(0).gameObject;
        cookedPatty = transform.GetChild(1).gameObject;
        trashPatty = transform.GetChild(2).gameObject;
    }

    void Update()
    {
        if (!isOnStove)
        {
            return;
        }

        roastTime += Time.deltaTime; 
        UpdatePattyState();
    }

    public bool IsPattyOnStove() 
    { 
        return isOnStove; 
    }

    public void SetIsOnStove(bool b) 
    {
        isOnStove = b;

        if (isOnStove)
        {
            audioIndex = SoundManager.Instance.PlaySfx(SFX.Cook);
        }
        else
        {
            SoundManager.Instance.StopSfx(audioIndex);
        }
    }

    public int GetCurrentState()
    {
        return currentState;
    }

    private void UpdatePattyState()
    {
        if (currentState == 0 && roastTime > 20f)
        {
            ChangePattyState(1); // cooked
        }
        else if (currentState == 1 && roastTime > 40f)
        {
            ChangePattyState(2); // trash
        }
    }

    public void ChangePattyState(int newState)
    {
        if (newState == currentState)
        {
            return;
        }

        currentState = newState;

        switch (currentState)
        {
            case 1:
                uncookedPatty.SetActive(false);
                cookedPatty.SetActive(true);

                break;

            case 2:
                cookedPatty.SetActive(false);
                trashPatty.SetActive(true);
                OnSmokeByState();

                break;
        }
    }

    public void OnSmokeByState()
    {
        switch (currentState)
        {
            case 0:
                transform.parent.GetChild(1).gameObject.SetActive(true);
                transform.parent.GetChild(2).gameObject.SetActive(false);
                break;

            case 1:
                transform.parent.GetChild(1).gameObject.SetActive(true);
                transform.parent.GetChild(2).gameObject.SetActive(false);
                break;

            case 2:
                transform.parent.GetChild(1).gameObject.SetActive(false);
                transform.parent.GetChild(2).gameObject.SetActive(true);
                break;

            default:
                transform.parent.GetChild(1).gameObject.SetActive(false);
                transform.parent.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }
}
