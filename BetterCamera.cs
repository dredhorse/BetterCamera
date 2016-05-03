﻿using UnityEngine;

namespace BetterCamera
{
    class BetterCamera : CameraController
    {
        public float CameraSensitivity = 45;
        public float ClimbSpeed = 5;
        public float NormalMoveSpeed = 10;

        private float _rotationX = 90;
        private float _rotationY;
		private Vector3 pos;


        private CharacterController _cc;

        void Start()
        {
            GetComponent<Camera>().farClipPlane = 150;

            transform.position = new Vector3(1, 10, 1);

            _cc = gameObject.AddComponent<CharacterController>();
            _cc.radius = 0.1f;
            _cc.height = 0.4f;
            _cc.center = new Vector3(0, 0, 0);
        }

        void Update()
        {
            
        }

        void FixedUpdate()
        {
			pos = new Vector3();

            if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.LeftControl))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                _rotationX += Input.GetAxis("Mouse X") * CameraSensitivity * Time.fixedDeltaTime;
                _rotationY += Input.GetAxis("Mouse Y") * CameraSensitivity * Time.fixedDeltaTime;
                _rotationY = Mathf.Clamp(_rotationY, -90, 90);
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }



            transform.localRotation = Quaternion.AngleAxis(_rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(_rotationY, Vector3.left);

            pos += transform.forward * NormalMoveSpeed * (Input.GetAxis("Vertical") + Mathf.Clamp(-Input.GetAxis("Mouse ScrollWheel"), -2, 2));
            pos += transform.right * NormalMoveSpeed * Input.GetAxis("Horizontal");

			if (Input.GetKey(KeyCode.Q))
                pos += transform.up * ClimbSpeed;

            if (Input.GetKey(KeyCode.E))
                pos -= transform.up * ClimbSpeed;

            _cc.Move(pos * Time.fixedDeltaTime);
        }
    }
}
