using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("DestroyByBoundary: " + other.tag);
        Destroy(other.gameObject);
    }
}