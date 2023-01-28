using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplay : MonoBehaviour
{
    private Player playerScript;
    [SerializeField] private GameObject readyText;
    [SerializeField] private GameObject chargingText;

    // Start is called before the first frame update
    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        readyText.SetActive(true);
        chargingText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.currentHeroScript.isAbilityCooling() && readyText.activeInHierarchy)
        {
            readyText.SetActive(false);
            chargingText.SetActive(true);
        }
        else if (!playerScript.currentHeroScript.isAbilityCooling() && chargingText.activeInHierarchy)
        {
            readyText.SetActive(true);
            chargingText.SetActive(false);
        }
    }
}
