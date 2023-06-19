using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        //cache health and store health variable from Health.CS
        BaseStats level;

        private void Awake()
        {
            level = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            //update the text to display the current health
            //string format operator to modify the decimal point to be sent to the screen
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}", level.CalculateLevel());
        }
    }
}
