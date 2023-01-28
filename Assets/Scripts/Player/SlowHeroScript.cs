using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowHeroScript : MonoBehaviour, IHero
{
    private Player playerScript;
    public bool scriptEnabled;
    private bool canFire;
    private bool abilityCooling;

    private const float largeShipColliderX = 2.26f;
    private const float largeShipColliderY = 1.5f;
    private const float healthRegenRate = 3f;

    private Coroutine regenCoroutine;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] public int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private float iFrameLength;
    [SerializeField] private GameObject missile;
    [SerializeField] private int damagerPerAttack;
    [SerializeField] private float attackInterval;
    [SerializeField] private float missileRotationOffset;
    [SerializeField] private float missileSpeed;
    [SerializeField] private float missileLifetime;
    [SerializeField] private float abilityDuration;
    [SerializeField] private float abilityCooldown;
    //Will become Animator instead of sprite
    [SerializeField] private Sprite heroSprite;

    [Header("Audio")]
    [SerializeField] private AudioClip fireMissileSFX;
    [SerializeField] private AudioClip parrySFX;
    [SerializeField] private AudioClip regainShieldSFX;

    public delegate void VoidDelegate();
    public VoidDelegate onRegenHealth;
    public VoidDelegate onParry;
    public VoidDelegate onParryEnd;

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
        scriptEnabled = true;
        playerScript.movementSpeed = this.movementSpeed;
        playerScript.turnSpeed = this.turnSpeed;
        playerScript.health = health;
        playerScript.iFrameLength = this.iFrameLength;
        playerScript.damagerPerAttack = this.damagerPerAttack;
        playerScript.playerSprite.sprite = heroSprite;
        playerScript.boxCollider.size = new Vector2(largeShipColliderX, largeShipColliderY);
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
        Missile[] missiles = new Missile[3];
        missiles[0] = Instantiate(missile, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + missileRotationOffset)).GetComponent<Missile>();
        missiles[1] = Instantiate(missile, transform.position, transform.rotation).GetComponent<Missile>();
        missiles[2] = Instantiate(missile, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - missileRotationOffset)).GetComponent<Missile>();
        foreach (Missile missile in missiles)
        {
            missile.damage = damagerPerAttack;
            missile.lifetime = missileLifetime;
            missile.speed = missileSpeed;
        }
        playerScript.audioSource.PlayOneShot(fireMissileSFX);
        canFire = false;
        yield return new WaitForSeconds(attackInterval);
        canFire = true;
    }

    public IEnumerator HeroAbility()
    {
        playerScript.playerParrying = true;
        playerScript.audioSource.PlayOneShot(parrySFX);
        onParry();
        yield return new WaitForSeconds(abilityDuration);
        onParryEnd();
        playerScript.playerParrying = false;
        abilityCooling = true;
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
