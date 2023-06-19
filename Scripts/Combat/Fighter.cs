using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using Unity.VisualScripting;
using UnityEngine;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        
        [SerializeField] float timeBetweenAttacks = 1.0f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
       // [SerializeField] Weapon currentWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";
        
        
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        LazyValue<Weapon> currentWeapon;

        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon); 
        }
        private void Start()
        {
            currentWeapon.ForceInit();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            
            if (target == null) return;
            if (target.IsDead()) return;
            if (!GetIsInRange(target.transform))
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);   
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
            
        }

        public Health GetTarget()
        {
            return target;
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttatchWeapon(weapon);
        }

        private void AttatchWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private Weapon SetupDefaultWeapon()
        {
            AttatchWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void AttackBehaviour()
        {
            // developer put code transform.LookAt here instead -
            // i think inside the loop will make the character LookAt() every time it initiates an attack
            // transform.LookAt(target.transform.position);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // trigger Hit()
                transform.LookAt(target.transform);
                TriggerAttack();
                timeSinceLastAttack = 0f;


            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //calling the animation -- the point of contact on the animation 
        void Hit()
        {
            if (target == null) { return; }
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                Instantiate(currentWeapon.value.GetEffect(), GetAimLocation(), target.transform.rotation);
                target.TakeDamage(gameObject, damage);
            }
            

        }
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            //check distance between target and current position
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeapon.value.GetRange();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;

            }
            if(GetIsInRange(combatTarget.transform))
            {
                return true;
            }
 
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetDamage();
            }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage) 
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
