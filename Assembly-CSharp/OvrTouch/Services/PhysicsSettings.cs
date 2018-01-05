using System;
using UnityEngine;

namespace OvrTouch.Services
{
	// Token: 0x02000726 RID: 1830
	public class PhysicsSettings : MonoBehaviour
	{
		// Token: 0x06003B89 RID: 15241 RVA: 0x0012B849 File Offset: 0x00129C49
		private void Start()
		{
			Physics.gravity = this.m_gravity;
			Physics.bounceThreshold = Mathf.Max(1f, this.m_gravity.magnitude * 0.15f);
		}

		// Token: 0x0400244C RID: 9292
		[SerializeField]
		private Vector3 m_gravity = new Vector3(0f, -6.8f, 0f);
	}
}
