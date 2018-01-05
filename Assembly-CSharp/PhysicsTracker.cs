using System;
using UnityEngine;

// Token: 0x02000ABF RID: 2751
public class PhysicsTracker
{
	// Token: 0x0600537E RID: 21374 RVA: 0x001CCEAC File Offset: 0x001CB2AC
	public PhysicsTracker()
	{
	}

	// Token: 0x0600537F RID: 21375 RVA: 0x001CCEE0 File Offset: 0x001CB2E0
	public PhysicsTracker(Transform trackedTransform)
	{
		this.SetTrackedTransform(trackedTransform);
	}

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x06005380 RID: 21376 RVA: 0x001CCF18 File Offset: 0x001CB318
	public PhysicsState State
	{
		get
		{
			return this.currentState;
		}
	}

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x06005381 RID: 21377 RVA: 0x001CCF20 File Offset: 0x001CB320
	public PhysicsState PreviousState
	{
		get
		{
			return this.previousState;
		}
	}

	// Token: 0x17000C20 RID: 3104
	// (get) Token: 0x06005382 RID: 21378 RVA: 0x001CCF28 File Offset: 0x001CB328
	public Transform TrackedTransform
	{
		get
		{
			return this.trackedTransform;
		}
	}

	// Token: 0x06005383 RID: 21379 RVA: 0x001CCF30 File Offset: 0x001CB330
	public void SetTrackedTransform(Transform rx)
	{
		this.trackedTransform = rx;
		this.ResetPhysicsState();
	}

	// Token: 0x06005384 RID: 21380 RVA: 0x001CCF40 File Offset: 0x001CB340
	public void ResetPhysicsState()
	{
		this.currentState = (this.previousState = default(PhysicsState));
		if (this.trackedTransform != null)
		{
			this.currentState.Position = (this.previousState.Position = this.trackedTransform.position);
			this.currentState.Rotation = (this.previousState.Rotation = this.trackedTransform.rotation);
		}
	}

	// Token: 0x06005385 RID: 21381 RVA: 0x001CCFBE File Offset: 0x001CB3BE
	public void Update()
	{
		this.Update(Time.unscaledDeltaTime);
	}

	// Token: 0x06005386 RID: 21382 RVA: 0x001CCFCC File Offset: 0x001CB3CC
	public void Update(float dt)
	{
		if (this.trackedTransform == null)
		{
			return;
		}
		if (Mathf.Approximately(dt, 0f))
		{
			return;
		}
		this.previousState = this.currentState;
		this.currentState = default(PhysicsState);
		this.currentState.Position = this.trackedTransform.position;
		this.currentState.Rotation = this.trackedTransform.rotation;
		this.currentState.Velocity = (this.currentState.Position - this.previousState.Position) / dt;
		this.currentState.Acceleration = (this.currentState.Velocity - this.previousState.Velocity) / dt;
		float num = 0f;
		Vector3 zero = Vector3.zero;
		(this.currentState.Rotation * Quaternion.Inverse(this.previousState.Rotation)).ToAngleAxis(out num, out zero);
		num /= dt;
		this.currentState.AngularVelocity = zero * num;
		Vector3 vector = this.currentState.AngularVelocity - this.previousState.AngularVelocity;
		float magnitude = vector.magnitude;
		vector = ((magnitude <= 0.001f) ? Vector3.zero : (vector / magnitude * (magnitude / dt)));
		this.currentState.AngularAcceleration = vector;
	}

	// Token: 0x04003AF8 RID: 15096
	private PhysicsState currentState = default(PhysicsState);

	// Token: 0x04003AF9 RID: 15097
	private PhysicsState previousState = default(PhysicsState);

	// Token: 0x04003AFA RID: 15098
	private Transform trackedTransform;
}
