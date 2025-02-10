using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        transform.localScale = Vector3.one * (distance * 0.1f * 0.3f);
        transform.rotation = Camera.main.transform.rotation;
    }
}
