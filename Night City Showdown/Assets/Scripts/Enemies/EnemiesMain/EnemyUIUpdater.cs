using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIUpdater : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;

    public void UpdateUIHealthBar(float CurrentHealth, float MaxHealth)
    {
        healthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }
}
