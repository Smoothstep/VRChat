using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000530 RID: 1328
	[AddComponentMenu("Klak/Wiring/Convertion/Color Ramp")]
	public class ColorRamp : NodeBase
	{
		// Token: 0x1700070A RID: 1802
		// (set) Token: 0x06002E8E RID: 11918 RVA: 0x000E31CC File Offset: 0x000E15CC
		[Inlet]
		public float parameter
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				if (this._colorMode == ColorRamp.ColorMode.Gradient)
				{
					this._colorEvent.Invoke(this._gradient.Evaluate(value));
				}
				else
				{
					int num = this._colorArray.Length;
					int num2 = Mathf.FloorToInt(value * (float)(num - 1));
					num2 = Mathf.Clamp(num2, 0, num - 2);
					float t = value * (float)(num - 1) - (float)num2;
					Color arg = Color.Lerp(this._colorArray[num2], this._colorArray[num2 + 1], t);
					this._colorEvent.Invoke(arg);
				}
			}
		}

		// Token: 0x040018AF RID: 6319
		[SerializeField]
		private ColorRamp.ColorMode _colorMode;

		// Token: 0x040018B0 RID: 6320
		[SerializeField]
		private Gradient _gradient = new Gradient();

		// Token: 0x040018B1 RID: 6321
		[SerializeField]
		[ColorUsage(true, true, 0f, 16f, 0.125f, 3f)]
		private Color[] _colorArray = new Color[]
		{
			Color.black,
			Color.white
		};

		// Token: 0x040018B2 RID: 6322
		[SerializeField]
		[Outlet]
		private NodeBase.ColorEvent _colorEvent = new NodeBase.ColorEvent();

		// Token: 0x02000531 RID: 1329
		public enum ColorMode
		{
			// Token: 0x040018B4 RID: 6324
			Gradient,
			// Token: 0x040018B5 RID: 6325
			ColorArray
		}
	}
}
