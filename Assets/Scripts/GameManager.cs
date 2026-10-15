using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;


    public GameObject currentPlayer;
    public SpriteRenderer playerSprite;
    public float playerHealth = 5;
    public float score = 0;
    public float timer = 0;
    public GameObject hazard;
    public GameObject coin;
    public float startingCoinSpeed;
    public float coinSpawnInterval = 1f;
    public int coinsSpawned = 0;


    public List<GameObject> allCoins;

    public string startText = "Hello World";
    // Start is called once before the first execution of Update after this script gets loaded into your game scene
    

    void Awake()
    {
        gameManager = this;
    }
    
    
    void Start()
    {
        Debug.Log(startText);

        currentPlayer = GameObject.Find("Player");
        playerSprite = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        playerSprite.color = Color.white;
        coinsSpawned = 0;
    }
    void Update()
    {
        if(playerHealth <= 0 )
        { 
            playerSprite.color = Color.red;
            currentPlayer.GetComponent<wasd>().enabled = false;
        }



        timer += Time.deltaTime;
        if(timer > coinSpawnInterval) //for a repeating timer, just reset timer when it hits the limit
        {
            Vector2 pos;
            pos.x = Random.Range(-9, 9);
            pos.y = Random.Range(-4, 5);
            //instatiating a coin every 3 seconds with this timer - EXCEPT every tenths
            if (coinsSpawned % 10 != 0)
            {
                GameObject newCoin = Instantiate(coin, pos, Quaternion.identity);
                newCoin.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * startingCoinSpeed);
            }
            else
            {
                GameObject newHazard = Instantiate(hazard, pos, Quaternion.identity);
                newHazard.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * startingCoinSpeed);
            }
            coinsSpawned++;
            timer = 0;
        }
    }


    public void ChangePlayerHealth(float health)
    {
        playerHealth = health;
    }
}
