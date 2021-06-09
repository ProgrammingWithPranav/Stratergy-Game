using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public bool selected;
	public int tileSpeed;
	public bool hasMoved;
	public float moveSpeed;
	public int playerNumber;
	public int attackRange;
	List<Unit> enemiesInRange = new List<Unit>();
	public bool hasAttacked;
	public GameObject weaponIcon;
	public int health;
	public int attackDamage;
	public int defenseDamage;
	public int armor;
	public DamageIcon damageIcon;
	public GameObject deathEffect;
    GameMaster gm;
	Animator camAnim;
	public Text kingHealth;
	public bool isKing;
	AudioSource source;
	public GameObject victoryPannel;

	void Start()
    {
        gm = FindObjectOfType<GameMaster>();
		camAnim = Camera.main.GetComponent<Animator>();
		UpdateKingHealth();
		source = GetComponent<AudioSource>();
    }

	private void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(1))
		{
			gm.ToggleStatsPannel(this);
		}
	}

	public void UpdateKingHealth()
	{
		if (isKing)
		{
			kingHealth.text = health.ToString();
		}
	}

	private void OnMouseDown()
	{
		ResetWeaponIcons();
		if (selected)
		{
            selected = false;
            gm.selectedUnit = null;
			gm.ResetTiles();
		}
		else
		{
			if(playerNumber == gm.playerTurn)
			{
				if (gm.selectedUnit != null)
				{
					gm.selectedUnit.selected = false;
				}
				source.Play();
				selected = true;
				gm.selectedUnit = this;
				gm.ResetTiles();
				GetEnemies();
				GetWalkableTiles();
			}
		}

		Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.150f);
		Unit unit = col.GetComponent<Unit>();
		if(gm.selectedUnit != null)
		{
			if(gm.selectedUnit.enemiesInRange.Contains(unit) && gm.selectedUnit.hasAttacked == false)
			{
				gm.selectedUnit.Attack(unit);
			}
		}
	}

	void Attack(Unit enemy)
	{
		camAnim.SetTrigger("shake");

		hasAttacked = true;

		int enemyDamage = attackDamage - enemy.armor;
		int myDamage = enemy.defenseDamage - armor;

		if(enemyDamage >= 1)
		{
			DamageIcon instance = Instantiate(damageIcon, enemy.transform.position, Quaternion.identity);
			instance.Setup(enemyDamage);
			enemy.health -= enemyDamage;
			enemy.UpdateKingHealth();
		}
		if(transform.tag == "Archer" && enemy.tag != "Archer")
		{
			if(Mathf.Abs(transform.position.x - enemy.transform.position.x) + Mathf.Abs(transform.position.y - enemy.transform.position.y) <= 1)
			{
				if (myDamage >= 1)
				{
					DamageIcon instance = Instantiate(damageIcon, transform.position, Quaternion.identity);
					instance.Setup(myDamage);
					health -= 1;
					UpdateKingHealth();
				}
			}
		}
		else
		{
			if (myDamage >= 1)
			{
				DamageIcon instance = Instantiate(damageIcon, transform.position, Quaternion.identity);
				instance.Setup(myDamage);
				health -= 1;
				UpdateKingHealth();
			}
		}

		if(enemy.health <= 0)
		{
			Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
			Destroy(enemy.gameObject);
			GetWalkableTiles();
			gm.RemoveStatsPannel(enemy);
		}

		if(health <= 0)
		{
			if (isKing == true)
			{
				victoryPannel.SetActive(true);
			}
			Instantiate(deathEffect, transform.position, Quaternion.identity);
			gm.ResetTiles();
			gm.RemoveStatsPannel(this);
			Destroy(gameObject);
		}

		gm.UpdateStatsPannel();

	}

	void GetWalkableTiles()
	{
		if(hasMoved == true)
		{
			return;
		}

		foreach(Tile tile in FindObjectsOfType<Tile>())
		{
			if(Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= tileSpeed)
			{
				if(tile.isClear() == true)
				{
					tile.Highlight();
				}
			}
		}
	}

	void GetEnemies()
	{
		enemiesInRange.Clear();

		foreach(Unit unit in FindObjectsOfType<Unit>())
		{
			if (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange)
			{
				if(unit.playerNumber != gm.playerTurn && hasAttacked == false)
				{
					enemiesInRange.Add(unit);
					unit.weaponIcon.SetActive(true);
				}
			}
		}
	}

	public void ResetWeaponIcons()
	{
		foreach(Unit unit in FindObjectsOfType<Unit>())
		{
			unit.weaponIcon.SetActive(false);
		}
	}

	public void Move(Vector2 tilePos)
	{
		gm.ResetTiles();
		StartCoroutine(StartMovement(tilePos));
	}

	IEnumerator StartMovement(Vector2 tilePos)
	{

		while(transform.position.x != tilePos.x)
		{
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePos.x, transform.position.y), moveSpeed * Time.deltaTime);
			yield return null;
		}

		while (transform.position.y != tilePos.y)
		{
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, tilePos.y), moveSpeed * Time.deltaTime);
			yield return null;
		}

		hasMoved = true;
		ResetWeaponIcons();
		GetEnemies();
		gm.MoveStatsPannel(this);
	}

	void Update()
    {
        
    }
}
