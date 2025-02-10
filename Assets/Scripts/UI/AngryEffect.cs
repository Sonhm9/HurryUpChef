using DG.Tweening;
using UnityEngine;

public class AngryEffect : MonoBehaviour
{
    public void ActivateEffect(Vector3 spawnPos)
    {
        transform.DOMoveY(spawnPos.y + 1f, 1f).OnComplete(() => Destroy(gameObject));
    }
}
