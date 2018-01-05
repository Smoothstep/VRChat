using System;
using UnityEngine;

namespace OvrTouch.Hands
{
	// Token: 0x02000719 RID: 1817
	public class Grabbable : MonoBehaviour
	{
		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003B39 RID: 15161 RVA: 0x0012A3FF File Offset: 0x001287FF
		public bool AllowOffhandGrab
		{
			get
			{
				return this.m_allowOffhandGrab;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003B3A RID: 15162 RVA: 0x0012A407 File Offset: 0x00128807
		public HandPose HandPose
		{
			get
			{
				return this.m_grabbedGrabPoint.HandPose;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06003B3B RID: 15163 RVA: 0x0012A414 File Offset: 0x00128814
		public bool IsGrabbed
		{
			get
			{
				return this.m_grabbedHand != null;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06003B3C RID: 15164 RVA: 0x0012A422 File Offset: 0x00128822
		public Hand GrabbedHand
		{
			get
			{
				return this.m_grabbedHand;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06003B3D RID: 15165 RVA: 0x0012A42A File Offset: 0x0012882A
		public Transform GrabTransform
		{
			get
			{
				return this.m_grabbedGrabPoint.GrabTransform;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06003B3E RID: 15166 RVA: 0x0012A437 File Offset: 0x00128837
		public GrabPoint[] GrabPoints
		{
			get
			{
				return this.m_grabPoints;
			}
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x0012A440 File Offset: 0x00128840
		public void GrabBegin(Hand hand, GrabPoint grabPoint)
		{
			this.m_grabbedHand = hand;
			this.m_grabbedGrabPoint = grabPoint;
			if (this.m_grabbedGrabPoint.Rigidbody != null)
			{
				this.m_grabbedKinematic = this.m_grabbedGrabPoint.Rigidbody.isKinematic;
				this.m_grabbedGrabPoint.Rigidbody.isKinematic = true;
			}
			GrabbableGrabMsg grabbableGrabMsg = new GrabbableGrabMsg
			{
				Sender = this
			};
			this.SendMsg("OnGrabBegin", grabbableGrabMsg);
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x0012A4BC File Offset: 0x001288BC
		public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
		{
			if (this.m_grabbedGrabPoint.Rigidbody != null)
			{
				this.m_grabbedGrabPoint.Rigidbody.isKinematic = this.m_grabbedKinematic;
				this.m_grabbedGrabPoint.Rigidbody.velocity = linearVelocity;
				this.m_grabbedGrabPoint.Rigidbody.angularVelocity = angularVelocity;
			}
			GrabbableGrabMsg grabbableGrabMsg = new GrabbableGrabMsg
			{
				Sender = this
			};
			this.SendMsg("OnGrabEnd", grabbableGrabMsg);
			this.m_grabbedHand = null;
			this.m_grabbedGrabPoint = null;
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x0012A548 File Offset: 0x00128948
		public void OverlapBegin(Hand hand)
		{
			GrabbableOverlapMsg grabbableOverlapMsg = new GrabbableOverlapMsg
			{
				Sender = this,
				Hand = hand
			};
			this.SendMsg("OnOverlapBegin", grabbableOverlapMsg);
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x0012A580 File Offset: 0x00128980
		public void OverlapEnd(Hand hand)
		{
			GrabbableOverlapMsg grabbableOverlapMsg = new GrabbableOverlapMsg
			{
				Sender = this,
				Hand = hand
			};
			this.SendMsg("OnOverlapEnd", grabbableOverlapMsg);
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x0012A5B8 File Offset: 0x001289B8
		private void Awake()
		{
			if (this.m_grabPoints.Length == 0)
			{
				Collider component = base.GetComponent<Collider>();
				if (component == null)
				{
					throw new ArgumentException("Grabbable: Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
				}
				this.m_grabPoints = new GrabPoint[]
				{
					new GrabPoint(component)
				};
			}
			foreach (GrabPoint grabPoint in this.m_grabPoints)
			{
				grabPoint.Initialize();
				GameObject gameObject = grabPoint.GrabCollider.gameObject;
				GrabTrigger grabTrigger = gameObject.GetComponent<GrabTrigger>();
				if (grabTrigger == null)
				{
					grabTrigger = gameObject.AddComponent<GrabTrigger>();
				}
				grabTrigger.SetGrabbable(this);
			}
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x0012A65E File Offset: 0x00128A5E
		private void SendMsg(string msgName, object msg)
		{
			base.transform.SendMessage(msgName, msg, SendMessageOptions.DontRequireReceiver);
		}

		// Token: 0x040023F7 RID: 9207
		[SerializeField]
		private bool m_allowOffhandGrab = true;

		// Token: 0x040023F8 RID: 9208
		[SerializeField]
		private GrabPoint[] m_grabPoints;

		// Token: 0x040023F9 RID: 9209
		private bool m_grabbedKinematic;

		// Token: 0x040023FA RID: 9210
		private GrabPoint m_grabbedGrabPoint;

		// Token: 0x040023FB RID: 9211
		private Hand m_grabbedHand;
	}
}
