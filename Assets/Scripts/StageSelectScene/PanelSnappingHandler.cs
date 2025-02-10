using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PanelSnappingHandler : MonoBehaviour
{
    public ScrollRect scrollRect; // Scroll View의 ScrollRect 컴포넌트
    public RectTransform content; // Scroll View의 Content
    public RectTransform mainPanel;
    public RectTransform[] panels; // 개별 스테이지 패널들 (자식 RectTransform 배열)
    public float snapSpeed = 10f; // 스냅 이동 속도

    private bool isSnapping = false; // 스냅 중인지 여부
    private float targetPosX; // 스냅 목표 위치
    private float[] panelsPosX;

    private int panelIndex = 0;
    private float offset;
    private Vector2 mouseDownPoint; // 마우스 버튼을 눌렀을 때의 포인트
    private Vector2 mouseUpPoint;   // 마우스 버튼을 뗐을 때의 점의 포인트
    private float screenWidth;

    void Start()
    {
        Canvas.ForceUpdateCanvases();
        screenWidth = Screen.width;
        offset = screenWidth * 0.2f; // 해상도에 비례하여 이동 거리 조정
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

        // 지정된 패널을 화면 중앙으로 이동
        if (isSnapping)
        {
            content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, new Vector2(-targetPosX , 0), snapSpeed * Time.deltaTime);

            // 목표 위치에 거의 도달하면 스냅 종료
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