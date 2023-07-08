using System.Collections;
using UnityEngine;

public class MoveDreadnaught : MonoBehaviour
{
    public float dodge;
    public float smoothing;
    public float tilt;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;

    private float targetManeuver;
    public float speed;
    public int rotationDamping;

    private Vector3 target;
    private float currentSpeed;
    private Rigidbody rb;
    private GameController gameController;

    // Use this for initialization
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
        
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.z;

        target = Vector3.zero;
        StartCoroutine(Evade());
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            target = Vector3.zero;
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));

            Vector3 currentPosition = transform.position;
            
            currentPosition.x += Random.Range(-1000, 1000);
            currentPosition.z += Random.Range(-1000, 1000);

            target = currentPosition;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }

    void Update()
    {        
        float step = speed * Time.deltaTime;

        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
        
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}
