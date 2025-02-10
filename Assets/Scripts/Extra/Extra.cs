using UnityEngine;

public class Extra : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Remover")
        {
            Destroy(gameObject);
        }
    }
}
