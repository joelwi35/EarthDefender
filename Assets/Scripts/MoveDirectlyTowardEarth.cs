using UnityEngine;

public class MoveDirectlyTowardEarth : MonoBehaviour
{
    public Vector2 speedRange;
    
    void Update()
    {
        float speed = Random.Range(speedRange.x, speedRange.y);
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, step);
    }
}
