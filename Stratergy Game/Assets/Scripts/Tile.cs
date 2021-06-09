using UnityEngine;

public class Tile : MonoBehaviour
{
    SpriteRenderer rend;
    public Sprite[] tileGraphics;

    public float hoverAmount;
    public Color highlightedColor;
    public bool isWalkable;
    public LayerMask obstacleLayer;
    GameMaster gm;
    public Color creatableColour;
    public bool isCreatable;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        gm = FindObjectOfType<GameMaster>();
        int randTile = Random.Range(0, tileGraphics.Length);
        rend.sprite = tileGraphics[randTile];
    }

    public void SetCreatable()
	{
        rend.color = creatableColour;
        isCreatable = true;
	}

	private void OnMouseEnter()
	{
        transform.localScale += Vector3.one * hoverAmount;
	}

	private void OnMouseExit()
	{
        transform.localScale -= Vector3.one * hoverAmount;
    }

    public bool isClear()
	{
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer);
        if(obstacle != null)
		{
            return false;
		}
		else
		{
            return true;
		}
	}

    public void Highlight()
	{
        rend.color = highlightedColor;
        isWalkable = true;
	}

	public void Reset()
	{
        rend.color = Color.white;
        isWalkable = false;
        isCreatable = false;
	}

	private void OnMouseDown()
	{
		if (isWalkable && gm.selectedUnit != null)
		{
            gm.selectedUnit.Move(this.transform.position);
		}
        else if (isCreatable)
		{
            BarrackItem item = Instantiate(gm.purchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            gm.ResetTiles();
            Unit unit = item.GetComponent<Unit>();
            if(unit != null)
			{
                unit.hasMoved = true;
                unit.hasAttacked = true;
			}
		}
	}

	void Update()
    {
        
    }
}
