using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Slider health;
    public Slider mana;
    public TMP_Text money;

    public GameObject UI_gameObject;

    public void SetMaxHealth(float maxHealth, float currenthealth)
    {
        health.minValue = 0;
        health.maxValue = maxHealth;
        health.value = currenthealth;
    }

    public void SetHealth(float currenthealth)
    {
        health.value = currenthealth;
    }

    public void SetMaxMana(float maxMana, float currentMana)
    {
        mana.minValue = 0;
        mana.maxValue = maxMana;
        mana.value = currentMana;
    }

    public void SetMana(float currentMana)
    {
        mana.value = currentMana;
    }

    public void UpdateMoney()
    {

        money.text = GameInstance.gameInstance.data.money.ToString();
    }
}
