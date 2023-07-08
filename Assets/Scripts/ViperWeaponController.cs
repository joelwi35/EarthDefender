using UnityEngine;

public class ViperWeaponController : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn;

    public Vector2 fireRate;    
	public Vector2 delay;

    public int shotSpeed;

    private Rigidbody rb;

    void Start ()
	{
        rb = GetComponent<Rigidbody>();
        InvokeRepeating ("Fire", Random.Range(delay.x, delay.y), Random.Range(fireRate.x, fireRate.y));
	}

	void Fire ()
	{        
		GameObject projectile = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

        Vector3 shotDirection = Vector3.zero - shotSpawn.position;
        projectile.GetComponent<Rigidbody>().AddForce(shotDirection * shotSpeed);

        GetComponent<AudioSource>().Play();
	}
}
