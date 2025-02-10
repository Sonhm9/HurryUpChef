using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePanel : MonoBehaviour
{
    [SerializeField]
    private SettingPopup settingPanel;

    private void Start()
    {
        //로컬에 저장 된 볼륨 설정을 적용하기 위한 초기화
        settingPanel.Initialize();
        SoundManager.Instance.ChangeBgm(BGM.Title);
        SoundManager.Instance.StopSfx();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            PlayerPrefs.DeleteKey("IsFirstTutorial");
        }
    }

    public void GotoStageSelectPressed()
    {
        SoundManager.Instance.PlaySfx(SFX.Click);
        if (PlayerPrefs.GetString("IsFirstTutorial", "True") == "True")
        {
            SceneManager.LoadScene("TutorialScene");
        }
        else
        {
            SceneManager.LoadScene("StageSelectScene");
        }
    }

    public void SettingPressed()
    {
        Time.timeScale = 0;
        SoundManager.Instance.PlaySfx(SFX.Click);
        settingPanel.gameObject.SetActive(true);
    }

    public void QuitPressed()
    {
        SoundManager.Instance.PlaySfx(SFX.Click);
        Application.Quit();
    }
}
