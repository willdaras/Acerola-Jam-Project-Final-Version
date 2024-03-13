using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Player;

namespace UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        public PlayerHealth health;
        public Image healthBar;
        public TMP_Text text;

        private void Start()
        {
            health = FindObjectOfType<PlayerHealth>();
        }

        private void Update()
        {
            healthBar.fillAmount = health.Health / 100.0f;
            text.text = health.Health.ToString();
        }
    }
}
