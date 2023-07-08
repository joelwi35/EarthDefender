using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("DestroyByContact " + other.gameObject.tag);
        if (other.CompareTag("Boundary") || other.CompareTag("PlayerLightningBolt"))
        {
            return;
        }        

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        Debug.Log("Destroying " + other.gameObject.tag);

        if (!other.CompareTag("Earth") && !other.CompareTag("MotherShip") && !other.CompareTag("LongRangeSensor"))
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject); // this is destroying the asteroids, but not the vipers...
    }
}
