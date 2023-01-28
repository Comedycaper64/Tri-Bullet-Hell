using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private GameObject player;
    private Player playerScript;
    private SlowHeroScript slowHero;
    private MediumHeroScript mediumHero;
    private FastHeroScript fastHero;
    private Image Image;
    [SerializeField] private Image leftSwitchSprite;
    [SerializeField] private Image rightSwitchSprite;

    public bool fastHeroActive;
    public bool mediumHeroActive;
    public bool slowHeroActive;

    [SerializeField] private Sprite redShield;
    [SerializeField] private Sprite redShieldBroken;
    [SerializeField] private Sprite greenShield;
    [SerializeField] private Sprite greenDamagedShield;
    [SerializeField] private Sprite greenShieldBroken;
    [SerializeField] private Sprite blueShield;
    [SerializeField] private Sprite blueLightlyDamagedShield;
    [SerializeField] private Sprite blueDamagedShield;
    [SerializeField] private Sprite blueHeavilyDamagedShield;
    [SerializeField] private Sprite blueShieldBroken;
    [SerializeField] private Sprite parrySprite;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        slowHero = player.GetComponent<SlowHeroScript>();
        mediumHero = player.GetComponent<MediumHeroScript>();
        fastHero = player.GetComponent<FastHeroScript>();
        Image = GetComponent<Image>();
        fastHeroActive = false;
        mediumHeroActive = false;
        slowHeroActive = false;
        if (playerScript != null)
        {
            playerScript.onChangeHero += HeroChanged;
            playerScript.onChangeHero += HealthChanged;
            playerScript.onChangeHealth += HealthChanged;
            slowHero.onRegenHealth += HealthChanged;
            mediumHero.onRegenHealth += HealthChanged;
            fastHero.onRegenHealth += HealthChanged;
            slowHero.onParry += OnParry;
            slowHero.onParryEnd += OnParryEnd;
        }
        else
        {
            Debug.Log("Error: PlayerScript is null");
        }
    }

    private void HeroChanged()
    {
        //Key: 0 = Fast Hero, 1 = Medium Hero, 2 = Slow Hero

        if (playerScript.currentHeroScript == playerScript.heroes[0])
        {
            mediumHeroActive = false;
            slowHeroActive = false;
            fastHeroActive = true;
        }
        else if (playerScript.currentHeroScript == playerScript.heroes[1])
        {
            mediumHeroActive = true;
            slowHeroActive = false;
            fastHeroActive = false;
        }
        else if (playerScript.currentHeroScript == playerScript.heroes[2])
        {
            mediumHeroActive = false;
            slowHeroActive = true;
            fastHeroActive = false;
        }
    }

    private void OnParry()
    {
        Image.sprite = parrySprite;
    }

    private void OnParryEnd()
    {
        ChangeSlowHeroHealth(playerScript.health, Image);
    }

    private void ChangeFastHeroHealth(int health, Image renderer)
    {
        switch (health)
        {
            case (2):
                renderer.sprite = redShield;
                break;

            default:
                renderer.sprite = redShieldBroken;
                break;
        }
    }

    private void ChangeFastHeroHealth(int health, Image renderer, int leftSwitchHealth, int rightSwitchHealth)
    {
        //Key: left is slow, right is medium
        switch (health)
        {
            case (2):
                renderer.sprite = redShield;
                ChangeSlowHeroHealth(leftSwitchHealth, leftSwitchSprite);
                ChangeMediumHeroHealth(rightSwitchHealth, rightSwitchSprite);
                break;

            default:
                renderer.sprite = redShieldBroken;
                break;
        }
    }

    private void ChangeMediumHeroHealth(int health, Image renderer)
    {
        switch (health)
        {
            case (3):
                renderer.sprite = greenShield;
                break;

            case (2):
                renderer.sprite = greenDamagedShield;
                break;

            default:
                renderer.sprite = greenShieldBroken;
                break;
        }
    }

    private void ChangeMediumHeroHealth(int health, Image renderer, int leftSwitchHealth, int rightSwitchHealth)
    {
        //Key: left is fast, right is slow
        switch (health)
        {
            case (3):
                renderer.sprite = greenShield;
                ChangeFastHeroHealth(leftSwitchHealth, leftSwitchSprite);
                ChangeSlowHeroHealth(rightSwitchHealth, rightSwitchSprite);
                break;

            case (2):
                renderer.sprite = greenDamagedShield;
                ChangeFastHeroHealth(leftSwitchHealth, leftSwitchSprite);
                ChangeSlowHeroHealth(rightSwitchHealth, rightSwitchSprite);
                break;

            default:
                renderer.sprite = greenShieldBroken;
                break;
        }
    }

    private void ChangeSlowHeroHealth(int health, Image renderer)
    {
        switch (health)
        {
            case (5):
                renderer.sprite = blueShield;
                break;

            case (4):
                renderer.sprite = blueLightlyDamagedShield;
                break;

            case (3):
                renderer.sprite = blueDamagedShield;
                break;

            case (2):
                renderer.sprite = blueHeavilyDamagedShield;
                break;

            default:
                renderer.sprite = blueShieldBroken;
                break;
        }
    }

    private void ChangeSlowHeroHealth(int health, Image renderer, int leftSwitchHealth, int rightSwitchHealth)
    {
        //Key: left is medium, right is fast
        switch (health)
        {
            case (5):
                renderer.sprite = blueShield;
                ChangeMediumHeroHealth(leftSwitchHealth, leftSwitchSprite);
                ChangeFastHeroHealth(rightSwitchHealth, rightSwitchSprite);
                break;

            case (4):
                renderer.sprite = blueLightlyDamagedShield;
                ChangeMediumHeroHealth(leftSwitchHealth,leftSwitchSprite);
                ChangeFastHeroHealth(rightSwitchHealth, rightSwitchSprite);
                break;

            case (3):
                renderer.sprite = blueDamagedShield;
                ChangeMediumHeroHealth(leftSwitchHealth, leftSwitchSprite);
                ChangeFastHeroHealth(rightSwitchHealth, rightSwitchSprite);
                break;

            case (2):
                renderer.sprite = blueHeavilyDamagedShield;
                ChangeMediumHeroHealth(leftSwitchHealth, leftSwitchSprite);
                ChangeFastHeroHealth(rightSwitchHealth, rightSwitchSprite);
                break;

            default:
                renderer.sprite = blueShieldBroken;
                break;
        }
    }

    private void HealthChanged()
    {
        if (slowHeroActive)
        {
            ChangeSlowHeroHealth(playerScript.health, Image, mediumHero.health, fastHero.health);
        }
        else if (mediumHeroActive)
        {
            ChangeMediumHeroHealth(playerScript.health, Image, fastHero.health, slowHero.health);
        }
        else if (fastHeroActive)
        {
            ChangeFastHeroHealth(playerScript.health, Image, slowHero.health, mediumHero.health);
        }
        else
        {
            Image.sprite = null;
        }
    }
}
