using System;
using UnityEngine;

// Token: 0x02000436 RID: 1078
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Hue, Saturation, Value")]
public class CC_HueSaturationValue : CC_Base
{
	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x060026A1 RID: 9889 RVA: 0x000BE79D File Offset: 0x000BCB9D
	// (set) Token: 0x060026A2 RID: 9890 RVA: 0x000BE7A5 File Offset: 0x000BCBA5
	public float hue
	{
		get
		{
			return this.masterHue;
		}
		set
		{
			this.masterHue = value;
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x060026A3 RID: 9891 RVA: 0x000BE7AE File Offset: 0x000BCBAE
	// (set) Token: 0x060026A4 RID: 9892 RVA: 0x000BE7B6 File Offset: 0x000BCBB6
	public float saturation
	{
		get
		{
			return this.masterSaturation;
		}
		set
		{
			this.masterSaturation = value;
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x060026A5 RID: 9893 RVA: 0x000BE7BF File Offset: 0x000BCBBF
	// (set) Token: 0x060026A6 RID: 9894 RVA: 0x000BE7C7 File Offset: 0x000BCBC7
	public float value
	{
		get
		{
			return this.masterValue;
		}
		set
		{
			this.masterValue = value;
		}
	}

	// Token: 0x060026A7 RID: 9895 RVA: 0x000BE7D0 File Offset: 0x000BCBD0
	[ImageEffectTransformsToLDR]
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Master", new Vector3(this.masterHue / 360f, this.masterSaturation * 0.01f, this.masterValue * 0.01f));
		if (this.advanced)
		{
			base.material.SetVector("_Reds", new Vector3(this.redsHue / 360f, this.redsSaturation * 0.01f, this.redsValue * 0.01f));
			base.material.SetVector("_Yellows", new Vector3(this.yellowsHue / 360f, this.yellowsSaturation * 0.01f, this.yellowsValue * 0.01f));
			base.material.SetVector("_Greens", new Vector3(this.greensHue / 360f, this.greensSaturation * 0.01f, this.greensValue * 0.01f));
			base.material.SetVector("_Cyans", new Vector3(this.cyansHue / 360f, this.cyansSaturation * 0.01f, this.cyansValue * 0.01f));
			base.material.SetVector("_Blues", new Vector3(this.bluesHue / 360f, this.bluesSaturation * 0.01f, this.bluesValue * 0.01f));
			base.material.SetVector("_Magentas", new Vector3(this.magentasHue / 360f, this.magentasSaturation * 0.01f, this.magentasValue * 0.01f));
			Graphics.Blit(source, destination, base.material, 1);
		}
		else
		{
			Graphics.Blit(source, destination, base.material, 0);
		}
	}

	// Token: 0x040013B1 RID: 5041
	[Range(-180f, 180f)]
	public float masterHue;

	// Token: 0x040013B2 RID: 5042
	[Range(-100f, 100f)]
	public float masterSaturation;

	// Token: 0x040013B3 RID: 5043
	[Range(-100f, 100f)]
	public float masterValue;

	// Token: 0x040013B4 RID: 5044
	[Range(-180f, 180f)]
	public float redsHue;

	// Token: 0x040013B5 RID: 5045
	[Range(-100f, 100f)]
	public float redsSaturation;

	// Token: 0x040013B6 RID: 5046
	[Range(-100f, 100f)]
	public float redsValue;

	// Token: 0x040013B7 RID: 5047
	[Range(-180f, 180f)]
	public float yellowsHue;

	// Token: 0x040013B8 RID: 5048
	[Range(-100f, 100f)]
	public float yellowsSaturation;

	// Token: 0x040013B9 RID: 5049
	[Range(-100f, 100f)]
	public float yellowsValue;

	// Token: 0x040013BA RID: 5050
	[Range(-180f, 180f)]
	public float greensHue;

	// Token: 0x040013BB RID: 5051
	[Range(-100f, 100f)]
	public float greensSaturation;

	// Token: 0x040013BC RID: 5052
	[Range(-100f, 100f)]
	public float greensValue;

	// Token: 0x040013BD RID: 5053
	[Range(-180f, 180f)]
	public float cyansHue;

	// Token: 0x040013BE RID: 5054
	[Range(-100f, 100f)]
	public float cyansSaturation;

	// Token: 0x040013BF RID: 5055
	[Range(-100f, 100f)]
	public float cyansValue;

	// Token: 0x040013C0 RID: 5056
	[Range(-180f, 180f)]
	public float bluesHue;

	// Token: 0x040013C1 RID: 5057
	[Range(-100f, 100f)]
	public float bluesSaturation;

	// Token: 0x040013C2 RID: 5058
	[Range(-100f, 100f)]
	public float bluesValue;

	// Token: 0x040013C3 RID: 5059
	[Range(-180f, 180f)]
	public float magentasHue;

	// Token: 0x040013C4 RID: 5060
	[Range(-100f, 100f)]
	public float magentasSaturation;

	// Token: 0x040013C5 RID: 5061
	[Range(-100f, 100f)]
	public float magentasValue;

	// Token: 0x040013C6 RID: 5062
	public bool advanced;

	// Token: 0x040013C7 RID: 5063
	public int currentChannel;
}
