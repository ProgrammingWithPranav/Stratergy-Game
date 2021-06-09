using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] damageSprites;

    public float lifetime;

    public GameObject effect;

    public void Setup(int damage)
	{
        GetComponent<SpriteRenderer>().sprite = damageSprites[damage - 1];
	}

    void Destruction()
	{
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
	}

    void Start()
    {
        Invoke("Destruction", lifetime);
    }

    void Update()
    {
        
    }
}
