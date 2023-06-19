using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {

        //cache health and store health variable from Health.CS
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            //update the text to display the current health
            //string format operator to modify the decimal point to be sent to the screen
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}", experience.DisplayExperience());
        }
    }
}
