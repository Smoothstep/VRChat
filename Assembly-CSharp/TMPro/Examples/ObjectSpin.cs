using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008EB RID: 2283
	public class ObjectSpin : MonoBehaviour
	{
		// Token: 0x06004545 RID: 17733 RVA: 0x00173648 File Offset: 0x00171A48
		private void Awake()
		{
			this.m_transform = base.transform;
			this.m_initial_Rotation = this.m_transform.rotation.eulerAngles;
			this.m_initial_Position = this.m_transform.position;
			Light component = base.GetComponent<Light>();
			this.m_lightColor = ((!(component != null)) ? Color.black : component.color);
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x001736BC File Offset: 0x00171ABC
		private void Update()
		{
			if (this.Motion == ObjectSpin.MotionType.Rotation)
			{
				this.m_transform.Rotate(0f, this.SpinSpeed * Time.deltaTime, 0f);
			}
			else if (this.Motion == ObjectSpin.MotionType.BackAndForth)
			{
				this.m_time += this.SpinSpeed * Time.deltaTime;
				this.m_transform.rotation = Quaternion.Euler(this.m_initial_Rotation.x, Mathf.Sin(this.m_time) * (float)this.RotationRange + this.m_initial_Rotation.y, this.m_initial_Rotation.z);
			}
			else
			{
				this.m_time += this.SpinSpeed * Time.deltaTime;
				float x = 15f * Mathf.Cos(this.m_time * 0.95f);
				float z = 10f;
				float y = 0f;
				this.m_transform.position = this.m_initial_Position + new Vector3(x, y, z);
				this.m_prevPOS = this.m_transform.position;
				this.frames++;
			}
		}

		// Token: 0x04002F4A RID: 12106
		public float SpinSpeed = 5f;

		// Token: 0x04002F4B RID: 12107
		public int RotationRange = 15;

		// Token: 0x04002F4C RID: 12108
		private Transform m_transform;

		// Token: 0x04002F4D RID: 12109
		private float m_time;

		// Token: 0x04002F4E RID: 12110
		private Vector3 m_prevPOS;

		// Token: 0x04002F4F RID: 12111
		private Vector3 m_initial_Rotation;

		// Token: 0x04002F50 RID: 12112
		private Vector3 m_initial_Position;

		// Token: 0x04002F51 RID: 12113
		private Color32 m_lightColor;

		// Token: 0x04002F52 RID: 12114
		private int frames;

		// Token: 0x04002F53 RID: 12115
		public ObjectSpin.MotionType Motion;

		// Token: 0x020008EC RID: 2284
		public enum MotionType
		{
			// Token: 0x04002F55 RID: 12117
			Rotation,
			// Token: 0x04002F56 RID: 12118
			BackAndForth,
			// Token: 0x04002F57 RID: 12119
			Translation
		}
	}
}
