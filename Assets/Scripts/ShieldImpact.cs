using UnityEngine;

public class ShieldImpact : MonoBehaviour {

    private bool shielded;

    private void Start()
    {
        shielded = true;    
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ShieldImpact: " + other.tag);

        if (other.CompareTag("Boundary") || other.CompareTag("Earth") || other.CompareTag("LongRangeSensor"))
        {
            return;
        }

        if (!other.CompareTag("MotherShip"))
        {
            Debug.Log("ShieldImpact destroying " + other.tag);
            Destroy(other.gameObject);
        }

        bool isLightningShot = other.CompareTag("PlayerLightningBolt");

        if (shielded)
        {
            if (isLightningShot)
            {
                shielded = false;                
            }

            return;
        }

        if (!gameObject.CompareTag("MotherShip"))
        {
            Debug.Log("ShieldImpact destroying " + gameObject.tag);
            Destroy(gameObject);
        }
     }
}
