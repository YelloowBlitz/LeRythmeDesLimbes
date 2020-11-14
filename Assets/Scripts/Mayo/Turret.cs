﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Turret : MonoBehaviour
{
    [Header("Position parameters")]
    [SerializeField] protected Vector2Int orientation = Vector2Int.right;
    protected Vector2Int position;

    [Space][Header("Attack parameters")]
    [SerializeField] protected int attackDamage = 1;
    [SerializeField] protected int attackRate = 2;
    [SerializeField] protected int attackLoad = 0;
    [SerializeField] protected int attackDamageIncreaseOnRankUp = 1;
    [SerializeField] protected int attackRateIncreaseOnRankUp = 1;
    [SerializeField] protected bool permaAttack = false;
    [SerializeField] protected bool isOn = true;

    [Space][Header("Souls costs")]
    public int buildCost = 10;
    public int upgradeCost = 5;
    public int sellPrice = 3;
    [SerializeField] protected int rank = 0;

    [Space][Header("UI")]
    [SerializeField] protected Canvas turretMenu;
    protected CameraManager cameraManager;
    protected UpgradeButton upgradeButton;
    protected SellButton sellButton;
    protected Text upgradeCostText;
    protected Text sellPriceText;
    protected bool isMenuOn = false;

    [Space][Header("Sprites")]
    [SerializeField] protected Sprite standardSprite;
    [SerializeField] protected Sprite hoverSprite;
    protected SpriteRenderer spriteRenderer;



    
    protected MonsterManager monsterManager;


    protected void Start()
    {
        position = new Vector2Int((int)Mathf.Floor(transform.position.x), (int)Mathf.Floor(transform.position.y));
        transform.position = new Vector2(position.x + .5f, position.y + .5f);

        monsterManager = FindObjectOfType<MonsterManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraManager = FindObjectOfType<CameraManager>();
        spriteRenderer.sprite = standardSprite;
        upgradeButton = turretMenu.transform.Find("UpgradeButton").GetComponent<UpgradeButton>();
        upgradeCostText = upgradeButton.transform.Find("UpgradeCost").GetComponent<Text>();
        sellButton = turretMenu.transform.Find("SellButton").GetComponent<SellButton>();
        sellPriceText = sellButton.transform.Find("SellPrice").GetComponent<Text>();
        UpdateButtons();
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMenuOn)
        {
            if (!(upgradeButton.isHovered || sellButton.isHovered))
            {
                turretMenu.gameObject.SetActive(false);
                isMenuOn = false;
                cameraManager.shouldMove = true;
            }
        }
    }

    public void TurnOnOff()
    {
        isOn = !isOn;
    }

    public void Upgrade()
    {
        if (rank < 2)
        {
            attackDamage += attackDamageIncreaseOnRankUp;
            if (attackRate > 1)
            {
                attackRate -= attackRateIncreaseOnRankUp;
            }
            else
            {
                permaAttack = true;
            }
            monsterManager.setEnemySouls(monsterManager.getEnemySouls() - upgradeCost);
            rank++;
            upgradeCost += 5;
            sellPrice += 3;
            UpdateButtons();
        }
    }

    public void Sell()
    {
        monsterManager.setFriendlySouls(monsterManager.getFriendlySouls() + sellPrice);
        Destroy(this.gameObject);
    }

    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isMenuOn)
        {
            TurnOnOff();
        }
        if (Input.GetMouseButtonDown(1))
        {
            turretMenu.gameObject.SetActive(true);
            isMenuOn = true;
            cameraManager.shouldMove = false;
        }
    }

    protected void OnMouseEnter()
    {
        spriteRenderer.sprite = hoverSprite;
    }

    protected void OnMouseExit()
    {
        spriteRenderer.sprite = standardSprite;
    }

    protected void UpdateButtons()
    {
        if (rank == 2)
        {
            upgradeCostText.text = "-";
        }
        else
        {
            upgradeCostText.text = upgradeCost.ToString();
        }
        sellPriceText.text = sellPrice.ToString();
    }
}
