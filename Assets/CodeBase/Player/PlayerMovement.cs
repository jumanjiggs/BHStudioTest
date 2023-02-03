using Mirror;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        public Vector3 currentDirection;
        
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator anim;
        [SerializeField] private float speed = 5;
        [SerializeField] private float mouseSens = 250f;
        [SerializeField] private PlayerAnimator playerAnimator;
        
        private void Update()
        {
            if (!isLocalPlayer) return;
            
            currentDirection = GetAxis();
            currentDirection = UpdateDirection(currentDirection);
            Move(currentDirection);
            RotateCamera();

            if (currentDirection.x == 0 && currentDirection.z == 0)
                playerAnimator.StopWalk();
            else
                playerAnimator.PlayWalk();
        }

        private static Vector3 GetAxis()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var direction = new Vector3(horizontal, 0, vertical);
            return direction;
        }
        
        
        private Vector3 UpdateDirection(Vector3 dir)
        {
            dir = transform.TransformDirection(dir);
            return dir;
        }

        private void RotateCamera()
        {
            var mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
            transform.Rotate(Vector3.up, mouseX);
        }

        private void Move(Vector3 dir) => characterController.Move(dir * speed * Time.deltaTime);
        private static void HideCursor() => Cursor.lockState = CursorLockMode.Locked;
    }
}