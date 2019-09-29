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

    private bool isIFrame;
    [SerializeField]
    private float iFrameTime = 1.2f;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    protected override void Start()
    {
        spellList = GameObject.Find("Spell List").GetComponent<SpellListController>();
        gameManager = GameObject.Find("GameManager").GetComponent<Photon.Pun.Demo.PunBasics.GameManager>();
        spellFunctions = gameObject.GetComponentInParent<SpellFunctions> ();
        photonView = gameObject.GetComponent<PhotonView>();
        audioSource = gameObject.GetComponent<AudioSource> ();
        renderer = gameObject.GetComponent<SpriteRenderer> ();
        if (!testingMode)
        {
            playerName = photonView.Owner.NickName;
        }
        gameObject.name = playerName;
        direction = 0;
        isIFrame = false;
        base.Start();
    }

    private void OnDisable()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        string spellName = other.gameObject.GetComponent<ScrollController>().spellName;
        if (photonView.IsMine || testingMode)
        {
            if (spellList.AddSpell (spellName))
            {

                Destroy (other.gameObject);
                gameManager.addScroll ();
                audioSource.clip = sfx [1];
                audioSource.Play ();
            }
        }
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
                this.gameObject.GetComponent<SpriteRenderer>().sprite = LookSprites[0];
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                audioSource.clip = sfx [0];
                audioSource.Play ();
                vertical = -1;
                direction = 2;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = LookSprites[2];
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                audioSource.clip = sfx [0];
                audioSource.Play ();
                horizontal = -1;
                direction = 3;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = LookSprites[3];
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                audioSource.clip = sfx [0];
                audioSource.Play ();
                horizontal = 1;
                direction = 1;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = LookSprites[1];
            }

            if(Input.GetKeyDown("a"))
            {
                LoseHP(1);
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
        if (isIFrame)
            return;
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

        StartCoroutine (IFrameFlash());
    }


    protected override void OnCantMove<T>(T component)
    {

    }

    // every 2 seconds perform the print()
    private IEnumerator IFrameFlash ()
    {
        isIFrame = true;
        float flashTime = iFrameTime/8f;
        renderer.color = new Color (1f, 1f, 1f, 0f);
        yield return new WaitForSeconds (flashTime);
        renderer.color = new Color (1f, 1f, 1f, 1f);
        yield return new WaitForSeconds (flashTime);
        renderer.color = new Color (1f, 1f, 1f, 0f);
        yield return new WaitForSeconds (flashTime);
        renderer.color = new Color (1f, 1f, 1f, 1f);
        yield return new WaitForSeconds (flashTime);
        renderer.color = new Color (1f, 1f, 1f, 0f);
        yield return new WaitForSeconds (flashTime);
        renderer.color = new Color (1f, 1f, 1f, 1f);
        yield return new WaitForSeconds (flashTime);
        renderer.color = new Color (1f, 1f, 1f, 0f);
        yield return new WaitForSeconds (flashTime);
        renderer.color = new Color (1f, 1f, 1f, 1f);
        yield return new WaitForSeconds (flashTime);
        isIFrame = false;
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
