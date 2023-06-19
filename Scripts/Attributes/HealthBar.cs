using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas canvasComponent = null;


        private void Start()
        {
         
        }
        // Update is called once per frame
        void Update()
        {
            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
            if (healthComponent.GetFraction() != 0)
            {
                canvasComponent.enabled = true;
            }
           if (Mathf.Approximately(healthComponent.GetFraction(), 1) || Mathf.Approximately(healthComponent.GetFraction(), 0))
            {
                canvasComponent.enabled = false;
            }
           
        }
    }

}