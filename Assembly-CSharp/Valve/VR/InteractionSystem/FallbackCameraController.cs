using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BA3 RID: 2979
	[RequireComponent(typeof(Camera))]
	public class FallbackCameraController : MonoBehaviour
	{
		// Token: 0x06005C6C RID: 23660 RVA: 0x0020471A File Offset: 0x00202B1A
		private void OnEnable()
		{
			this.realTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x00204728 File Offset: 0x00202B28
		private void Update()
		{
			float num = 0f;
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				num += 1f;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				num -= 1f;
			}
			float num2 = 0f;
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				num2 += 1f;
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				num2 -= 1f;
			}
			float d = this.speed;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				d = this.shiftSpeed;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float d2 = realtimeSinceStartup - this.realTime;
			this.realTime = realtimeSinceStartup;
			Vector3 direction = new Vector3(num2, 0f, num) * d * d2;
			base.transform.position += base.transform.TransformDirection(direction);
			Vector3 mousePosition = Input.mousePosition;
			if (Input.GetMouseButtonDown(1))
			{
				this.startMousePosition = mousePosition;
				this.startEulerAngles = base.transform.localEulerAngles;
			}
			if (Input.GetMouseButton(1))
			{
				Vector3 vector = mousePosition - this.startMousePosition;
				base.transform.localEulerAngles = this.startEulerAngles + new Vector3(-vector.y * 360f / (float)Screen.height, vector.x * 360f / (float)Screen.width, 0f);
			}
		}

		// Token: 0x06005C6E RID: 23662 RVA: 0x002048DC File Offset: 0x00202CDC
		private void OnGUI()
		{
			if (this.showInstructions)
			{
				GUI.Label(new Rect(10f, 10f, 600f, 400f), "WASD/Arrow Keys to translate the camera\nRight mouse click to rotate the camera\nLeft mouse click for standard interactions.\n");
			}
		}

		// Token: 0x040041FC RID: 16892
		public float speed = 4f;

		// Token: 0x040041FD RID: 16893
		public float shiftSpeed = 16f;

		// Token: 0x040041FE RID: 16894
		public bool showInstructions = true;

		// Token: 0x040041FF RID: 16895
		private Vector3 startEulerAngles;

		// Token: 0x04004200 RID: 16896
		private Vector3 startMousePosition;

		// Token: 0x04004201 RID: 16897
		private float realTime;
	}
}
