using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace UI
{
    public class PlayerDashes : MonoBehaviour
    {
        public PlayerController player;
        public GameObject dash1;
        public GameObject dash2;
        public GameObject dash3;

        private void Start()
        {
            player = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            dash1.SetActive(false);
            dash2.SetActive(false);
            dash3.SetActive(false);
            if (player.dashCount >= 1)
            {
                dash1.SetActive(true);
            }
            if (player.dashCount >= 2)
            {
                dash2.SetActive(true);
            }
            if (player.dashCount >= 3)
            {
                dash3.SetActive(true);
            }
        }
    }
}