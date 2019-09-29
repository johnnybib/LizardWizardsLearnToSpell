using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

public class WizardPlayer : MovingObject
{
 
    public int maxHp = 3;
    public int hp = 3;

    private Photon.Pun.Demo.PunBasics.GameManager gameManager;

    PhotonView photonView;

    private string playerName = "";
    public GameObject healthBar;
    private bool testingMode = false;
    
    int direction;
    public Sprite[] LookSprites;


    // Start is called before the first frame update
    protected override void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<Photon.Pun.Demo.PunBasics.GameManager>();
        photonView = gameObject.GetComponent<PhotonView>();
        if(!testingMode)
        {
            playerName = photonView.Owner.NickName;
        }
        gameObject.name = playerName;
        direction = 0;
        base.Start();
    }

    private void OnDisable()
    {
       
    }

    private void CheckIfDead()
    {
        if (hp <= 0)
        {
            gameManager.isDead();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
    protected override void AttemptMove(int xDir, int yDir)
    {
        base.AttemptMove(xDir, yDir);
        RaycastHit2D hit;

    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine || testingMode)
        {
            int horizontal = 0;
            int vertical = 0;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                vertical = 1;
                direction = 0;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = LookSprites[0];
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                vertical = -1;
                direction = 2;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = LookSprites[2];
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                horizontal = -1;
                direction = 3;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = LookSprites[3];
            }
                
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                horizontal = 1;
                direction = 1;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = LookSprites[1];
            }
                
            if(Input.GetKeyDown("a"))
            {
                loseHP(1);
            }

            //horizontal = (int)Input.GetAxisRaw("Horizontal");
            //vertical = (int)Input.GetAxisRaw("Vertical");

            if (horizontal != 0)
            {
                vertical = 0;
            }
                    

            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove(horizontal, vertical);

            }
        }
    }
    public void loseHP(int loss)
    {
        hp -= loss;
        Vector3 healthBarReduced = new Vector3(healthBar.transform.localScale.x * hp/maxHp, 1, 1);
        healthBar.transform.localScale = healthBarReduced;
        CheckIfDead();
    }

    protected override void OnCantMove<T>(T component)
    {
        
    }
}