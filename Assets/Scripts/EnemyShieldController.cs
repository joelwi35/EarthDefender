using System.Collections;
using UnityEngine;

public class EnemyShieldController : MonoBehaviour {

    public GameObject playerLightningBoltImpact;
    public GameObject playerLaserBoltImpact;
    public GameObject playerRocketImpact;
    public GameObject nonPlayerImpact;
    
    public GameObject motherShip;
    public GameObject sharpRing;
    public GameObject particles;
    public GameObject glowRing;

    public AudioSource AudioSource { get; private set; }

    ParticleSystem.MainModule shield;    

    public float hitPoints;
    private float currentHitPoints;
    private GameController gameController;

    private void Start()
    {
        currentHitPoints = hitPoints;

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find game controller script");
        }

        if (gameObject.CompareTag("MotherShip"))
        {
            StartCoroutine(UpdateMotherShipStatus());
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MotherShip"))
        {
            gameController.ShowGameAssistance(5, "MotherShip shield are strong but will eventually overload with sustained lightning fire.");
        }
        
        if (other.CompareTag("LongRangeSensor"))
        {
            return;
        }

        if (!other.CompareTag("Earth") && !other.CompareTag("MotherShip"))
        {
            Debug.Log("ShieldImpact destroying " + other.tag);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("PlayerLightningBolt"))
        {
            Instantiate(playerLightningBoltImpact, other.transform.position, other.transform.rotation);
            currentHitPoints = Mathf.Max(0, currentHitPoints - 1);
            if (currentHitPoints == 0)
            {
                Debug.Log("ShieldImpact destroying " + gameObject.tag + " due to 0 hit points");
                Destroy(gameObject); // shield is down!
            }
        }
        else if (other.CompareTag("PlayerLaserBolt"))
        {
            gameController.ShowGameAssistance(2, "Enemy shields can only be affected by purple lightning fire.");

            Instantiate(playerLaserBoltImpact, other.transform.position, other.transform.rotation);
        }
        else if (other.CompareTag("PlayerRockets"))
        {
            gameController.ShowGameAssistance(2, "Enemy shields can only be affected by purple lightning fire.");
            Instantiate(playerRocketImpact, other.transform.position, other.transform.rotation);
        }
        else
        {
            Instantiate(nonPlayerImpact, other.transform.position, other.transform.rotation);            
        }
    }

    IEnumerator UpdateMotherShipStatus()
    {        
        while (true)
        {
            yield return new WaitForSeconds(1.0f); // update once per second

            float hitPointsPercentLeft = currentHitPoints / hitPoints;
            
            Debug.Log(hitPointsPercentLeft);

            ParticleSystem.MainModule coreColor = GetComponent<ParticleSystem>().main;
            ParticleSystem.MainModule sharpRingColor = sharpRing.GetComponent<ParticleSystem>().main;
            ParticleSystem.MainModule particleColor = particles.GetComponent<ParticleSystem>().main;
            ParticleSystem.MainModule glowRingColor = glowRing.GetComponent<ParticleSystem>().main;

            coreColor.startColor = new ParticleSystem.MinMaxGradient(new Color(1, hitPointsPercentLeft, hitPointsPercentLeft, 1));            
            sharpRingColor.startColor = new ParticleSystem.MinMaxGradient(new Color(1, hitPointsPercentLeft, hitPointsPercentLeft, 1));
            particleColor.startColor = new ParticleSystem.MinMaxGradient(new Color(1, hitPointsPercentLeft, hitPointsPercentLeft, 1));
            glowRingColor.startColor = new ParticleSystem.MinMaxGradient(new Color(1, hitPointsPercentLeft, hitPointsPercentLeft, 1));
        }
    }    
}
