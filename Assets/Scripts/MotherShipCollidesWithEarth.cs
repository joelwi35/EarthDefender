using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipCollidesWithEarth : MonoBehaviour
{
    public int numberEarthExplosions;
    public Vector2 PopulationImpact;
    public GameObject explosion;
    public GameObject playerExplosion;
    public GameObject playerExplosion2;
    public GameObject playerExplosion3;
    private GameController gameController;

    void Start()
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
        if (other.tag == "Earth")
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);

            if (gameController != null)
            {
                gameController.ReducePopulation((long)PopulationImpact.x, (long)PopulationImpact.y);
            }

            if (gameObject.CompareTag("LaserBolt"))
            {
                Destroy(gameObject);
            }

            if (gameObject.CompareTag("MotherShip"))
            {                
                StartCoroutine(DestroyEarth(other.gameObject));
            }
        }
    }

    IEnumerator DestroyEarth(GameObject earth)
    {
        Quaternion earthRotiation = earth.transform.rotation;
        Vector3 earthPosition = earth.transform.position;
        
        for (int i = 0; i < numberEarthExplosions; i++)
        {
            Vector3 explosionOffset = earthPosition;

            explosionOffset.x += Random.Range(-15.0f, -5.0f);
            explosionOffset.y += Random.Range(-2.0f, 2.0f);
            explosionOffset.z += Random.Range(-5.0f, 5.0f);

            int explosionType = (int)Random.Range(0, 4);
            switch (explosionType)
            {
                case 0:
                    Instantiate(playerExplosion, explosionOffset, earthRotiation);
                    break;

                case 1:
                    Instantiate(playerExplosion2, explosionOffset, earthRotiation);
                    break;

                case 2:
                    Instantiate(playerExplosion3, explosionOffset, earthRotiation);
                    break;
            }            

            yield return new WaitForSeconds(0.1f); // pause between explosions
        }

        GameObject[] earthParts = GameObject.FindGameObjectsWithTag("Earth");

        foreach(GameObject part in earthParts)
        {
            Destroy(part);
        }

        Instantiate(playerExplosion, earthPosition, earthRotiation);
    }
}
