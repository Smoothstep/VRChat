using System;
using UnityEngine;

// Token: 0x02000B7C RID: 2940
public abstract class VehicleController : MonoBehaviour
{
	// Token: 0x06005BA9 RID: 23465
	public abstract void Move(float horizontalInput, float verticalInput, float yawInput, float throttleInput);
}
