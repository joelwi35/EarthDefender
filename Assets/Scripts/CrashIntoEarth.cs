using UnityEngine;

public class CrashIntoEarth : MonoBehaviour {

    public Vector2 PopulationImpact;
    public GameObject explosion;
    public GameObject playerExplosion;
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

            Destroy(gameObject);

            gameController.ShowGameAssistance(4, "Enemies that crash into earth reduce Earth's population. Don't let humanity perish!");
            /*
            if (gameObject.CompareTag("LaserBolt"))
            {
                Destroy(gameObject);
            }
            */
        }        
    }
}
