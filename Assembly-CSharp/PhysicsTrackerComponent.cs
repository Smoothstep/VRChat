using System;
using UnityEngine;

// Token: 0x02000AC0 RID: 2752
public class PhysicsTrackerComponent : MonoBehaviour
{
	// Token: 0x17000C21 RID: 3105
	// (get) Token: 0x06005388 RID: 21384 RVA: 0x001CD159 File Offset: 0x001CB559
	public PhysicsState State
	{
		get
		{
			return this.physicsTracker.State;
		}
	}

	// Token: 0x17000C22 RID: 3106
	// (get) Token: 0x06005389 RID: 21385 RVA: 0x001CD166 File Offset: 0x001CB566
	public PhysicsState PreviousState
	{
		get
		{
			return this.physicsTracker.PreviousState;
		}
	}

	// Token: 0x17000C23 RID: 3107
	// (get) Token: 0x0600538A RID: 21386 RVA: 0x001CD173 File Offset: 0x001CB573
	// (set) Token: 0x0600538B RID: 21387 RVA: 0x001CD180 File Offset: 0x001CB580
	public Transform TrackedTransform
	{
		get
		{
			return this.physicsTracker.TrackedTransform;
		}
		set
		{
			this.physicsTracker.SetTrackedTransform(value);
		}
	}

	// Token: 0x0600538C RID: 21388 RVA: 0x001CD18E File Offset: 0x001CB58E
	private void Awake()
	{
		this.physicsTracker.SetTrackedTransform(base.transform);
	}

	// Token: 0x0600538D RID: 21389 RVA: 0x001CD1A1 File Offset: 0x001CB5A1
	private void LateUpdate()
	{
		this.physicsTracker.Update(Time.unscaledDeltaTime);
	}

	// Token: 0x04003AFB RID: 15099
	private PhysicsTracker physicsTracker = new PhysicsTracker();
}
