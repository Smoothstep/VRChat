using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000669 RID: 1641
public class OVRBoundary
{
	// Token: 0x06003794 RID: 14228 RVA: 0x0011B9E5 File Offset: 0x00119DE5
	public bool GetConfigured()
	{
		return OVRPlugin.GetBoundaryConfigured();
	}

	// Token: 0x06003795 RID: 14229 RVA: 0x0011B9EC File Offset: 0x00119DEC
	public OVRBoundary.BoundaryTestResult TestNode(OVRBoundary.Node node, OVRBoundary.BoundaryType boundaryType)
	{
		OVRPlugin.BoundaryTestResult boundaryTestResult = OVRPlugin.TestBoundaryNode((OVRPlugin.Node)node, (OVRPlugin.BoundaryType)boundaryType);
		return new OVRBoundary.BoundaryTestResult
		{
			IsTriggering = (boundaryTestResult.IsTriggering == OVRPlugin.Bool.True),
			ClosestDistance = boundaryTestResult.ClosestDistance,
			ClosestPoint = boundaryTestResult.ClosestPoint.FromFlippedZVector3f(),
			ClosestPointNormal = boundaryTestResult.ClosestPointNormal.FromFlippedZVector3f()
		};
	}

	// Token: 0x06003796 RID: 14230 RVA: 0x0011BA54 File Offset: 0x00119E54
	public OVRBoundary.BoundaryTestResult TestPoint(Vector3 point, OVRBoundary.BoundaryType boundaryType)
	{
		OVRPlugin.BoundaryTestResult boundaryTestResult = OVRPlugin.TestBoundaryPoint(point.ToFlippedZVector3f(), (OVRPlugin.BoundaryType)boundaryType);
		return new OVRBoundary.BoundaryTestResult
		{
			IsTriggering = (boundaryTestResult.IsTriggering == OVRPlugin.Bool.True),
			ClosestDistance = boundaryTestResult.ClosestDistance,
			ClosestPoint = boundaryTestResult.ClosestPoint.FromFlippedZVector3f(),
			ClosestPointNormal = boundaryTestResult.ClosestPointNormal.FromFlippedZVector3f()
		};
	}

	// Token: 0x06003797 RID: 14231 RVA: 0x0011BAC0 File Offset: 0x00119EC0
	public void SetLookAndFeel(OVRBoundary.BoundaryLookAndFeel lookAndFeel)
	{
		OVRPlugin.BoundaryLookAndFeel boundaryLookAndFeel = new OVRPlugin.BoundaryLookAndFeel
		{
			Color = lookAndFeel.Color.ToColorf()
		};
		OVRPlugin.SetBoundaryLookAndFeel(boundaryLookAndFeel);
	}

	// Token: 0x06003798 RID: 14232 RVA: 0x0011BAF1 File Offset: 0x00119EF1
	public void ResetLookAndFeel()
	{
		OVRPlugin.ResetBoundaryLookAndFeel();
	}

	// Token: 0x06003799 RID: 14233 RVA: 0x0011BAFC File Offset: 0x00119EFC
	public Vector3[] GetGeometry(OVRBoundary.BoundaryType boundaryType)
	{
		int num = 0;
		if (OVRPlugin.GetBoundaryGeometry2((OVRPlugin.BoundaryType)boundaryType, IntPtr.Zero, ref num))
		{
			int num2 = num * OVRBoundary.cachedVector3fSize;
			if (OVRBoundary.cachedGeometryNativeBuffer.GetCapacity() < num2)
			{
				OVRBoundary.cachedGeometryNativeBuffer.Reset(num2);
			}
			int num3 = num * 3;
			if (OVRBoundary.cachedGeometryManagedBuffer.Length < num3)
			{
				OVRBoundary.cachedGeometryManagedBuffer = new float[num3];
			}
			if (OVRPlugin.GetBoundaryGeometry2((OVRPlugin.BoundaryType)boundaryType, OVRBoundary.cachedGeometryNativeBuffer.GetPointer(0), ref num))
			{
				Marshal.Copy(OVRBoundary.cachedGeometryNativeBuffer.GetPointer(0), OVRBoundary.cachedGeometryManagedBuffer, 0, num3);
				Vector3[] array = new Vector3[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = new OVRPlugin.Vector3f
					{
						x = OVRBoundary.cachedGeometryManagedBuffer[3 * i],
						y = OVRBoundary.cachedGeometryManagedBuffer[3 * i + 1],
						z = OVRBoundary.cachedGeometryManagedBuffer[3 * i + 2]
					}.FromFlippedZVector3f();
				}
				return array;
			}
		}
		return new Vector3[0];
	}

	// Token: 0x0600379A RID: 14234 RVA: 0x0011BC03 File Offset: 0x0011A003
	public Vector3 GetDimensions(OVRBoundary.BoundaryType boundaryType)
	{
		return OVRPlugin.GetBoundaryDimensions((OVRPlugin.BoundaryType)boundaryType).FromVector3f();
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x0011BC10 File Offset: 0x0011A010
	public bool GetVisible()
	{
		return OVRPlugin.GetBoundaryVisible();
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x0011BC17 File Offset: 0x0011A017
	public void SetVisible(bool value)
	{
		OVRPlugin.SetBoundaryVisible(value);
	}

	// Token: 0x04002034 RID: 8244
	private static int cachedVector3fSize = Marshal.SizeOf(typeof(OVRPlugin.Vector3f));

	// Token: 0x04002035 RID: 8245
	private static OVRNativeBuffer cachedGeometryNativeBuffer = new OVRNativeBuffer(0);

	// Token: 0x04002036 RID: 8246
	private static float[] cachedGeometryManagedBuffer = new float[0];

	// Token: 0x0200066A RID: 1642
	public enum Node
	{
		// Token: 0x04002038 RID: 8248
		HandLeft = 3,
		// Token: 0x04002039 RID: 8249
		HandRight,
		// Token: 0x0400203A RID: 8250
		Head = 9
	}

	// Token: 0x0200066B RID: 1643
	public enum BoundaryType
	{
		// Token: 0x0400203C RID: 8252
		OuterBoundary = 1,
		// Token: 0x0400203D RID: 8253
		PlayArea = 256
	}

	// Token: 0x0200066C RID: 1644
	public struct BoundaryTestResult
	{
		// Token: 0x0400203E RID: 8254
		public bool IsTriggering;

		// Token: 0x0400203F RID: 8255
		public float ClosestDistance;

		// Token: 0x04002040 RID: 8256
		public Vector3 ClosestPoint;

		// Token: 0x04002041 RID: 8257
		public Vector3 ClosestPointNormal;
	}

	// Token: 0x0200066D RID: 1645
	public struct BoundaryLookAndFeel
	{
		// Token: 0x04002042 RID: 8258
		public Color Color;
	}
}
