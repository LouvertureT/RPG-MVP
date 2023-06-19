using RPG.Attributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        //cache health and store health variable from Health.CS
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            
        }

        private void Update()
        {
            //update the text to display the current health
            if(fighter.GetTarget() == null)
            {
                GetComponent<TextMeshProUGUI>().text = "None";
                return;
            }
            Health health = fighter.GetTarget();
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}:{1:0}", health.GetMaxHealth(), health.GetHealth());
        }
    }
}