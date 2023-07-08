using UnityEngine;

public class RandomRotator : MonoBehaviour 
{
	public Vector2 tumble;
	
	void Start ()
	{
        float tumbleSpeed = (float)Random.Range((int)tumble.x, (int)tumble.y);
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumbleSpeed;
	}
}