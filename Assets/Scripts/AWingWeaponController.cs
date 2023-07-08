using System.Collections;
using UnityEngine;

public class AWingWeaponController : MonoBehaviour
{
	public GameObject shot;
    public Vector2 shotsPerBurst;
    public float shotPause;
	public Transform shotSpawn;
    public Vector2 fireDelay;

    public int shotSpeed;

    void Start ()
	{
        StartCoroutine(Fire());
    }

    IEnumerator Fire ()
	{
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(fireDelay.x, fireDelay.y));

            int volleySize = (int)Random.Range(shotsPerBurst.x, shotsPerBurst.y);

            for (int i = 0; i < volleySize; i++)
            {
                GameObject projectile = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

                Vector3 shotDirection = Vector3.zero - shotSpawn.position;
                projectile.GetComponent<Rigidbody>().AddForce(shotDirection * shotSpeed);

                GetComponent<AudioSource>().Play();

                yield return new WaitForSeconds(shotPause);
            }
        }
	}
}
