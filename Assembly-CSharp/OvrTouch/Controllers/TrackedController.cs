using System;
using UnityEngine;

namespace OvrTouch.Controllers
{
	// Token: 0x02000713 RID: 1811
	public class TrackedController : MonoBehaviour
	{
		// Token: 0x06003B20 RID: 15136 RVA: 0x00129FE4 File Offset: 0x001283E4
		public static TrackedController FindOrCreate(HandednessId handedness)
		{
			TrackedController[] array = UnityEngine.Object.FindObjectsOfType<TrackedController>();
			foreach (TrackedController trackedController in array)
			{
				if (trackedController.Handedness == handedness)
				{
					return trackedController;
				}
			}
			GameObject gameObject = new GameObject("TrackedController");
			TrackedController trackedController2 = gameObject.AddComponent<TrackedController>();
			Transform trackedTransform = null;
			OVRCameraRig ovrcameraRig = UnityEngine.Object.FindObjectOfType<OVRCameraRig>();
			if (ovrcameraRig != null)
			{
				trackedTransform = ((handedness != HandednessId.Left) ? ovrcameraRig.rightHandAnchor : ovrcameraRig.leftHandAnchor);
			}
			trackedController2.Initialize(handedness, trackedTransform);
			return trackedController2;
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x0012A072 File Offset: 0x00128472
		public void PlayHapticEvent(float frequency, float amplitude, float duration)
		{
			this.m_hapticStartTime = Time.time;
			this.m_hapticDuration = duration;
			OVRInput.SetControllerVibration(frequency, amplitude, this.m_controllerType);
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003B22 RID: 15138 RVA: 0x0012A093 File Offset: 0x00128493
		public HandednessId Handedness
		{
			get
			{
				return this.m_handedness;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x0012A09B File Offset: 0x0012849B
		public bool IsLeft
		{
			get
			{
				return this.m_handedness == HandednessId.Left;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x0012A0A6 File Offset: 0x001284A6
		public bool IsPoint
		{
			get
			{
				return this.m_point;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003B25 RID: 15141 RVA: 0x0012A0AE File Offset: 0x001284AE
		public bool IsThumbsUp
		{
			get
			{
				return this.m_thumbsUp;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003B26 RID: 15142 RVA: 0x0012A0B6 File Offset: 0x001284B6
		public bool Button1
		{
			get
			{
				return OVRInput.Get(OVRInput.Button.One, this.m_controllerType);
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003B27 RID: 15143 RVA: 0x0012A0C4 File Offset: 0x001284C4
		public bool Button2
		{
			get
			{
				return OVRInput.Get(OVRInput.Button.Two, this.m_controllerType);
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003B28 RID: 15144 RVA: 0x0012A0D2 File Offset: 0x001284D2
		public bool ButtonJoystick
		{
			get
			{
				return OVRInput.Get(OVRInput.Button.PrimaryThumbstick, this.m_controllerType);
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003B29 RID: 15145 RVA: 0x0012A0E4 File Offset: 0x001284E4
		public float Trigger
		{
			get
			{
				return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, this.m_controllerType);
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003B2A RID: 15146 RVA: 0x0012A0F2 File Offset: 0x001284F2
		public float GripTrigger
		{
			get
			{
				return OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, this.m_controllerType);
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003B2B RID: 15147 RVA: 0x0012A100 File Offset: 0x00128500
		public Vector2 Joystick
		{
			get
			{
				return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, this.m_controllerType);
			}
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x0012A10E File Offset: 0x0012850E
		private void Awake()
		{
			if (this.m_trackedTransform != null)
			{
				this.Initialize(this.m_handedness, this.m_trackedTransform);
			}
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x0012A134 File Offset: 0x00128534
		private void LateUpdate()
		{
			if (this.m_trackedTransform != null)
			{
				base.transform.position = this.m_trackedTransform.position;
				base.transform.rotation = this.m_trackedTransform.rotation;
			}
			float num = Time.time - this.m_hapticStartTime;
			if (num >= this.m_hapticDuration)
			{
				OVRInput.SetControllerVibration(0f, 0f, this.m_controllerType);
			}
			float time = Time.time;
			bool flag = !OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, this.m_controllerType);
			if (flag)
			{
				this.m_lastPoint = time;
			}
			else
			{
				this.m_lastNonPoint = time;
			}
			bool flag2 = !OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, this.m_controllerType);
			if (flag2)
			{
				this.m_lastThumb = time;
			}
			else
			{
				this.m_lastNonThumb = time;
			}
			if (flag != this.IsPoint)
			{
				bool flag3 = (flag && time - this.m_lastNonPoint > 0.05f) || (!flag && time - this.m_lastPoint > 0.05f);
				if (flag3)
				{
					this.m_point = flag;
				}
			}
			if (flag2 != this.IsThumbsUp)
			{
				bool flag4 = (flag2 && time - this.m_lastNonThumb > 0.15f) || (!flag2 && time - this.m_lastThumb > 0.15f);
				if (flag4)
				{
					this.m_thumbsUp = flag2;
				}
			}
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x0012A2A4 File Offset: 0x001286A4
		private void Initialize(HandednessId handedness, Transform trackedTransform)
		{
			if (this.m_initialized)
			{
				return;
			}
			this.m_handedness = handedness;
			this.m_controllerType = ((this.m_handedness != HandednessId.Left) ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch);
			if (trackedTransform != null)
			{
				this.m_trackedTransform = trackedTransform;
				base.transform.position = this.m_trackedTransform.position;
				base.transform.rotation = this.m_trackedTransform.rotation;
			}
			this.m_initialized = true;
		}

		// Token: 0x040023DC RID: 9180
		[SerializeField]
		private HandednessId m_handedness;

		// Token: 0x040023DD RID: 9181
		[SerializeField]
		private Transform m_trackedTransform;

		// Token: 0x040023DE RID: 9182
		private bool m_initialized;

		// Token: 0x040023DF RID: 9183
		private OVRInput.Controller m_controllerType;

		// Token: 0x040023E0 RID: 9184
		private bool m_point;

		// Token: 0x040023E1 RID: 9185
		private bool m_thumbsUp;

		// Token: 0x040023E2 RID: 9186
		private float m_lastPoint = -1f;

		// Token: 0x040023E3 RID: 9187
		private float m_lastNonPoint = -1f;

		// Token: 0x040023E4 RID: 9188
		private float m_lastThumb = -1f;

		// Token: 0x040023E5 RID: 9189
		private float m_lastNonThumb = -1f;

		// Token: 0x040023E6 RID: 9190
		private float m_hapticDuration = -1f;

		// Token: 0x040023E7 RID: 9191
		private float m_hapticStartTime = -1f;

		// Token: 0x02000714 RID: 1812
		private static class Const
		{
			// Token: 0x040023E8 RID: 9192
			public const float TriggerDebounceTime = 0.05f;

			// Token: 0x040023E9 RID: 9193
			public const float ThumbDebounceTime = 0.15f;
		}
	}
}
