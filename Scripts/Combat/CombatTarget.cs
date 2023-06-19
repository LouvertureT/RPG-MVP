using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {

        public bool HandleRaycast(PlayerController callingController)
        {
           
                if (!GetComponent<Fighter>().CanAttack(gameObject))
                {
                    return false;
                }
                
                if (Input.GetMouseButton(1))
                {
                    callingController.GetComponent<Fighter>().Attack(gameObject);
                }
                
                return true;
        }
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}
