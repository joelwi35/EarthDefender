using System.Collections;
using UnityEngine;

public class ViperMover : MonoBehaviour {

    public float minDistanceToEarth;
    public float maxDistancetoEarth;
    public float maxMoveDistance;

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

        targetPosition = Vector3.zero; // initially, all vipers bee-line for Earth
        insideDonut = false;        
    }
    
    IEnumerator MoveToNewFiringPosition()
    {
        while (true)
        {
            float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTargetPosition == 0)
            {
                // arrived at new target position, so hold for a little bit before picking new position
                yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));

                float distanceToEarth = Vector3.Distance(transform.position, Vector3.zero);
                if (distanceToEarth > maxDistancetoEarth)
                {
                    targetPosition = Vector3.zero; // if object moves out of donut, force it to move directly back to the origin once again (prevents random drift away from origin)
                }
                else
                {
                    // pick new Firing position within the donut
                    while (true)
                    {
                        // randomly generate new positions in the donut until a valid one is generated
                        Vector3 potentialNewPosition = new Vector3((float)Random.Range(-maxDistancetoEarth, maxDistancetoEarth), 0, (float)Random.Range(-maxDistancetoEarth, maxDistancetoEarth));

                        distanceToEarth = Vector3.Distance(potentialNewPosition, Vector3.zero);
                        if (distanceToEarth > minDistanceToEarth && distanceToEarth < maxDistancetoEarth)
                        {
                            float distanceFromCurrentPosition = Vector3.Distance(potentialNewPosition, transform.position);
                            if (distanceFromCurrentPosition < maxMoveDistance)
                            {
                                targetPosition = potentialNewPosition;
                                break;
                            }
                        }
                    }
                }
            }

            yield return new WaitForSeconds(2); // wait a bit before re-testing new position
        }
    }

    void Update()
    {
        Vector3 lookPos = transform.position - Vector3.zero; // - transform.position;            

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);        

        float distanceToEarth = Vector3.Distance(transform.position, Vector3.zero);        
        if (distanceToEarth < maxDistancetoEarth && !insideDonut)
        {
            targetPosition = transform.position; // this forces the co-routine to immediately pick a new target position
            insideDonut = true;
            StartCoroutine(MoveToNewFiringPosition());
        }
    }
}
