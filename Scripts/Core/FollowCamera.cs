using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        public Vector3 camOffset;


        void LateUpdate()
        {
            transform.position = target.transform.position + camOffset;
        }
    }
}
