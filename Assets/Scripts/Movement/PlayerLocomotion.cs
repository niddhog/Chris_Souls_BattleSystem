using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CA
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHander;
        Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;

        [HideInInspector]
        public AnimationHandler animatonHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Stats")]
        [SerializeField]
        float movementSpeed = 5;

        [SerializeField]
        float rotationsSpeed = 10;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHander = GetComponent<InputHandler>();
            animatonHandler = GetComponentInChildren<AnimationHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatonHandler.Initialize();
        }

        public void Update()
        {
            float delta = Time.deltaTime;

            inputHander.TickInput(delta);

            moveDirection = cameraObject.forward * inputHander.vertical;
            moveDirection += cameraObject.right * inputHander.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;
            

            animatonHandler.UpdateAnimatorValues(inputHander.moveAmount, 0);

            if (animatonHandler.canRoate)
            {
                HandleRotation(delta);
            }
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHander.moveAmount;

            targetDir = cameraObject.forward * inputHander.vertical;
            targetDir += cameraObject.right * inputHander.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if(targetDir == Vector3.zero)
                targetDir = myTransform.forward;

            float rs = rotationsSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }
        #endregion

    }
}
