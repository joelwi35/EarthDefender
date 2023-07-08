using UnityEngine;

public class DestroyIfAtOrigin : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InvokeRepeating("CheckPosition", 5, 1);
    }
	
	// Update is called once per frame
	void CheckPosition() {
        if (transform.position == Vector3.zero)
        {
            Destroy(gameObject);
        }
	}
}
