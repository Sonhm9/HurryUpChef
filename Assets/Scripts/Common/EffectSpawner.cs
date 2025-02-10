using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public static EffectSpawner Instance { get; private set; }

    [SerializeField]
    private MoneyEffect moneyEffectPrefab;
    [SerializeField]
    private AngryEffect angryEffectPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnMoneyEffect(Vector3 spawnPos, string money)
    {
        SoundManager.Instance.PlaySfx(SFX.GetPoint);
        MoneyEffect moneyEffect = Instantiate(moneyEffectPrefab);
        moneyEffect.transform.position = spawnPos;
        moneyEffect.transform.LookAt(Camera.main.transform);
        moneyEffect.ActivateEffect(spawnPos, money);
    }

    public void SpawnAngryEffect(Vector3 spawnPos)
    {
        AngryEffect angryEffect = Instantiate(angryEffectPrefab);
        angryEffect.transform.position = spawnPos;
        angryEffect.transform.LookAt(Camera.main.transform);
        angryEffect.ActivateEffect(spawnPos);
    }
}
