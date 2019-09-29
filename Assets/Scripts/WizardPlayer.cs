using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

public class WizardPlayer : MovingObject
{
    public delegate void OnLoseHealth (bool start);
    public static event OnLoseHealth HealthLost;

    public int maxHp = 3;
    public int hp = 3;

    private Photon.Pun.Demo.PunBasics.GameManager gameManager;

    PhotonView photonView;

    private string playerName = "";
    public GameObject healthBar;
    private SpellListController spellList;
    private SpellFunctions spellFunctions;

    private bool testingMode = false;
    
    int direction;
    public Sprite[] LookSprites;
    [SerializeField]
    private AudioClip [] sfx;
    private AudioSource audioSource;

    // Start is called before the first frame update
    protected override void Start()
    {
        spellList = GameObject.Find("Spell List").GetComponent<SpellListController>();
        gameManager = GameObject.Find("GameManager").GetComponent<Photon.Pun.Demo.PunBasics.GameManager>();
        spellFunctions = gameObject.GetComponentInParent<SpellFunctions> ();
        photonView = gameObject.GetComponent<PhotonView>();
        audioSource = gameObject.GetComponent<AudioSource> ();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Scroll")
        {
            string spellName = other.gameObject.GetComponent<ScrollController>().spellName;
            if (photonView.IsMine || testingMode)
            {
                Debug.Log(other.gameObject.GetComponent<ScrollController>().GetScrollID());
                if (spellList.AddSpell (spellName))
                {

                    if(PhotonNetwork.IsMasterClient)
                    {
                        Debug.Log("Master");
                        gameManager.DestroyScroll(other.gameObject.GetComponent<ScrollController>().GetScrollID());
                        gameManager.AddScroll();
                    }
                    else
                    {
                        Debug.Log("Not master");
                        photonView.RPC("MasterScrollPickup", RpcTarget.MasterClient, other.gameObject.GetComponent<ScrollController>().GetScrollID());
                    }
                     
                    audioSource.clip = sfx [1];
                    audioSource.Play ();
                }
            }
        }

    }

    [PunRPC]
    public void MasterScrollPickup(string scrollID)
    {
        Debug.Log("Master pickup");
        gameManager.DestroyScroll(scrollID);
        gameManager.AddScroll();
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
                audioSource.clip = sfx [0];
                audioSource.Play();
                vertical = 1;
                direction = 0;
                photonView.RPC("RotateSpriteRPC", RpcTarget.All, 0);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                audioSource.clip = sfx [0];
                audioSource.Play ();
                vertical = -1;
                direction = 2;
                photonView.RPC("RotateSpriteRPC", RpcTarget.All, 2);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                audioSource.clip = sfx [0];
                audioSource.Play ();
                horizontal = -1;
                direction = 3;
                photonView.RPC("RotateSpriteRPC", RpcTarget.All, 3);
            }
                
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                audioSource.clip = sfx [0];
                audioSource.Play ();
                horizontal = 1;
                direction = 1;
                photonView.RPC("RotateSpriteRPC", RpcTarget.All, 1);
            }
                
            // if(Input.GetKeyDown("a"))
            // {
            //     LoseHP(1);
            // }

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

    [PunRPC]
    public void RotateSpriteRPC(int dir)
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = LookSprites[dir];
    }

    public void LoseHP(int loss)
    {
        photonView.RPC("LoseHPRPC", RpcTarget.All, loss);
        if (hp <= 0)
        {
            gameManager.KillPlayer();
        }

    }
    [PunRPC]
    public void LoseHPRPC(int loss)
    {
        hp -= loss;
        Vector3 healthBarReduced = new Vector3(healthBar.transform.localScale.x * hp/maxHp, healthBar.transform.localScale.y, 1);
        healthBar.transform.localScale = healthBarReduced;
        HealthLost (photonView.IsMine || testingMode);
        audioSource.clip = sfx [2];
        audioSource.Play ();
        if (hp <= 0)
        {
            gameManager.ReducePlayers(playerName);
        }
    }


    protected override void OnCantMove<T>(T component)
    {
        
    }

    public SpellListController GetSpellList ()
    {
        return spellList;
    }

    public SpellFunctions GetSpellFunctions()
    {
        return spellFunctions;
    }

    public PhotonView GetPhotonView ()
    {
        return photonView;
    }

    public bool GetTestingMode ()
    {
        return testingMode;
    }
}
