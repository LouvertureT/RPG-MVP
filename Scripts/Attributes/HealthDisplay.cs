using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        //cache health and store health variable from Health.CS
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            //update the text to display the current health
            //string format operator to modify the decimal point to be sent to the screen
             GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}:{1:0}", health.GetMaxHealth(), health.GetHealth());
        }
    }
}
