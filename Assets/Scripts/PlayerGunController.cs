using UnityEngine;

public class PlayerGunController : MonoBehaviour {

    float clockwise = -1.0f;
    const int speed = 3;
    
    public int shotSpeed; // all shots travel at the same speed... can/should I make this different per weapon and alterable as a power up ability?
    public Transform shotSpawn;
    public GameObject[] shots;

    private float[] nextFire = new float[3];
    private Rigidbody rb;
    private GameController gameController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("PlayerGunController Cannot find game controller script");
        }
    }

    // Update is called once per frame
    void Update () {

        float h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            if (h > 0)
            {
                transform.RotateAround(Vector3.zero, Vector3.up, speed);
            }
            else
            {
                transform.RotateAround(Vector3.zero, Vector3.up, -speed);
            }            
        }

        GameObject projectile = null;

        int fireIndex = 0;
        float shotFrequency = 0;
        bool shotFired = false;

        if (Input.GetButton("Fire1"))
        {
            fireIndex = 0;
            shotFrequency = gameController.GetLaserShotSpeed();
            shotFired = true;
        }
        else if (Input.GetButton("Fire2"))
        {
            fireIndex = 1;
            shotFrequency = gameController.GetRocketShotSpeed();
            shotFired = true;
        }
        else if (Input.GetButton("Fire3"))
        {
            fireIndex = 2;
            shotFrequency = gameController.GetLightningShotSpeed();
            shotFired = true;
        }
        
        if (shotFired && Time.time > nextFire[fireIndex])
        {
            nextFire[fireIndex] = Time.time + shotFrequency;
            projectile = Instantiate(shots[fireIndex], shotSpawn.position, shotSpawn.rotation) as GameObject;

            if (projectile != null)
            {
                // Add force 90 degrees from the orientation of the orbital cannon so that the shot goes in the direction that it is pointing
                projectile.transform.Rotate(0, 90, 0);
                projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * shotSpeed);
            }
        }       
    }
}
