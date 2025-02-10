using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string[] sceneAssets;

    private PanelSnappingHandler handler;

    void Start()
    {
        handler = GetComponent<PanelSnappingHandler>();
    }

    public void OnClickGameStartButton()
    {
        int index = handler.GetSceneIndexNum();
        string sceneName = sceneAssets[index];
        SceneManager.LoadScene(sceneName);
    }

    public void OnClickTitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}