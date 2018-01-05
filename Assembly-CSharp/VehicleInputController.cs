using System;
using UnityEngine;

// Token: 0x02000B7D RID: 2941
public class VehicleInputController : InputStateController
{
	// Token: 0x06005BAB RID: 23467 RVA: 0x002002D9 File Offset: 0x001FE6D9
	private void Start()
	{
		this.vehicle = base.GetComponentInParent<VehicleController>();
		this.inAxisHorizontal = VRCInputManager.FindInput("Horizontal");
		this.inAxisVertical = VRCInputManager.FindInput("Vertical");
	}

	// Token: 0x06005BAC RID: 23468 RVA: 0x00200308 File Offset: 0x001FE708
	private void FixedUpdate()
	{
		if (this.inAxisHorizontal == null || this.inAxisVertical == null)
		{
			return;
		}
		float axis = this.inAxisHorizontal.axis;
		float axis2 = this.inAxisVertical.axis;
		float axis3 = Input.GetAxis("Mouse X");
		float axis4 = Input.GetAxis("Mouse Y");
		this.vehicle.Move(axis, axis2, axis3, axis4);
	}

	// Token: 0x04004156 RID: 16726
	private VehicleController vehicle;

	// Token: 0x04004157 RID: 16727
	private VRCInput inAxisHorizontal;

	// Token: 0x04004158 RID: 16728
	private VRCInput inAxisVertical;
}
