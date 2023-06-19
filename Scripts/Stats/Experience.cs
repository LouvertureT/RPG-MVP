using UnityEngine;
using RPG.Stats;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable 
    {
        [SerializeField] float experiencePoints = 0f;

        
        public event Action onExperienceGained;

        public void GainExperience(float experience) 
        {
            experiencePoints += experience;
            onExperienceGained();
        }
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
        
        public float DisplayExperience()
        {
            return experiencePoints;
        }
        
    }
}