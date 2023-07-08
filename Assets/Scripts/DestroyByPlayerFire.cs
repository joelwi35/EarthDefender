using System.Collections;
using UnityEngine;

public class DestroyByPlayerFire : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public GameObject[] powerUpDrops;
    public GameObject shield;
    public float dropProbability;
    public string vulnerableToPlayerFireType;

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
        if (shield == null)
        {
            if (other.CompareTag("PlayerRockets") || other.CompareTag("PlayerLaserBolt") || other.CompareTag("PlayerLightningBolt"))
            {
                if (explosion != null)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                }

                if (other.gameObject.CompareTag(vulnerableToPlayerFireType))
                {
                    if (gameObject.CompareTag("MotherShip"))
                    {
                        StartCoroutine(DestroyMothership(gameObject));
                    }
                    else
                    {
                        Destroy(gameObject);
                    }

                    float powerUpRoll = Random.value;
                    if (powerUpRoll <= dropProbability)
                    {
                        GameObject powerUp = powerUpDrops[Random.Range(0, powerUpDrops.Length)];
                        Instantiate(powerUp, gameObject.transform.position, gameObject.transform.rotation);
                    }
                }
                else
                {
                    gameController.ShowGameAssistance(1, "Some enememies can only be destroyed by a specific weapon type.");
                }

                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator DestroyMothership(GameObject gameObject)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 explosionOffset = gameObject.transform.position;

            explosionOffset.x += Random.Range(-15.0f, 15.0f);
            explosionOffset.y += Random.Range(-2.0f, 2.0f);
            explosionOffset.z += Random.Range(-15.0f, 15.0f);

            if (Random.Range(0, 2) == 1)
            {
                Instantiate(explosion, explosionOffset, gameObject.transform.rotation);
            }
            else
            {
                Instantiate(playerExplosion, explosionOffset, gameObject.transform.rotation);
            }

            yield return new WaitForSeconds(0.1f); // pause between explosions
        }

        GameObject[] earthParts = GameObject.FindGameObjectsWithTag("MotherShip");
        foreach (GameObject part in earthParts)
        {
            Destroy(part);
        }

        if (gameController != null)
        {
            gameController.YouWin();
        }
    }
}
