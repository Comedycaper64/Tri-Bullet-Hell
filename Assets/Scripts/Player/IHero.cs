using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHero
{

    void ChangeToHero(IHero previousHero);

    void DisableScript();

    void UpdateHealth();

    void StartRegen();

    void StopRegen();

    bool CanFire();

    bool isAbilityCooling();

    IEnumerator FireMissile();

    IEnumerator HeroAbility();

    IEnumerator RegainHealth();
}