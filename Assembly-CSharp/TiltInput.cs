using System;
using UnityEngine;

// Token: 0x02000A0D RID: 2573
public class TiltInput : MonoBehaviour
{
	// Token: 0x06004E21 RID: 20001 RVA: 0x001A26C6 File Offset: 0x001A0AC6
	private void OnEnable()
	{
		if (this.mapping.type == TiltInput.AxisMapping.MappingType.NamedAxis)
		{
			this.steerAxis = new CrossPlatformInput.VirtualAxis(this.mapping.axisName);
		}
	}

	// Token: 0x06004E22 RID: 20002 RVA: 0x001A26F0 File Offset: 0x001A0AF0
	private void Update()
	{
		float value = 0f;
		if (Input.acceleration != Vector3.zero)
		{
			TiltInput.AxisOptions axisOptions = this.tiltAroundAxis;
			if (axisOptions != TiltInput.AxisOptions.ForwardAxis)
			{
				if (axisOptions == TiltInput.AxisOptions.SidewaysAxis)
				{
					value = Mathf.Atan2(Input.acceleration.z, -Input.acceleration.y) * 57.29578f + this.centreAngleOffset;
				}
			}
			else
			{
				value = Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y) * 57.29578f + this.centreAngleOffset;
			}
		}
		float num = Mathf.InverseLerp(-this.fullTiltAngle, this.fullTiltAngle, value) * 2f - 1f;
		switch (this.mapping.type)
		{
		case TiltInput.AxisMapping.MappingType.NamedAxis:
			this.steerAxis.Update(num);
			break;
		case TiltInput.AxisMapping.MappingType.MousePositionX:
			CrossPlatformInput.SetVirtualMousePositionX(num * (float)Screen.width);
			break;
		case TiltInput.AxisMapping.MappingType.MousePositionY:
			CrossPlatformInput.SetVirtualMousePositionY(num * (float)Screen.width);
			break;
		case TiltInput.AxisMapping.MappingType.MousePositionZ:
			CrossPlatformInput.SetVirtualMousePositionZ(num * (float)Screen.width);
			break;
		}
	}

	// Token: 0x06004E23 RID: 20003 RVA: 0x001A282D File Offset: 0x001A0C2D
	private void OnDisable()
	{
		this.steerAxis.Remove();
	}

	// Token: 0x04003608 RID: 13832
	public TiltInput.AxisMapping mapping;

	// Token: 0x04003609 RID: 13833
	public TiltInput.AxisOptions tiltAroundAxis;

	// Token: 0x0400360A RID: 13834
	public float fullTiltAngle = 25f;

	// Token: 0x0400360B RID: 13835
	public float centreAngleOffset;

	// Token: 0x0400360C RID: 13836
	private CrossPlatformInput.VirtualAxis steerAxis;

	// Token: 0x02000A0E RID: 2574
	public enum AxisOptions
	{
		// Token: 0x0400360E RID: 13838
		ForwardAxis,
		// Token: 0x0400360F RID: 13839
		SidewaysAxis
	}

	// Token: 0x02000A0F RID: 2575
	[Serializable]
	public class AxisMapping
	{
		// Token: 0x04003610 RID: 13840
		public TiltInput.AxisMapping.MappingType type;

		// Token: 0x04003611 RID: 13841
		public string axisName;

		// Token: 0x02000A10 RID: 2576
		public enum MappingType
		{
			// Token: 0x04003613 RID: 13843
			NamedAxis,
			// Token: 0x04003614 RID: 13844
			MousePositionX,
			// Token: 0x04003615 RID: 13845
			MousePositionY,
			// Token: 0x04003616 RID: 13846
			MousePositionZ
		}
	}
}
