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

        private static readonly int IsWalk = Animator.StringToHash("isWalk");

        private void Start()
        {
            HideCursor();
        }

        private void Update()
        {
            if (isOwned)
            {
                currentDirection = GetAxis();
                currentDirection = UpdateDirection(currentDirection);
                Move(currentDirection);
                RotateCamera();

                if (currentDirection.x == 0 && currentDirection.z == 0)
                    anim.SetBool(IsWalk, false);
                else
                    anim.SetBool(IsWalk, true);
            }
        }

        private static Vector3 GetAxis()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            var dir = new Vector3(h, 0, v);
            return dir;
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