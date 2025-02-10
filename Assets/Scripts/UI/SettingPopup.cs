using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;

    private void SaveVolumes()
    {
        PlayerPrefs.SetFloat("BgmVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void Initialize()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
        bgmSlider.value = PlayerPrefs.GetFloat("BgmVolume", 0.5f);

        SoundManager.Instance.SetVolume(bgmSlider.value, sfxSlider.value);
    }

    public void ShowPopup()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void HidePopup()
    {
        SoundManager.Instance.PlaySfx(SFX.Click);
        Time.timeScale = 1f;
        SaveVolumes();
        gameObject.SetActive(false);
    }

    public void OnChangedVolume()
    {
        SoundManager.Instance.SetVolume(bgmSlider.value, sfxSlider.value);
    }

    public void OnClickRetryBtn()
    {
        Time.timeScale = 1f;
        SaveVolumes();
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickMainBtn()
    {
        Time.timeScale = 1f;
        SaveVolumes();
        SceneManager.LoadScene("TitleScene");
    }
}
