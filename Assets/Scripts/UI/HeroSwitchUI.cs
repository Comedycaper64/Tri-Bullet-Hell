using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSwitchUI : MonoBehaviour
{
    private Player playerScript;

    [SerializeField] Image leftSwitchUI;
    [SerializeField] Image rightSwitchUI;

    [SerializeField] private Sprite[] UIShips;

    private void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerScript.onChangeHero += HeroChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HeroChanged()
    {
        int playerHeroTracker = playerScript.heroTracker;
        leftSwitchUI.sprite = UIShips[playerScript.CustomModulus(playerHeroTracker - 1, 3)];
        rightSwitchUI.sprite = UIShips[playerScript.CustomModulus(playerHeroTracker + 1, 3)];
    }
}
