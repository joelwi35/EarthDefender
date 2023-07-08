using UnityEngine;

public class TriggerPowerUp : MonoBehaviour {

    public GameObject activeUpgrade;
    public GameObject explosion;
    public float boost;
    private GameController gameController;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find game controller script");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerLaserBolt") || other.CompareTag("PlayerRockets") || other.CompareTag("PlayerLightningBolt") || other.CompareTag("Earth"))
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            Debug.Log("Triggering power-up!");

            if (!other.CompareTag("Earth"))
            {
                Destroy(other.gameObject);
            }

            Destroy(gameObject);

            gameController.ShowGameAssistance(3, "Power up cubes temporarily increase the same colored weapon rate.");

            Instantiate(activeUpgrade, transform.position, transform.rotation); // invisible object that disappears in a few seconds
        }
    }
}
