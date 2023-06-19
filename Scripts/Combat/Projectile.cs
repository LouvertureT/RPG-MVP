using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        
        [SerializeField] float projectileSpeed = 1.0f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 1f;
        
        Health target = null;
        GameObject instigator = null;
        float damage = 0;
        float timeSinceShot = 0f;


        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        // Update is called once per frame
        void Update()
        {
            if (target == null) return;

            // Gamedev.TV psuedo Code
            // Created a method to get half of the capsul colliders height to aim the arrow
            if(isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
            
            if(timeSinceShot > 5)
            {
                Destroy(gameObject);
               
            }
            timeSinceShot += Time.deltaTime;
        }

        public void SetTarget(Health target, GameObject instigator, float damage) 
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        //The Method for Getting the Aim
         private Vector3 GetAimLocation()
         {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
                if (targetCapsule == null)
                {
                    return target.transform.position;
                }
                return target.transform.position + Vector3.up * targetCapsule.height / 2;     
         }
        private void OnTriggerEnter(Collider other)
        {
           
          if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);
            projectileSpeed = 0;
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
        
        
    }

}