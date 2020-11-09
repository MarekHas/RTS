using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MH.Games.RTS
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Health _health = null;
        [SerializeField] private GameObject _healthBarParent = null;
        [SerializeField] private Image _healthBar = null;

        private void Awake()
        {
            _health.OnHealthChanged += HandleHealthUpdated;
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= HandleHealthUpdated;
        }

        private void OnMouseEnter()
        {
            _healthBarParent.SetActive(true);
        }

        private void OnMouseExit()
        {
            _healthBarParent.SetActive(false);
        }

        private void HandleHealthUpdated(int currentHealth, int maxHealth)
        {
            _healthBar.fillAmount = (float)currentHealth / maxHealth;
        }

    }
}

