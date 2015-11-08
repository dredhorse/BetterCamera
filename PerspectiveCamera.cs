using UnityEngine;

namespace Perspective
{
    class PerspectiveCamera : CameraController
    {
        public float CameraSensitivity = 90;
        public float ClimbSpeed = 4;
        public float NormalMoveSpeed = 10;

        private float _rotationX;
        private float _rotationY;

        private CharacterController _cc;

        void Start()
        {
            GetComponent<Camera>().farClipPlane = 150;

            transform.position = new Vector3(1, 10, 1);

            _cc = gameObject.AddComponent<CharacterController>();
            _cc.radius = 0.1f;
            _cc.height = 0.5f;
            _cc.center = new Vector3(0, 0, 0);
        }

        void Update()
        {
            if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.LeftShift))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                _rotationX += Input.GetAxis("Mouse X") * CameraSensitivity * Time.deltaTime;
                _rotationY += Input.GetAxis("Mouse Y") * CameraSensitivity * Time.deltaTime;
                _rotationY = Mathf.Clamp(_rotationY, -90, 90);
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            transform.localRotation = Quaternion.AngleAxis(_rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(_rotationY, Vector3.left);

            Vector3 pos = new Vector3();

            pos += transform.forward * NormalMoveSpeed * Input.GetAxis("Vertical");
            pos += transform.right * NormalMoveSpeed * Input.GetAxis("Horizontal");

            if (Input.GetKey(KeyCode.Q))
                pos += transform.up * ClimbSpeed;

            if (Input.GetKey(KeyCode.E))
                pos -= transform.up * ClimbSpeed;

            _cc.Move(pos * Time.deltaTime);
        }
    }
}
