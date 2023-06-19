using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        //Serialize Target
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        Health health;
        NavMeshAgent navMeshAgent;
        [SerializeField] string uniqueIdentifier = "";

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent <Health>();
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }


        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
       
        private void UpdateAnimator()
        {
            //obtain the velocity from the Nav Mesh Agent and convert the Global Velocity 
            //or the velocity of the object relative to the world space. Then convert it to
            //the local velocity using the formula from the lecture 

            // Vector3 localVelocity = transform.InverseTransformDirectorion(velocity);
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            //create a variable called speed and assign it to local velocity
            float speed = localVelocity.z;

            //set the speed to the forward speed in the animator by using GetComponent.SetFloat
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
