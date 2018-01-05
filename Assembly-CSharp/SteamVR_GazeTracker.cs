using System;
using UnityEngine;

// Token: 0x02000B88 RID: 2952
public class SteamVR_GazeTracker : MonoBehaviour
{
	// Token: 0x14000075 RID: 117
	// (add) Token: 0x06005BE6 RID: 23526 RVA: 0x0020169C File Offset: 0x001FFA9C
	// (remove) Token: 0x06005BE7 RID: 23527 RVA: 0x002016D4 File Offset: 0x001FFAD4
	public event GazeEventHandler GazeOn;

	// Token: 0x14000076 RID: 118
	// (add) Token: 0x06005BE8 RID: 23528 RVA: 0x0020170C File Offset: 0x001FFB0C
	// (remove) Token: 0x06005BE9 RID: 23529 RVA: 0x00201744 File Offset: 0x001FFB44
	public event GazeEventHandler GazeOff;

	// Token: 0x06005BEA RID: 23530 RVA: 0x0020177A File Offset: 0x001FFB7A
	private void Start()
	{
	}

	// Token: 0x06005BEB RID: 23531 RVA: 0x0020177C File Offset: 0x001FFB7C
	public virtual void OnGazeOn(GazeEventArgs e)
	{
		if (this.GazeOn != null)
		{
			this.GazeOn(this, e);
		}
	}

	// Token: 0x06005BEC RID: 23532 RVA: 0x00201796 File Offset: 0x001FFB96
	public virtual void OnGazeOff(GazeEventArgs e)
	{
		if (this.GazeOff != null)
		{
			this.GazeOff(this, e);
		}
	}

	// Token: 0x06005BED RID: 23533 RVA: 0x002017B0 File Offset: 0x001FFBB0
	private void Update()
	{
		if (this.hmdTrackedObject == null)
		{
			SteamVR_TrackedObject[] array = UnityEngine.Object.FindObjectsOfType<SteamVR_TrackedObject>();
			foreach (SteamVR_TrackedObject steamVR_TrackedObject in array)
			{
				if (steamVR_TrackedObject.index == SteamVR_TrackedObject.EIndex.Hmd)
				{
					this.hmdTrackedObject = steamVR_TrackedObject.transform;
					break;
				}
			}
		}
		if (this.hmdTrackedObject)
		{
			Ray ray = new Ray(this.hmdTrackedObject.position, this.hmdTrackedObject.forward);
			Plane plane = new Plane(this.hmdTrackedObject.forward, base.transform.position);
			float d = 0f;
			if (plane.Raycast(ray, out d))
			{
				Vector3 a = this.hmdTrackedObject.position + this.hmdTrackedObject.forward * d;
				float num = Vector3.Distance(a, base.transform.position);
				if (num < this.gazeInCutoff && !this.isInGaze)
				{
					this.isInGaze = true;
					GazeEventArgs e;
					e.distance = num;
					this.OnGazeOn(e);
				}
				else if (num >= this.gazeOutCutoff && this.isInGaze)
				{
					this.isInGaze = false;
					GazeEventArgs e2;
					e2.distance = num;
					this.OnGazeOff(e2);
				}
			}
		}
	}

	// Token: 0x0400417A RID: 16762
	public bool isInGaze;

	// Token: 0x0400417D RID: 16765
	public float gazeInCutoff = 0.15f;

	// Token: 0x0400417E RID: 16766
	public float gazeOutCutoff = 0.4f;

	// Token: 0x0400417F RID: 16767
	private Transform hmdTrackedObject;
}
