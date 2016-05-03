﻿using System;
using UnityEngine;

namespace BetterCamera
{
	class BetterPerspectiveCameraMouse : MonoBehaviour
	{
		public KeyCode MouseOrbitButton;


		public bool ScreenEdgeMoveBreaksFollow;
		public int ScreenEdgeBorderWidth;
		public float MoveSpeed;

		public bool AllowPan;
		public bool PanBreaksFollow;
		public float PanSpeedVar;
		public float PanSpeed;

		public bool AllowRotate;
		public float RotateSpeedVar;
		public float RotateSpeed;

		public bool AllowTilt;
		public float TiltSpeedVar;
		public float TiltSpeed;

		public bool AllowZoom;
		public float ZoomSpeedVar;
		public float ZoomSpeed;

		public string RotateInputAxis = "Mouse X";
		public string TiltInputAxis = "Mouse Y";
		public string ZoomInputAxis = "Mouse ScrollWheel";
		public KeyCode PanKey1 = KeyCode.LeftShift;
		public KeyCode PanKey2 = KeyCode.RightShift;

		//

		private BetterPerspectiveCamera _BPCamera;

		//

		public void Reset()
		{
			MouseOrbitButton = KeyCode.Mouse2;   


			ScreenEdgeMoveBreaksFollow = true;
			ScreenEdgeBorderWidth = 4;
			MoveSpeed = 10f;

			AllowPan = true;
			PanBreaksFollow = true;
			PanSpeedVar = 50f;
			PanKey1 = KeyCode.LeftShift;
			PanKey2 = KeyCode.RightShift;

			AllowRotate = true;
			RotateSpeedVar = 360f;

			AllowTilt = true;
			TiltSpeedVar = 200f;

			AllowZoom = true;

			ZoomSpeedVar = 10f;

			RotateInputAxis = "Mouse X";
			TiltInputAxis = "Mouse Y";
			ZoomInputAxis = "Mouse ScrollWheel";
		}

		protected void Start()
		{

			_BPCamera = gameObject.GetComponent<BetterPerspectiveCamera>();
		}

		protected void Update()
		{
			float num = 0.02f;

			MoveSpeed = 20f;
			RotateSpeed = RotateSpeedVar;
			ZoomSpeed = ZoomSpeedVar;
			TiltSpeed = TiltSpeedVar;
			PanSpeed= PanSpeedVar;


			if (_BPCamera == null)
				return; 

			if (AllowZoom && !UIUtility.isMouseOverUIElement())
			{

				var scroll = -Input.GetAxisRaw(ZoomInputAxis);
				_BPCamera.Distance -= scroll * ZoomSpeed * num;
			}

			if (Input.GetKey(MouseOrbitButton))
			{
				if (AllowPan && (Input.GetKey(PanKey1) || Input.GetKey(PanKey2)))
				{

					var panX = -1 * Input.GetAxisRaw("Mouse X") * PanSpeed * num;
					var panZ = -1 * Input.GetAxisRaw("Mouse Y") * PanSpeed * num;

					_BPCamera.AddToPosition(panX, 0, panZ);

					if (PanBreaksFollow && (Mathf.Abs(panX) > 0.001f || Mathf.Abs(panZ) > 0.001f))
					{
						_BPCamera.EndFollow();
					}
				}
				else
				{


					if (AllowTilt)
					{
						var tilt = Input.GetAxisRaw(TiltInputAxis);
						_BPCamera.Tilt -= tilt * TiltSpeed * num;
					}

					if (AllowRotate)
					{
						var rot = Input.GetAxisRaw(RotateInputAxis);
						_BPCamera.Rotation += rot * RotateSpeed * num;
					}
				}
			}

			if (Settings.Instance.controlsEdgeScrolling && (!_BPCamera.IsFollowing || ScreenEdgeMoveBreaksFollow))
			{
				var hasMovement = false;

				if (Input.mousePosition.y > (Screen.height - ScreenEdgeBorderWidth))
				{
					hasMovement = true;
					_BPCamera.AddToPosition(0, 0, MoveSpeed * num);
				}
				else if (Input.mousePosition.y < ScreenEdgeBorderWidth)
				{
					hasMovement = true;
					_BPCamera.AddToPosition(0, 0, -1 * MoveSpeed * num);
				}

				if (Input.mousePosition.x > (Screen.width - ScreenEdgeBorderWidth))
				{
					hasMovement = true;
					_BPCamera.AddToPosition(MoveSpeed * num, 0, 0);
				}
				else if (Input.mousePosition.x < ScreenEdgeBorderWidth)
				{
					hasMovement = true;
					_BPCamera.AddToPosition(-1 * MoveSpeed * num, 0, 0);
				}

				if (hasMovement && _BPCamera.IsFollowing && ScreenEdgeMoveBreaksFollow)
				{
					_BPCamera.EndFollow();
				}
			}
		}
	}
}

