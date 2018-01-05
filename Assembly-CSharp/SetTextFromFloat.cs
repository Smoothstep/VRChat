using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AF0 RID: 2800
public class SetTextFromFloat : MonoBehaviour
{
	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x060054C6 RID: 21702 RVA: 0x001D3DB5 File Offset: 0x001D21B5
	// (set) Token: 0x060054C7 RID: 21703 RVA: 0x001D3DC0 File Offset: 0x001D21C0
	public float floatVal
	{
		get
		{
			return this._floatVal;
		}
		set
		{
			this._floatVal = value;
			if (this.UIText != null)
			{
				this.UIText.text = this.floatVal.ToString(this.floatFormat);
			}
		}
	}

	// Token: 0x04003BD2 RID: 15314
	public Text UIText;

	// Token: 0x04003BD3 RID: 15315
	public string floatFormat = "F2";

	// Token: 0x04003BD4 RID: 15316
	[SerializeField]
	private float _floatVal;
}
