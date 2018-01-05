using System;
using UnityEngine;

namespace OvrTouch.Controllers
{
	// Token: 0x0200070F RID: 1807
	public class TouchController : MonoBehaviour
	{
		// Token: 0x06003B1A RID: 15130 RVA: 0x00129DE9 File Offset: 0x001281E9
		public void SetVisible(bool visible)
		{
			this.m_meshRoot.gameObject.SetActive(visible);
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x00129DFC File Offset: 0x001281FC
		private void Start()
		{
			for (int i = 0; i < this.m_animParamIndices.Length; i++)
			{
				this.m_animParamIndices[i] = Animator.StringToHash(TouchController.Const.AnimParamNames[i]);
			}
			this.m_trackedController = TrackedController.FindOrCreate(this.m_handedness);
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x00129E48 File Offset: 0x00128248
		private void LateUpdate()
		{
			if (this.m_trackedController == null)
			{
				return;
			}
			base.transform.position = this.m_trackedController.transform.position;
			base.transform.rotation = this.m_trackedController.transform.rotation;
			float paramValue = (!this.m_trackedController.Button1) ? 0f : 1f;
			this.SetAnimParam(TouchController.AnimParamId.Button1, paramValue);
			float paramValue2 = (!this.m_trackedController.Button2) ? 0f : 1f;
			this.SetAnimParam(TouchController.AnimParamId.Button2, paramValue2);
			Vector2 joystick = this.m_trackedController.Joystick;
			this.SetAnimParam(TouchController.AnimParamId.JoyX, joystick.x);
			this.SetAnimParam(TouchController.AnimParamId.JoyY, joystick.y);
			this.SetAnimParam(TouchController.AnimParamId.Grip, this.m_trackedController.GripTrigger);
			this.SetAnimParam(TouchController.AnimParamId.Trigger, this.m_trackedController.Trigger);
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x00129F39 File Offset: 0x00128339
		private void SetAnimParam(TouchController.AnimParamId paramId, float paramValue)
		{
			this.m_animator.SetFloat(this.m_animParamIndices[(int)paramId], paramValue);
		}

		// Token: 0x040023CB RID: 9163
		[SerializeField]
		private HandednessId m_handedness;

		// Token: 0x040023CC RID: 9164
		[SerializeField]
		private Animator m_animator;

		// Token: 0x040023CD RID: 9165
		[SerializeField]
		private Transform m_meshRoot;

		// Token: 0x040023CE RID: 9166
		private TrackedController m_trackedController;

		// Token: 0x040023CF RID: 9167
		private int[] m_animParamIndices = new int[6];

		// Token: 0x02000710 RID: 1808
		private enum AnimParamId
		{
			// Token: 0x040023D1 RID: 9169
			Button1,
			// Token: 0x040023D2 RID: 9170
			Button2,
			// Token: 0x040023D3 RID: 9171
			Trigger,
			// Token: 0x040023D4 RID: 9172
			Grip,
			// Token: 0x040023D5 RID: 9173
			JoyX,
			// Token: 0x040023D6 RID: 9174
			JoyY,
			// Token: 0x040023D7 RID: 9175
			Count
		}

		// Token: 0x02000711 RID: 1809
		private static class Const
		{
			// Token: 0x040023D8 RID: 9176
			public static readonly string[] AnimParamNames = new string[]
			{
				"Button 1",
				"Button 2",
				"Trigger",
				"Grip",
				"Joy X",
				"Joy Y"
			};
		}
	}
}
