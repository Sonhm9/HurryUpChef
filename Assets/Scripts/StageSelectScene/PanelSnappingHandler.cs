using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PanelSnappingHandler : MonoBehaviour
{
    public ScrollRect scrollRect; // Scroll View�� ScrollRect ������Ʈ
    public RectTransform content; // Scroll View�� Content
    public RectTransform mainPanel;
    public RectTransform[] panels; // ���� �������� �гε� (�ڽ� RectTransform �迭)
    public float snapSpeed = 10f; // ���� �̵� �ӵ�

    private bool isSnapping = false; // ���� ������ ����
    private float targetPosX; // ���� ��ǥ ��ġ
    private float[] panelsPosX;

    private int panelIndex = 0;
    private float offset;
    private Vector2 mouseDownPoint; // ���콺 ��ư�� ������ ���� ����Ʈ
    private Vector2 mouseUpPoint;   // ���콺 ��ư�� ���� ���� ���� ����Ʈ
    private float screenWidth;

    void Start()
    {
        Canvas.ForceUpdateCanvases();
        screenWidth = Screen.width;
        offset = screenWidth * 0.2f; // �ػ󵵿� ����Ͽ� �̵� �Ÿ� ����
        panelsPosX = new float[panels.Length];

        for (int i = 0; i < panels.Length; i++)
        {
            panelsPosX[i] = panels[i].position.x;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPoint = Input.mousePosition;
            isSnapping = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseUpPoint = Input.mousePosition;
            SnapToNextPanel();
        }

        // ������ �г��� ȭ�� �߾����� �̵�
        if (isSnapping)
        {
            content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, new Vector2(-targetPosX , 0), snapSpeed * Time.deltaTime);

            // ��ǥ ��ġ�� ���� �����ϸ� ���� ����
            if (Mathf.Abs(content.anchoredPosition.x - targetPosX) < 0.1f)
            {
                content.anchoredPosition = new Vector2(-targetPosX, 0);
                isSnapping = false;
            }
        }
    }

    private void SnapToNextPanel()
    {
        float moveDistance = mouseDownPoint.x - mouseUpPoint.x;

        if (moveDistance > offset && panelIndex + 1 < panels.Length)
        {
            panelIndex += 1;
        }
        else if (moveDistance < -offset && panelIndex - 1 >= 0)
        {
            panelIndex -= 1;
        }

        targetPosX = panelIndex * 1920f;
        isSnapping = true;
    }

    public void OnClickRightButton()
    {
        if (panelIndex + 1 < panels.Length)
        {
            panelIndex += 1;
        }

        targetPosX = panelIndex * 1920f;
        isSnapping = true;
    }

    public void OnClickLeftButton()
    {
        if (panelIndex - 1 >= 0)
        {
            panelIndex -= 1;
        }

        targetPosX = panelIndex * 1920f;
        isSnapping = true;
    }

    public int GetSceneIndexNum() 
    { 
        return panelIndex; 
    }
}