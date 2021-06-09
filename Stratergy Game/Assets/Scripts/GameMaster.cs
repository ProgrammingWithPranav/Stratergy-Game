using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public Unit selectedUnit;

    public int playerTurn = 1;
    public int player1Gold = 100;
    public int player2Gold = 100;

    public Text player1GoldText;
    public Text player2GoldText;

    public GameObject selectedUnitSquare;

    public Image playerIndicator;
    public Sprite player1Indicator;
    public Sprite player2Indicator;

    public BarrackItem purchasedItem;

    public GameObject statsPanel;
    public Vector2 statsPannelShift;
    public Unit viewedUnit;

    public Text healthText;
    public Text armourText;
    public Text attackDamageText;
    public Text defenseDamageText;

    public void UpdateGoldText()
	{
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
    }

    public void ToggleStatsPannel(Unit unit)
	{
        if(unit.Equals(viewedUnit) == false)
		{
            statsPanel.SetActive(true);
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPannelShift;
            viewedUnit = unit;
            UpdateStatsPannel();
		}
		else
		{
            statsPanel.SetActive(false);
            viewedUnit = null;
		}
	}

    public void UpdateStatsPannel()
	{
        if(viewedUnit != null)
		{
            healthText.text = viewedUnit.health.ToString();
            armourText.text = viewedUnit.armor.ToString();
            attackDamageText.text = viewedUnit.attackDamage.ToString();
            defenseDamageText.text = viewedUnit.defenseDamage.ToString();
		}
	}

    public void MoveStatsPannel(Unit unit)
	{
		if (unit.Equals(viewedUnit))
		{
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPannelShift;
        }
	}

    public void RemoveStatsPannel(Unit unit)
	{
		if (unit.Equals(viewedUnit))
		{
            statsPanel.SetActive(false);
            viewedUnit = null;
		}
	}

    void GetGoldIncome(int playerTurn)
	{
        foreach(Village village in FindObjectsOfType<Village>())
		{
            if(village.playerNumber == playerTurn)
			{
                if(playerTurn == 1)
				{
                    player1Gold += village.goldPerTurn;
				}
				else
				{
                    player2Gold += village.goldPerTurn;
				}
			}
		}

        UpdateGoldText();
	}

    public void ResetTiles()
	{
        foreach(Tile tile in FindObjectsOfType<Tile>())
		{
            tile.Reset();
		}
	}

    void Start()
    {
        GetGoldIncome(1);
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
            EndTurn();
		}

        if(selectedUnit != null)
		{
            selectedUnitSquare.SetActive(true);
            selectedUnitSquare.transform.position = selectedUnit.transform.position;
		}
		else
		{
            selectedUnitSquare.SetActive(false);
		}
    }

    void EndTurn()
	{
        if(playerTurn == 1)
		{
            playerTurn = 2;
            playerIndicator.sprite = player2Indicator;
		}
        else if(playerTurn == 2)
		{
            playerTurn = 1;
            playerIndicator.sprite = player1Indicator;
		}
        GetGoldIncome(playerTurn);

        if(selectedUnit != null)
		{
            selectedUnit.selected = false;
            selectedUnit = null;
		}

        ResetTiles();

        foreach(Unit unit in FindObjectsOfType<Unit>())
		{
            unit.hasMoved = false;
            unit.weaponIcon.SetActive(false);
            unit.hasAttacked = false;
		}

        GetComponent<Barrack>().CloseMenus();

	}
}
