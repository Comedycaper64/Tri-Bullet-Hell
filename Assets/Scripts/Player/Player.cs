using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Hero Attributes")]
    public IHero currentHeroScript;
    private FastHeroScript fastHeroScript;
    private MediumHeroScript mediumHeroScript;
    private SlowHeroScript slowHeroScript;
    public IHero[] heroes = new IHero[3];
    public int heroTracker;
    public bool playerDashing;
    public Vector3 dashDirection;
    public bool playerCharging;
    public bool playerParrying;
    public BoxCollider2D boxCollider;

    [Header("Player Stats")]
    public float invincibilityTimer;
    public float movementSpeed;
    public float turnSpeed;
    public int health;
    public float iFrameLength;
    public float damagerPerAttack;
    public SpriteRenderer playerSprite;
    public bool canChangeHeroes = true;
    public bool isDead;

    [Header("Audio")]
    public AudioSource audioSource;
    [SerializeField] private AudioClip takeDamage;
    private MusicPlayer musicPlayer;

    public delegate void VoidDelegate();
    public VoidDelegate onChangeHero;
    public VoidDelegate onChangeHealth;

    private void Awake()
    {
        movementSpeed = 0f;
        turnSpeed = 0f;
        isDead = false;
        playerDashing = false;
        playerCharging = false;
        playerParrying = false;
        health = 3;
        musicPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>();
        audioSource = GetComponent<AudioSource>();
        mediumHeroScript = GetComponent<MediumHeroScript>();
        slowHeroScript = GetComponent<SlowHeroScript>();
        fastHeroScript = GetComponent<FastHeroScript>();
        boxCollider = GetComponent<BoxCollider2D>();
        heroes[0] = fastHeroScript;
        heroes[1] = mediumHeroScript;
        heroes[2] = slowHeroScript;
        heroTracker = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = musicPlayer.currentVolume;
        ChangeToHero(mediumHeroScript);
    }

    // Update is called once per frame
    void Update()
    {
        invincibilityTimer -= Time.deltaTime;

        if (playerDashing)
        {
            transform.position += movementSpeed * Time.deltaTime * dashDirection;
        }

        if (canChangeHeroes)
        {
            if (Input.GetKey(KeyCode.Mouse0) && currentHeroScript.CanFire())
            {
                StartCoroutine(currentHeroScript.FireMissile());
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && !currentHeroScript.isAbilityCooling())
            {
                canChangeHeroes = false;
                StartCoroutine(currentHeroScript.HeroAbility());
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                heroTracker = CustomModulus(heroTracker - 1, 3);
                foreach (IHero hero in heroes)
                {
                    if (hero != heroes[heroTracker])
                    {
                        hero.DisableScript();
                    }
                }
                ChangeToHero(heroes[heroTracker]);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                heroTracker = CustomModulus(heroTracker + 1, 3);
                foreach (IHero hero in heroes)
                {
                    if (hero != heroes[heroTracker])
                    {
                        hero.DisableScript();
                    }
                }
                ChangeToHero(heroes[heroTracker]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && invincibilityTimer <= 0)
        {
            if (playerParrying)
            {
                SetBulletInactive(collision);
            }
            else if (!playerParrying && !playerDashing)
            {
                SetBulletInactive(collision);
                TakeDamage();
            }
        }
    }

    private void SetBulletInactive(Collider2D collision)
    {
        collision.gameObject.GetComponent<Bullet>().SetBulletInactive();
    }

    public void TakeDamage()
    {
        health--;
        onChangeHealth();
        if (health < 1)
        {
            Die();
        }
        else
        {
            invincibilityTimer = iFrameLength;
        }
    }

    private void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    private void ChangeToHero(IHero heroScript)
    {
        IHero oldHeroScript = currentHeroScript;
        currentHeroScript = heroScript;
        heroScript.ChangeToHero(oldHeroScript);
        onChangeHero();
    }

    /*
    public void RecalculateHP(float newMaxHP)
    {
        float oldHPRatio = currentHP / health;
        currentHP = newMaxHP * oldHPRatio;
        health = newMaxHP;
    }
    */

    public int CustomModulus(int x, int y)
    {
        if (x == -1)
        {
            return 2;
        }
        else
        {
            return (x % y);
        }
    }
}
