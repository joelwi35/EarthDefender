using System.Collections;
using UnityEngine;

public class AWingMover : MonoBehaviour {

    public Vector2 warpDelay;
    public float minDistanceToEarth;
    public float maxDistancetoEarth;    

    public GameObject warpOut;
    public GameObject warpIn;

    public Vector2 maneuverWait;
    public float speed;
    public int rotationDamping;

    private Vector3 targetPosition;
    private float currentSpeed;
    private Rigidbody rb;
    
    private bool insideDonut;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.z;

        targetPosition = Vector3.zero; // initially, all enemies bee-line for Earth
        insideDonut = false;

        StartCoroutine(WarpPosition());
        //InvokeRepeating("WarpPosition", Random.Range(warpDelay.x, warpDelay.y), Random.Range(warpDelay.x, warpDelay.y));
    }

    IEnumerator WarpPosition()
    {
        while (true)
        {
            Instantiate(warpOut, transform.position, transform.rotation);
            
            yield return new WaitForSeconds(0.1f);

            Vector3 currentScale = transform.localScale;
            transform.localScale *= 0.001f;

            // pick new warp position within the space donut around earth
            while (true)
            {
                // randomly generate new positions in the donut until a valid one is generated
                Vector3 potentialNewPosition = new Vector3((float)Random.Range(-maxDistancetoEarth, maxDistancetoEarth), 0, (float)Random.Range(-maxDistancetoEarth, maxDistancetoEarth));

                float distanceToEarth = Vector3.Distance(potentialNewPosition, Vector3.zero);
                if (distanceToEarth > minDistanceToEarth && distanceToEarth < maxDistancetoEarth)
                {                    
                    Instantiate(warpIn, potentialNewPosition, transform.rotation);                    
                    yield return new WaitForSeconds(0.3f);

                    transform.position = potentialNewPosition;
                    transform.localScale = currentScale;
                    break;
                }
            }

            //Vector3 lookPos = Vector3.zero - transform.position; // - transform.position;            

            //var rotation = Quaternion.LookRotation(lookPos);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);

            yield return new WaitForSeconds(Random.Range(warpDelay.x, warpDelay.y));
        }
    }

    void Update()
    {
        Vector3 lookPos = Vector3.zero - transform.position; // - transform.position;            

        float step = speed * Time.deltaTime;
        
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
    }
}
