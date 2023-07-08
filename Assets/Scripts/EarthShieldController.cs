using UnityEngine;

public class EarthShieldController : MonoBehaviour {

    public GameObject enemyShipImpact;
    public GameObject enemyShotImpact;
    
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);

        if (other.CompareTag("Enemy"))
        {
            Instantiate(enemyShipImpact, other.transform.position, other.transform.rotation);
        }
        else
        { 
            Instantiate(enemyShotImpact, other.transform.position, other.transform.rotation);
        }        
    }
}
