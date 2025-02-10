using UnityEngine;

public enum BGM
{
    Title,
    Game,
}

public enum SFX
{
    Click,
    Cook,
    Cut,
    GameClear,
    GetPoint,
    MenuCorrect,
    Polish,
    PutDown,
    Trashcan,
    Walk,
    GameOver,
    PickUp,
    FemaleNod,
    FemaleSatisfaction,
    FemaleSigh,
    FemaleThroat,
    MaleNod,
    MaleSatisfaction,
    MaleSigh,
    MaleThroat
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private AudioSource bgmAudioSource;
    [SerializeField]
    private AudioSource[] sfxAudioSources;
    [SerializeField]
    private AudioClip[] bgmClips;
    [SerializeField]
    private AudioClip[] sfxClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeBgm(BGM bgm)
    {
        bgmAudioSource.clip = bgmClips[(int)bgm];
        bgmAudioSource.Play();
    }

    public int PlaySfx(SFX sfx, ulong delay = 0)
    {
        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
            if (!sfxAudioSources[i].isPlaying)
            {
                sfxAudioSources[i].clip = sfxClips[(int)sfx];
                sfxAudioSources[i].Play(delay);

                return i + 1;
            }
        }

        return -1;
    }

    public void StopSfx(int audioNumber = 0)
    {
        if (audioNumber == 0)
        {
            foreach (AudioSource audio in sfxAudioSources)
            {
                audio.Stop();
            }
        }
        else
        {
            sfxAudioSources[audioNumber - 1].Stop();
        }
    }

    public void SetVolume(float bgmVolume, float sfxVolume)
    {
        bgmAudioSource.volume = bgmVolume;

        foreach (AudioSource audio in sfxAudioSources)
        {
            audio.volume = sfxVolume;
        }
    }
}
