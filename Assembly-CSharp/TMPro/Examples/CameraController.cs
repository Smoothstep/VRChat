using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008E7 RID: 2279
	public class CameraController : MonoBehaviour
	{
		// Token: 0x06004539 RID: 17721 RVA: 0x00172C08 File Offset: 0x00171008
		private void Awake()
		{
			if (QualitySettings.vSyncCount > 0)
			{
				Application.targetFrameRate = 60;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				Input.simulateMouseWithTouches = false;
			}
			this.cameraTransform = base.transform;
			this.previousSmoothing = this.MovementSmoothing;
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x00172C67 File Offset: 0x00171067
		private void Start()
		{
			if (this.CameraTarget == null)
			{
				this.dummyTarget = new GameObject("Camera Target").transform;
				this.CameraTarget = this.dummyTarget;
			}
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x00172C9C File Offset: 0x0017109C
		private void LateUpdate()
		{
			this.GetPlayerInput();
			if (this.CameraTarget != null)
			{
				if (this.CameraMode == CameraController.CameraModes.Isometric)
				{
					this.desiredPosition = this.CameraTarget.position + Quaternion.Euler(this.ElevationAngle, this.OrbitalAngle, 0f) * new Vector3(0f, 0f, -this.FollowDistance);
				}
				else if (this.CameraMode == CameraController.CameraModes.Follow)
				{
					this.desiredPosition = this.CameraTarget.position + this.CameraTarget.TransformDirection(Quaternion.Euler(this.ElevationAngle, this.OrbitalAngle, 0f) * new Vector3(0f, 0f, -this.FollowDistance));
				}
				if (this.MovementSmoothing)
				{
					this.cameraTransform.position = Vector3.SmoothDamp(this.cameraTransform.position, this.desiredPosition, ref this.currentVelocity, this.MovementSmoothingValue * Time.fixedDeltaTime);
				}
				else
				{
					this.cameraTransform.position = this.desiredPosition;
				}
				if (this.RotationSmoothing)
				{
					this.cameraTransform.rotation = Quaternion.Lerp(this.cameraTransform.rotation, Quaternion.LookRotation(this.CameraTarget.position - this.cameraTransform.position), this.RotationSmoothingValue * Time.deltaTime);
				}
				else
				{
					this.cameraTransform.LookAt(this.CameraTarget);
				}
			}
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x00172E38 File Offset: 0x00171238
		private void GetPlayerInput()
		{
			this.moveVector = Vector3.zero;
			this.mouseWheel = Input.GetAxis("Mouse ScrollWheel");
			float num = (float)Input.touchCount;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || num > 0f)
			{
				this.mouseWheel *= 10f;
				if (Input.GetKeyDown(KeyCode.I))
				{
					this.CameraMode = CameraController.CameraModes.Isometric;
				}
				if (Input.GetKeyDown(KeyCode.F))
				{
					this.CameraMode = CameraController.CameraModes.Follow;
				}
				if (Input.GetKeyDown(KeyCode.S))
				{
					this.MovementSmoothing = !this.MovementSmoothing;
				}
				if (Input.GetMouseButton(1))
				{
					this.mouseY = Input.GetAxis("Mouse Y");
					this.mouseX = Input.GetAxis("Mouse X");
					if (this.mouseY > 0.01f || this.mouseY < -0.01f)
					{
						this.ElevationAngle -= this.mouseY * this.MoveSensitivity;
						this.ElevationAngle = Mathf.Clamp(this.ElevationAngle, this.MinElevationAngle, this.MaxElevationAngle);
					}
					if (this.mouseX > 0.01f || this.mouseX < -0.01f)
					{
						this.OrbitalAngle += this.mouseX * this.MoveSensitivity;
						if (this.OrbitalAngle > 360f)
						{
							this.OrbitalAngle -= 360f;
						}
						if (this.OrbitalAngle < 0f)
						{
							this.OrbitalAngle += 360f;
						}
					}
				}
				if (num == 1f && Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
					if (deltaPosition.y > 0.01f || deltaPosition.y < -0.01f)
					{
						this.ElevationAngle -= deltaPosition.y * 0.1f;
						this.ElevationAngle = Mathf.Clamp(this.ElevationAngle, this.MinElevationAngle, this.MaxElevationAngle);
					}
					if (deltaPosition.x > 0.01f || deltaPosition.x < -0.01f)
					{
						this.OrbitalAngle += deltaPosition.x * 0.1f;
						if (this.OrbitalAngle > 360f)
						{
							this.OrbitalAngle -= 360f;
						}
						if (this.OrbitalAngle < 0f)
						{
							this.OrbitalAngle += 360f;
						}
					}
				}
				if (Input.GetMouseButton(0))
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit, 300f, 23552))
					{
						if (raycastHit.transform == this.CameraTarget)
						{
							this.OrbitalAngle = 0f;
						}
						else
						{
							this.CameraTarget = raycastHit.transform;
							this.OrbitalAngle = 0f;
							this.MovementSmoothing = this.previousSmoothing;
						}
					}
				}
				if (Input.GetMouseButton(2))
				{
					if (this.dummyTarget == null)
					{
						this.dummyTarget = new GameObject("Camera Target").transform;
						this.dummyTarget.position = this.CameraTarget.position;
						this.dummyTarget.rotation = this.CameraTarget.rotation;
						this.CameraTarget = this.dummyTarget;
						this.previousSmoothing = this.MovementSmoothing;
						this.MovementSmoothing = false;
					}
					else if (this.dummyTarget != this.CameraTarget)
					{
						this.dummyTarget.position = this.CameraTarget.position;
						this.dummyTarget.rotation = this.CameraTarget.rotation;
						this.CameraTarget = this.dummyTarget;
						this.previousSmoothing = this.MovementSmoothing;
						this.MovementSmoothing = false;
					}
					this.mouseY = Input.GetAxis("Mouse Y");
					this.mouseX = Input.GetAxis("Mouse X");
					this.moveVector = this.cameraTransform.TransformDirection(this.mouseX, this.mouseY, 0f);
					this.dummyTarget.Translate(-this.moveVector, Space.World);
				}
			}
			if (num == 2f)
			{
				Touch touch = Input.GetTouch(0);
				Touch touch2 = Input.GetTouch(1);
				Vector2 a = touch.position - touch.deltaPosition;
				Vector2 b = touch2.position - touch2.deltaPosition;
				float magnitude = (a - b).magnitude;
				float magnitude2 = (touch.position - touch2.position).magnitude;
				float num2 = magnitude - magnitude2;
				if (num2 > 0.01f || num2 < -0.01f)
				{
					this.FollowDistance += num2 * 0.25f;
					this.FollowDistance = Mathf.Clamp(this.FollowDistance, this.MinFollowDistance, this.MaxFollowDistance);
				}
			}
			if (this.mouseWheel < -0.01f || this.mouseWheel > 0.01f)
			{
				this.FollowDistance -= this.mouseWheel * 5f;
				this.FollowDistance = Mathf.Clamp(this.FollowDistance, this.MinFollowDistance, this.MaxFollowDistance);
			}
		}

		// Token: 0x04002F27 RID: 12071
		private Transform cameraTransform;

		// Token: 0x04002F28 RID: 12072
		private Transform dummyTarget;

		// Token: 0x04002F29 RID: 12073
		public Transform CameraTarget;

		// Token: 0x04002F2A RID: 12074
		public float FollowDistance = 30f;

		// Token: 0x04002F2B RID: 12075
		public float MaxFollowDistance = 100f;

		// Token: 0x04002F2C RID: 12076
		public float MinFollowDistance = 2f;

		// Token: 0x04002F2D RID: 12077
		public float ElevationAngle = 30f;

		// Token: 0x04002F2E RID: 12078
		public float MaxElevationAngle = 85f;

		// Token: 0x04002F2F RID: 12079
		public float MinElevationAngle;

		// Token: 0x04002F30 RID: 12080
		public float OrbitalAngle;

		// Token: 0x04002F31 RID: 12081
		public CameraController.CameraModes CameraMode;

		// Token: 0x04002F32 RID: 12082
		public bool MovementSmoothing = true;

		// Token: 0x04002F33 RID: 12083
		public bool RotationSmoothing;

		// Token: 0x04002F34 RID: 12084
		private bool previousSmoothing;

		// Token: 0x04002F35 RID: 12085
		public float MovementSmoothingValue = 25f;

		// Token: 0x04002F36 RID: 12086
		public float RotationSmoothingValue = 5f;

		// Token: 0x04002F37 RID: 12087
		public float MoveSensitivity = 2f;

		// Token: 0x04002F38 RID: 12088
		private Vector3 currentVelocity = Vector3.zero;

		// Token: 0x04002F39 RID: 12089
		private Vector3 desiredPosition;

		// Token: 0x04002F3A RID: 12090
		private float mouseX;

		// Token: 0x04002F3B RID: 12091
		private float mouseY;

		// Token: 0x04002F3C RID: 12092
		private Vector3 moveVector;

		// Token: 0x04002F3D RID: 12093
		private float mouseWheel;

		// Token: 0x04002F3E RID: 12094
		private const string event_SmoothingValue = "Slider - Smoothing Value";

		// Token: 0x04002F3F RID: 12095
		private const string event_FollowDistance = "Slider - Camera Zoom";

		// Token: 0x020008E8 RID: 2280
		public enum CameraModes
		{
			// Token: 0x04002F41 RID: 12097
			Follow,
			// Token: 0x04002F42 RID: 12098
			Isometric,
			// Token: 0x04002F43 RID: 12099
			Free
		}
	}
}
