using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;

    public float minX;
    public float maxX;

    float speed;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);   
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if(transform.position.x < minX)
		{
            Vector2 newPos = new Vector2(maxX, transform.position.y);
            transform.position = newPos;
		}
    }
}
