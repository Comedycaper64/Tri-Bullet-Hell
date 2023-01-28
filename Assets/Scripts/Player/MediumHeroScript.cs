using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumHeroScript : MonoBehaviour, IHero
{
    private Player playerScript;
    public bool scriptEnabled;
    private bool canFire;
    private bool abilityCooling;

    private const float mediumShipColliderX = 1.3f;
    private const float mediumShipColliderY = 1f;
    private const float healthRegenRate = 3f;

    private Coroutine regenCoroutine;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] public int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private float iFrameLength;
    [SerializeField] private GameObject missile;
    [SerializeField] private GameObject chargedMissile;
    [SerializeField] private int damagerPerAttack;
    [SerializeField] private float attackInterval;
    [SerializeField] private float missileSpeed;
    [SerializeField] private float missileLifetime;
    [SerializeField] private int chargedMissileDamage;
    [SerializeField] private float chargedMissileSpeed;
    [SerializeField] private float chargedMissileLifetime;
    [SerializeField] private float abilityDuration;
    [SerializeField] private float abilityCooldown;
    //Will become Animator instead of sprite
    [SerializeField] private Sprite heroSprite;

    [Header("Audio")]
    [SerializeField] private AudioClip fireMissileSFX;
    [SerializeField] private AudioClip chargeHeavyMissileSFX;
    [SerializeField] private AudioClip fireHeavyMissileSFX;
    [SerializeField] private AudioClip regainShieldSFX;

    public delegate void VoidDelegate();
    public VoidDelegate onRegenHealth;

    private void Awake()
    {
        playerScript = GetComponent<Player>();
        scriptEnabled = false;
        canFire = true;
        abilityCooling = false;
        health = maxHealth;
    }

    public void ChangeToHero(IHero previousHero)
    {
        if (previousHero != null)
        {
            previousHero.UpdateHealth();
            previousHero.StartRegen();
        }
        if (regenCoroutine != null)
        {
            StopRegen();
        }
        StopCoroutine(RegainHealth());
        scriptEnabled = true;
        playerScript.movementSpeed = this.movementSpeed;
        playerScript.turnSpeed = this.turnSpeed;
        playerScript.health = health;
        playerScript.iFrameLength = this.iFrameLength;
        playerScript.damagerPerAttack = this.damagerPerAttack;
        playerScript.playerSprite.sprite = heroSprite;
        playerScript.boxCollider.size = new Vector2(mediumShipColliderX, mediumShipColliderY);
    }

    public void DisableScript()
    {
        scriptEnabled = false;
    }

    public void UpdateHealth()
    {
        health = playerScript.health;
    }

    public void StartRegen()
    {
        regenCoroutine = StartCoroutine(RegainHealth());
    }

    public void StopRegen()
    {
        StopCoroutine(regenCoroutine);
    }

    public bool CanFire()
    {
        return canFire;
    }

    public IEnumerator FireMissile()
    {
        Missile tempMissile = Instantiate(missile, transform.position, transform.rotation).GetComponent<Missile>();
        tempMissile.damage = damagerPerAttack;
        tempMissile.lifetime = missileLifetime;
        tempMissile.speed = missileSpeed;
        playerScript.audioSource.PlayOneShot(fireMissileSFX);
        canFire = false;
        yield return new WaitForSeconds(attackInterval);
        canFire = true;       
    }

    private void FireChargedMissile()
    {
        Missile tempMissile = Instantiate(chargedMissile, transform.position, transform.rotation).GetComponent<Missile>();
        tempMissile.damage = chargedMissileDamage;
        tempMissile.lifetime = chargedMissileLifetime;
        tempMissile.speed = chargedMissileSpeed;
        playerScript.audioSource.PlayOneShot(fireHeavyMissileSFX);
    }

    public IEnumerator HeroAbility()
    {
        playerScript.playerCharging = true;
        playerScript.audioSource.PlayOneShot(chargeHeavyMissileSFX);
        yield return new WaitForSeconds(abilityDuration);
        playerScript.playerCharging = false;
        abilityCooling = true;
        FireChargedMissile();
        StartCoroutine(AbilityCooldown());
        playerScript.canChangeHeroes = true;
    }

    private IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);
        abilityCooling = false;
    }

    public bool isAbilityCooling()
    {
        return abilityCooling;
    }

    public IEnumerator RegainHealth()
    {
        yield return new WaitForSeconds(healthRegenRate);
        if (health < maxHealth)
        {
            health++;
            playerScript.audioSource.PlayOneShot(regainShieldSFX);
            onRegenHealth();
            StartCoroutine(RegainHealth());
        }
    }
}
