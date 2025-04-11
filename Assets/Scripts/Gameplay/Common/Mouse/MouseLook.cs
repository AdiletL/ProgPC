using UnityEngine;

namespace Gameplay.Common.Mouse
{
    public class MouseLook : IRotate
    {
        public GameObject head;

        private float rotationX;
        private float rotationY;
        public float CurrentRotationSpeed { get; }
        

        public MouseLook(GameObject head, float rotationSpeed)
        {
            this.head = head;
            CurrentRotationSpeed = rotationSpeed;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        public void ExecuteRotate()
        {
            float mouseX = Input.GetAxis("Mouse X") * CurrentRotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * CurrentRotationSpeed * Time.deltaTime;

            rotationY += mouseX;
            rotationX -= mouseY;
            
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
            head.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
    }
}